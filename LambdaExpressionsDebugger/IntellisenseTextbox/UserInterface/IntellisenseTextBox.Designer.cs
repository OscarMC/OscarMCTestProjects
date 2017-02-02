namespace IntellisenseTextbox
{
    partial class IntellisenseTextBox
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
            this.syntaxTextBox = new IntellisenseTextbox.SyntaxTextBox();
            this.autoCompleteList = new IntellisenseTextbox.AutoCompleteListComponent(this.components);
            this.SuspendLayout();
            // 
            // syntaxTextBox
            // 
            this.syntaxTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxTextBox.Location = new System.Drawing.Point(0, 0);
            this.syntaxTextBox.Name = "syntaxTextBox";
            this.syntaxTextBox.Size = new System.Drawing.Size(527, 307);
            this.syntaxTextBox.TabIndex = 2;
            this.syntaxTextBox.Text = "";
            this.syntaxTextBox.NeedTokenAtCursorPos += new System.EventHandler<IntellisenseTextbox.SyntaxTextBox.NeedTokenAtCursorPosEventArgs>(this.syntaxTextBox_NeedTokenAtCursorPos);
            // 
            // autoCompleteList
            // 
            this.autoCompleteList.SyntaxTextBox = this.syntaxTextBox;            
            this.autoCompleteList.NeedCurrentParseResult += new System.EventHandler<IntellisenseTextbox.AutoCompleteListComponent.NeedCurrentParseResultEventArgs>(this.autoCompleteList_NeedCurrentParseResult);
            this.autoCompleteList.PopulateAutoCompleteList += new System.EventHandler<IntellisenseTextbox.AutoCompleteListComponent.PopulateAutoCompleteListEventArgs>(this.autoCompleteList_PopulateAutoCompleteList);
            this.autoCompleteList.NeedCurrentToken += new System.EventHandler<IntellisenseTextbox.AutoCompleteListComponent.NeedCurrentTokenEventArgs>(this.autoCompleteList_NeedCurrentTokenStartPos);
            // 
            // IntellisenseTextBox
            // 
            this.Controls.Add(this.syntaxTextBox);
            this.Name = "IntellisenseTextBox";
            this.Size = new System.Drawing.Size(527, 307);
            this.ResumeLayout(false);

        }

        #endregion
                
        private SyntaxTextBox syntaxTextBox;        
        private AutoCompleteListComponent autoCompleteList;

    }
}
