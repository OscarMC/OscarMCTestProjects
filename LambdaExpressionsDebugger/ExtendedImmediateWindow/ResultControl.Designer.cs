namespace uboot.ExtendedImmediateWindow
{
    partial class ResultControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeListViewControl = new TreeListView.TreeListViewControl();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.filterToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.filterTextToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.fullScreenToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.singleResultPanel = new System.Windows.Forms.Panel();
            this.lblResultMessage = new System.Windows.Forms.Label();
            this.tmrResultSearch = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.singleResultPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeListViewControl
            // 
            this.treeListViewControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeListViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeListViewControl.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.treeListViewControl.Location = new System.Drawing.Point(0, 28);
            this.treeListViewControl.Name = "treeListViewControl";
            this.treeListViewControl.Size = new System.Drawing.Size(610, 382);
            this.treeListViewControl.TabIndex = 0;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Path";
            this.columnHeader4.Width = 20;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filterToolStripButton,
            this.toolStripLabel1,
            this.filterTextToolStripTextBox,
            this.toolStripSeparator1,
            this.fullScreenToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(610, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // filterToolStripButton
            // 
            this.filterToolStripButton.CheckOnClick = true;
            this.filterToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.filterToolStripButton.Image = global::uboot.ExtendedImmediateWindow.ControlResources.Filter2HS;
            this.filterToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.filterToolStripButton.Name = "filterToolStripButton";
            this.filterToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.filterToolStripButton.Text = "Filter by Searchtext on / off";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(33, 22);
            this.toolStripLabel1.Text = "Find:";
            // 
            // filterTextToolStripTextBox
            // 
            this.filterTextToolStripTextBox.Name = "filterTextToolStripTextBox";
            this.filterTextToolStripTextBox.Size = new System.Drawing.Size(150, 25);
            this.filterTextToolStripTextBox.TextChanged += new System.EventHandler(this.filterTextToolStripTextBox_TextChanged);
            this.filterTextToolStripTextBox.Click += new System.EventHandler(this.filterTextToolStripTextBox_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // fullScreenToolStripButton
            // 
            this.fullScreenToolStripButton.CheckOnClick = true;
            this.fullScreenToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.fullScreenToolStripButton.Image = global::uboot.ExtendedImmediateWindow.ControlResources.fullscreen;
            this.fullScreenToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.fullScreenToolStripButton.Name = "fullScreenToolStripButton";
            this.fullScreenToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.fullScreenToolStripButton.Text = "Fullscreen";
            this.fullScreenToolStripButton.CheckedChanged += new System.EventHandler(this.fullScreenToolStripButton_CheckedChanged);
            // 
            // singleResultPanel
            // 
            this.singleResultPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.singleResultPanel.Controls.Add(this.lblResultMessage);
            this.singleResultPanel.Location = new System.Drawing.Point(0, 28);
            this.singleResultPanel.Name = "singleResultPanel";
            this.singleResultPanel.Size = new System.Drawing.Size(610, 382);
            this.singleResultPanel.TabIndex = 2;
            this.singleResultPanel.Visible = false;
            // 
            // lblResultMessage
            // 
            this.lblResultMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResultMessage.Location = new System.Drawing.Point(87, 62);
            this.lblResultMessage.Name = "lblResultMessage";
            this.lblResultMessage.Size = new System.Drawing.Size(415, 255);
            this.lblResultMessage.TabIndex = 0;
            this.lblResultMessage.Text = "ResultMessage";
            this.lblResultMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tmrResultSearch
            // 
            this.tmrResultSearch.Interval = 200;
            this.tmrResultSearch.Tick += new System.EventHandler(this.tmrResultSearch_Tick);
            // 
            // ResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.treeListViewControl);
            this.Controls.Add(this.singleResultPanel);
            this.Name = "ResultControl";
            this.Size = new System.Drawing.Size(610, 410);
            this.ParentChanged += new System.EventHandler(this.ResultControl_ParentChanged);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.singleResultPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TreeListView.TreeListViewControl treeListViewControl;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton fullScreenToolStripButton;
        private System.Windows.Forms.ToolStripButton filterToolStripButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox filterTextToolStripTextBox;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel singleResultPanel;
        private System.Windows.Forms.Label lblResultMessage;
        private System.Windows.Forms.Timer tmrResultSearch;
    }
}
