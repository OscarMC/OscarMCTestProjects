namespace IntellisenseTextbox
{
    partial class AutoCompleteListComponent
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
            this.keyHandler = new IntellisenseTextbox.SyntaxTextBoxKeyHandler(this.components);
            // 
            // keyHandler
            // 
            this.keyHandler.SyntaxTextBox = null;
            this.keyHandler.MakeAutoCompleteListTransparent += new System.EventHandler(this.keyHandler_MakeAutoCompleteListTransparent);
            this.keyHandler.AcceptAutoCompleteAdvice += new System.EventHandler(this.keyHandler_AcceptAutoCompleteAdvice);
            this.keyHandler.CloseAutoCompleteList += new System.EventHandler(this.keyHandler_CloseAutoCompleteList);
            this.keyHandler.MakeAutoCompleteListOpaque += new System.EventHandler(this.keyHandler_MakeAutoCompleteListOpaque);
            this.keyHandler.ForceAutoCompleteList += new System.EventHandler(this.keyHandler_ForceAutoCompleteList);
            this.keyHandler.ChangeAutoCompleteItemIndex += new System.EventHandler<IntellisenseTextbox.SyntaxTextBoxKeyHandler.ChangeAutoCompleteItemIndexEventArgs>(this.keyHandler_ChangeAutoCompleteItemIndex);
            this.keyHandler.CheckAutoCompleteListIsActive += new System.EventHandler<IntellisenseTextbox.SyntaxTextBoxKeyHandler.CheckAutoCompleteListIsActiveEventArgs>(this.keyHandler_CheckAutoCompleteListIsActive);

        }

        #endregion

        private SyntaxTextBoxKeyHandler keyHandler;
    }
}
