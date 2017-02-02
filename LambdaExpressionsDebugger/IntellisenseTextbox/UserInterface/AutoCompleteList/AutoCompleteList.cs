using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IntellisenseTextbox.UserInterface;

namespace IntellisenseTextbox
{
    public partial class AutoCompleteList : Form
    {
        public AutoCompleteList()
        {
            InitializeComponent();
            CreateHandle();
            #region Create List Queue
            m_listQueue.Enqueue(autoCompleteItemList1);
            m_listQueue.Enqueue(autoCompleteItemList2);
            m_listQueue.Enqueue(autoCompleteItemList3);
            m_listQueue.Enqueue(autoCompleteItemList4);
            m_listQueue.Enqueue(autoCompleteItemList5);
            m_listQueue.Enqueue(autoCompleteItemList6);
            m_listQueue.Enqueue(autoCompleteItemList7);
            m_listQueue.Enqueue(autoCompleteItemList8);
            m_listQueue.Enqueue(autoCompleteItemList9);
            m_listQueue.Enqueue(autoCompleteItemList10);
            m_listQueue.Enqueue(autoCompleteItemList11);
            m_listQueue.Enqueue(autoCompleteItemList12);
            m_listQueue.Enqueue(autoCompleteItemList13);
            m_listQueue.Enqueue(autoCompleteItemList14);
            m_listQueue.Enqueue(autoCompleteItemList15);
            m_listQueue.Enqueue(autoCompleteItemList16);
            m_listQueue.Enqueue(autoCompleteItemList17);
            m_listQueue.Enqueue(autoCompleteItemList18);
            m_listQueue.Enqueue(autoCompleteItemList19);
            m_listQueue.Enqueue(autoCompleteItemList20);
            for (int i = 0; i < m_listQueue.Count; i++)
                m_listQueue.ToList()[i].Visible = (i == 0);
            #endregion
        }

        private Queue<AutoCompleteItemList> m_listQueue = new Queue<AutoCompleteItemList>(20);

        private List<ListViewItem> m_currentListOfListViewItems = new List<ListViewItem>();

        public void ShowAtPosition(Point point)
        {
            if (!Visible)
            {
                Opacity = 0f;
                Show();
                DesktopLocation = point;
                Opacity = .9f;
                Visible = true;
                MakeCurrentItemListVisible();
            }
            else
            {
                DesktopLocation = point;
                Opacity = .9f;
            }
            if (CurrentItemListControl != null)
                CurrentItemListControl.UpdateToolTip();
        }

        private void MakeCurrentItemListVisible()
        {
            foreach (var list in m_listQueue)
            {
                if (list == m_currentActiveItemListControl)
                {
                    list.Show();
                    list.Visible = true;
                }
                else
                {
                    list.Hide();
                    list.Visible = false;
                }
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible == false)
                foreach (var list in m_listQueue)
                    list.HideToolTip();
        }

        public void ChangeAutoCompleteItemIndex(int delta)
        {
            if (CurrentItemListControl.SelectedIndices.Count > 0)
            {
                int idx = CurrentItemListControl.SelectedIndices[0];
                idx = Math.Max(0, Math.Min(CurrentItemListControl.Items.Count - 1, idx + delta));
                CurrentItemListControl.Items[idx].Selected = true;
                CurrentItemListControl.Items[idx].EnsureVisible();
            }
            else
            {
                if (CurrentItemListControl.Items.Count > 0)
                    CurrentItemListControl.SelectedIndices.Add(0);
            }
        }

        private IWin32Window m_windowToShowToolTipsOn;
        public IWin32Window WindowToShowToolTipsOn
        {
            get { return m_windowToShowToolTipsOn; }
            set
            {
                m_windowToShowToolTipsOn = value;
                foreach (var list in m_listQueue)
                    list.WindowToShowToolTipsOn = m_windowToShowToolTipsOn;
            }
        }

        private Point m_upperLeftCornerRelativeToToolTipParent;
        public Point UpperLeftCornerRelativeToToolTipParent
        {
            get { return m_upperLeftCornerRelativeToToolTipParent; }
            set
            {
                m_upperLeftCornerRelativeToToolTipParent = value;
                foreach (var list in m_listQueue)
                    list.UpperLeftCornerRelativeToToolTipParent = m_upperLeftCornerRelativeToToolTipParent;
            }
        }

        public bool HasItems
        {
            get
            {
                return CurrentItemListControl.Items.Count > 0 ||
                    (CurrentItemListControl.Tag is string && (CurrentItemListControl.Tag as string).Length > 0);
            }
        }

        public bool HasAdvice
        {
            get
            {
                return CurrentItemListControl.Visible &&
                    CurrentItemListControl.Items.Count > 0 &&
                    CurrentItemListControl.SelectedItems.Count > 0;
            }
        }

        public string Advice
        {
            get
            {
                return CurrentItemListControl.SelectedItems[0].Text;
            }
        }

        public void SelectIndex(int idx)
        {
            if (idx != -1)
            {
                if (idx < CurrentItemListControl.Items.Count - 1)
                {
                    CurrentItemListControl.Items[idx].Selected = true;
                    CurrentItemListControl.Items[idx].EnsureVisible();
                }
                else
                    SelectIndexAsSoonAsAvailable(idx);
            }

        }

