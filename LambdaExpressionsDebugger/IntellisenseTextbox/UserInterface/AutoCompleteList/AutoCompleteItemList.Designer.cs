namespace IntellisenseTextbox.UserInterface
{
    partial class AutoCompleteItemList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoCompleteItemList));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "VSObject_Namespace.bmp");
            this.imageList.Images.SetKeyName(1, "VSObject_Class.bmp");
            this.imageList.Images.SetKeyName(2, "VSObject_Interface.bmp");
            this.imageList.Images.SetKeyName(3, "VSObject_Method.bmp");
            this.imageList.Images.SetKeyName(4, "VSObject_Properties.bmp");
            this.imageList.Images.SetKeyName(5, "VSObject_Field.bmp");
            this.imageList.Images.SetKeyName(6, "VSObject_Constant.bmp");
            this.imageList.Images.SetKeyName(7, "VSObject_Enum.bmp");
            this.imageList.Images.SetKeyName(8, "VSObject_EnumItem.bmp");
            this.imageList.Images.SetKeyName(9, "VSObject_Delegate.bmp");
            this.imageList.Images.SetKeyName(10, "VSObject_Event.bmp");
            this.imageList.Images.SetKeyName(11, "Keyword.bmp");
            this.imageList.Images.SetKeyName(12, "ExtensionMethod.bmp");
            this.imageList.Images.SetKeyName(13, "GenericTypeParameter.bmp");
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 255;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            // 
            // AutoCompleteItemList
            // 
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.FullRowSelect = true;
            this.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.MultiSelect = false;
            this.SmallImageList = this.imageList;
            this.View = System.Windows.Forms.View.SmallIcon;
            this.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.AutoCompleteItemList_ItemSelectionChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
