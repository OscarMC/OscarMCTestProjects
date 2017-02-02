using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using System.Threading;
using uboot.ExtendedImmediateWindow.Implementation.Extensions;

namespace uboot.ExtendedImmediateWindow
{
    public partial class ResultControl : UserControl
    {
        public ResultControl()
        {
            InitializeComponent();
            OnAutoResizeColumnHeaders();
        }

        private void OnAutoResizeColumnHeaders()
        {
            double factor = (double)(this.Width-20) / (double)treeListViewControl.TotalColWidth;            
            foreach (var col in treeListViewControl.Columns.Cast<ColumnHeader>())
            {                
                col.Width = (int)((double)col.Width * factor);
            }
        }

        private Control OriginalParent { get; set; }

        private void fullScreenToolStripButton_CheckedChanged(object sender, EventArgs e)
        {
            if (fullScreenToolStripButton.Checked)
            {
                Form hostForm = new Form();
                hostForm.Text = "Extended Immediate Results";
                hostForm.Size = new Size(800, 600);
                hostForm.Icon = Icon.FromHandle(global::uboot.ExtendedImmediateWindow.ControlResources.intruding_methods_icon_16x16_version_3.GetHicon());
                hostForm.Controls.Add(this);
                hostForm.FormClosed +=
                    (s, eventArgs) =>
                    {
                        OriginalParent.Controls.Add(this);
                        fullScreenToolStripButton.Checked = false;
                    };
                hostForm.Show();
            }
            else
            {                
                if (this.Parent is Form)
                    (this.Parent as Form).Close();
            }

        }

        private void ResultControl_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Location = new Point(0, 0);
                this.Size = this.Parent.ClientSize;
                this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            if (OriginalParent == null)
                OriginalParent = this.Parent;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            toolStrip1.Width = this.Width;
            OnAutoResizeColumnHeaders();
        }


        internal void TellSingleMessage(string message)
        {
            singleResultPanel.BringToFront();
            singleResultPanel.Visible = true;
            lblResultMessage.Text = message;            
        }

        internal void DisplayExpression(Expression expression)
        {
            singleResultPanel.SendToBack();
            singleResultPanel.Visible = false;
            treeListViewControl.TreeView.Nodes.Clear();            

            string mainNodeText = string.Empty;
            if (expression.Type.Contains("Exception"))
                mainNodeText = treeListViewControl.CreateMultiColumnNodeText("Exception ocurred.", "", "", "");
            else
            {
                string value = expression.Value;
                if (value.EndsWith(" null"))
                    value = "null";
                mainNodeText = treeListViewControl.CreateMultiColumnNodeText("result", value, expression.Type, "");
            }

            TreeNode tn = new TreeNode(mainNodeText);
            tn.Tag = expression;
            treeListViewControl.TreeView.Nodes.Add(tn);
            treeListViewControl.TreeView.Tag = tn;

            StopBuildingResultTree = false;

            treeListViewControl.Enabled = true;

            Action addRecursiveAction = () => AddRecursive(tn);
            System.Threading.Thread thread = new System.Threading.Thread(new ThreadStart(addRecursiveAction));
            thread.Start();

            HasLoadedResults = true;
        }

        private bool HasLoadedResults { get; set; }

        private bool StopBuildingResultTree {get;set;}

        private void AddRecursive(TreeNode startTreeNode)
        {
            DateTime startTime = DateTime.Now;

            List<TreeNode> nodes = new List<TreeNode>() { startTreeNode };

            while (DateTime.Now - startTime < new TimeSpan(0, 0, MaxLoadResultSeconds) && nodes.Count > 0)
            {
                List<TreeNode> newList = new List<TreeNode>();
                foreach (TreeNode treeNode in nodes)
                {
                    newList.AddRange(AddNodes(treeNode, startTime));
                }
                nodes = newList;
            }
        }

        private List<TreeNode> AddNodes(TreeNode tn, DateTime startTime)
        {
            if (StopBuildingResultTree) return new List<TreeNode>();
            if (DateTime.Now - startTime > new TimeSpan(0, 0, MaxLoadResultSeconds)) return new List<TreeNode>();
            List<TreeNode> newSubNodes = new List<TreeNode>();
            Expression exp = tn.Tag as Expression;
            foreach (Expression e in exp.DataMembers.Cast<Expression>())
            {
                if (StopBuildingResultTree) break;

                string text = treeListViewControl.CreateMultiColumnNodeText(e.Name, e.Value, e.Type, "");
                TreeNode subTn = new TreeNode(text);
                subTn.Tag = e;

                Action addAction = () => tn.Nodes.Add(subTn);
                treeListViewControl.TreeView.Invoke(addAction);
                newSubNodes.Add(subTn);
            }
            return newSubNodes;
        }


        private const int MaxLoadResultSeconds = 60;

        private void filterTextToolStripTextBox_Click(object sender, EventArgs e)
        {       
        }

        private void filterTextToolStripTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!HasLoadedResults) return;
            StopBuildingResultTree = true;
            resultSearchRunNr++;
            tmrResultSearch.Start();
        }

        private int resultSearchRunNr = 0;

        private void tmrResultSearch_Tick(object sender, EventArgs e)
        {
            int runNr = resultSearchRunNr;
            tmrResultSearch.Stop();
            string searchText = filterTextToolStripTextBox.Text;
            TreeView treeView = treeListViewControl.TreeView;
            TreeNode baseTreeNode = treeView.Tag as TreeNode;
            if (searchText == "")
            {
                treeView.Nodes.Clear();
                treeView.Nodes.Add(baseTreeNode);
                treeView.ShowPlusMinus = true;
            }
            else
            {
                treeView.Nodes.Clear();
                var allNodesCompleteList = baseTreeNode.GetRecursive(new List<TreeNode>(), (tn, list) => tn.Nodes.Cast<TreeNode>());
                var allNodes = allNodesCompleteList.Where(t => t.Text.ToLower().Contains(searchText.ToLower()));
                treeView.ShowPlusMinus = false;
                foreach (var tn in allNodes)
                {
                    if (resultSearchRunNr != runNr) break;
                    string origin = (tn.Tag as Expression).Name;
                    var tmpTn = tn;
                    while (tmpTn.Parent != null)
                    {
                        if (tmpTn.Parent.Parent == null)
                        {
                            origin = "result." + origin;
                            break;
                        }
                        string name = (tmpTn.Parent.Tag as Expression).Name;
                        if (name.Contains("{"))
                            name = name.Substring(name.IndexOf("{") - 1).Trim();
                        origin = name + "." + origin;
                        tmpTn = tmpTn.Parent;
                        if (resultSearchRunNr != runNr) break;
                    }
                    if (resultSearchRunNr != runNr) break;                    
                    treeView.Nodes.Add(tn.Text + origin);
                    Application.DoEvents();
                };
            }
        }
    }
}
