using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace IntellisenseTextbox.UserInterface
{
    public partial class AutoCompleteItemList : ListView
    {
        public AutoCompleteItemList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.OwnerDraw = true;
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            var textBounds = e.Bounds;
            textBounds.X += 16 + 3 + 1;
            textBounds.Width = e.Item.ListView.ClientSize.Width + 15;
            var bgBrush = Brushes.White;
            var fgBrush = Brushes.Black;
            if (e.Item.Selected)
            {
                bgBrush = SystemBrushes.Highlight;
                fgBrush = SystemBrushes.HighlightText;
            }
            e.Graphics.DrawImageUnscaled(e.Item.ListView.SmallImageList.Images[e.Item.ImageIndex], new Point(e.Bounds.X + 3, e.Bounds.Y));
            e.Graphics.FillRectangle(bgBrush, textBounds);
            textBounds.Y += 1;
            e.Graphics.DrawString(e.Item.Text, e.Item.Font, fgBrush, textBounds);

            //base.OnDrawItem(e);
        }

        private void AutoCompleteItemList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            e.Item.EnsureVisible();

            UpdateToolTip();

        }

        public void UpdateToolTip()
        {
            if (SelectedItems.Count == 0) return;
            var item = SelectedItems[0];
            ContextItem ci = item.Tag as ContextItem;
            if (ci == null || WindowToShowToolTipsOn == null) return;

            string text = null;

            switch (ci.SymbolType)
            {
                case SymbolType.Class:
                    text = string.Format("class {0}", ci.Name);
                    break;
                case SymbolType.Delegate:
                    text = string.Format("delegate {0}", ci.Name);
                    break;
                case SymbolType.Enum:
                    text = string.Format("enum {0}", ci.Name);
                    break;
                case SymbolType.EnumItem:
                    text = string.Format("{0} {0}.{1}", ci.Type.GetCompilerFriendlyName(), ci.Name);
                    break;
                case SymbolType.ExtensionMethod:
                    text = string.Format("(extension) {0} {0}.{1}()", ci.Type.GetCompilerFriendlyName(), ci.Name);
                    break;
                case SymbolType.Field:
                    text = string.Format("(local variable) {0} {1}", ci.Type.GetCompilerFriendlyName(), ci.Name);
                    break;
                case SymbolType.GenericParameter:
                    text = string.Format("(type parameter) {0}", ci.Name);
                    break;
                case SymbolType.Interface:
                    text = string.Format("interface {0}", ci.Name);
                    break;
                case SymbolType.Keyword:
                    text = null;
                    break;
                case SymbolType.Namespace:
                    text = string.Format("namespace {0}", ci.Name);
                    break;
                case SymbolType.Property:
                case SymbolType.Event:
                case SymbolType.Constant:
                    text = string.Format("{0} {1}", ci.Type.GetCompilerFriendlyName(), ci.Name);
                    break;
                case SymbolType.Method:
                    foreach (var mi in ci.MethodOverloads)
                        text += string.Format(
                            "{0} {1}.{2}",
                            ci.Type.GetCompilerFriendlyName(),
                            mi.DeclaringType.GetCompilerFriendlyName(),
                            mi.GetCompilerFriendlyName(true)) + Environment.NewLine;
                    break;
            }
            if (text == null)
            {
                toolTip1.Active = false;
                return;
            }

            text = text.Replace("&", "").Replace("Boolean", "bool");

            toolTip1.Active = true;
            var point = item.Position;
            point.X += this.Width + 5;
            if (UpperLeftCornerRelativeToToolTipParent != null)
            {
                point.X += UpperLeftCornerRelativeToToolTipParent.X + 9;
                point.Y += UpperLeftCornerRelativeToToolTipParent.Y + 9;
            }

            toolTip1.Show(text, WindowToShowToolTipsOn, point, 15000);
        }

        public void HideToolTip()
        {
            if (WindowToShowToolTipsOn != null)
                toolTip1.Hide(WindowToShowToolTipsOn);
        }

        public IWin32Window WindowToShowToolTipsOn { get; set; }
        public Point UpperLeftCornerRelativeToToolTipParent { get; set; }

    }
}