        private void SelectIndexAsSoonAsAvailable(int idx)
        {
            Action selectItem = () =>
                {
                    int currentRun = m_addItemsRun;
                    while (true)
                    {
                        System.Threading.Thread.Sleep(50);
                        lock (asyncItemAddLockObject)
                        {
                            if (currentRun != m_addItemsRun) break;
                            if (idx < CurrentItemListControl.Items.Count - 1)
                            {
                                Action selectAndEnsureVisible = () =>
                                {
                                    CurrentItemListControl.Items[idx].Selected = true;
                                    CurrentItemListControl.Items[idx].EnsureVisible();
                                };
                                CurrentItemListControl.Invoke(selectAndEnsureVisible);
                                break;
                            }
                        }
                    }
                };
            selectItem.BeginInvoke(null, null);
        }

        public AutoCompleteItemList CurrentItemListControl
        {
            get
            {
                return m_currentActiveItemListControl;
            }
        }

        private int m_addItemsRun = 0;
        private object asyncItemAddLockObject = new object();

        public void ApplyItemList(List<ContextItem> list)
        {
            List<ListViewItem> listViewItemList = new List<ListViewItem>();
            foreach (var item in list)
            {
                var i = new ListViewItem(item.Name, (int)item.SymbolType);
                i.Tag = item;
                listViewItemList.Add(i);
            }

            string itemListId = GetStringFromListViewItemList(listViewItemList);

            if (ItemListHasChanged(itemListId))
            {
                if (IsCached(itemListId))
                {
                    MakeActive(itemListId, listViewItemList);
                }
                else
                {
                    var listControl = GetListControlForBackgroundBuildUp();

                    foreach (var l in m_listQueue)
                        l.HideToolTip();

                    BuildItemListAsync(listControl, listViewItemList, itemListId);

                    listControl.Tag = itemListId;

                    MakeActive(listControl, listViewItemList);                    
                }
            }
            MakeCurrentItemListVisible();
        }

        private AutoCompleteItemList m_currentActiveItemListControl = null;

        private void MakeActive(AutoCompleteItemList itemList, List<ListViewItem> listViewItemList)
        {
            m_currentListOfListViewItems = listViewItemList;
            var queueList = m_listQueue.ToList();
            m_listQueue = new Queue<AutoCompleteItemList>(queueList.Except(new[] { itemList }));
            m_listQueue.Enqueue(itemList); //ganz hinten einfügen
            itemList.Show();
            itemList.Visible = true;
            queueList.Except(new[] { itemList }).ToList().ForEach(l => { l.Visible = false; l.Hide(); });
            m_currentActiveItemListControl = itemList;
        }

        private void MakeActive(string itemListId, List<ListViewItem> listViewItemList)
        {            
            var queueList = m_listQueue.ToList();
            AutoCompleteItemList itemList = queueList.Find(l => l.Tag is string && (l.Tag as string) == itemListId);
            MakeActive(itemList, listViewItemList);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            if (m_currentActiveItemListControl != null)
            {
                m_currentActiveItemListControl.Show();
                m_currentActiveItemListControl.Visible = true;
                m_listQueue.Except(new[] { m_currentActiveItemListControl }).ToList().ForEach(l => l.Visible = false);
            }
        }

        private AutoCompleteItemList GetListControlForBackgroundBuildUp()
        {
            var listControl = m_listQueue.Dequeue();
            m_listQueue.Enqueue(listControl);
            if (listControl == m_currentActiveItemListControl)
            {
                listControl = m_listQueue.Dequeue();
                m_listQueue.Enqueue(listControl);
            }
            return listControl;
        }

        private void BuildItemListAsync(AutoCompleteItemList listControl, List<ListViewItem> listViewItemList, string itemListId)
        {
            lock (asyncItemAddLockObject)
            {
                m_addItemsRun++;
            }
            Action asyncAddAllItems = () =>
            {
                int currentRun = m_addItemsRun;
                lock (asyncItemAddLockObject)
                {
                    Action clearList = () => listControl.Items.Clear();
                    this.Invoke(clearList);
                }
                int pageStart = 0;
                int pageSize = 500;
                while (true)
                {
                    pageSize = Math.Min(pageSize, listViewItemList.Count - pageStart);
                    var page = listViewItemList.Skip(pageStart).Take(pageSize);
                    Action addItem = () => listControl.Items.AddRange(page.ToArray());
                    lock (asyncItemAddLockObject)
                    {
                        if (m_addItemsRun != currentRun) break;
                        this.BeginInvoke(addItem);
                    }
                    if (pageStart + pageSize == listViewItemList.Count)
                        break;
                    pageStart += Math.Min(pageSize, listViewItemList.Count - pageSize);
                }
            };
            asyncAddAllItems.BeginInvoke(null, null);

        }

        private bool IsCached(string itemListId)
        {
            foreach (var list in m_listQueue)
                if (list.Tag is string && (list.Tag as string) == itemListId)
                    return true;
            return false;
        }

        private Dictionary<string, AutoCompleteItemList> m_cachedAutoComleteItemLists = new Dictionary<string, AutoCompleteItemList>();

        private bool ItemListHasChanged(string idOfItemList)
        {
            return (GetStringFromListViewItemList(m_currentListOfListViewItems) != idOfItemList);
        }

        private bool ItemListHasChanged(List<ListViewItem> listViewItemList)
        {
            return (GetStringFromListViewItemList(m_currentListOfListViewItems) != GetStringFromListViewItemList(listViewItemList));
        }

        private string GetStringFromListViewItemList(List<ListViewItem> listViewItems)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in listViewItems)
            {
                sb.Append(i.Text);
                sb.Append("|");
            }
            return sb.ToString();
        }

    }
}
