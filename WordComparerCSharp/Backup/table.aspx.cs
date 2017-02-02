using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;


namespace WordApplication
{
	/// <summary>
	/// Summary description for table.
	/// </summary>
	public class table : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox DocName;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.Button Button2;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.HyperLink Hyperlink2;
		protected System.Web.UI.WebControls.Label StatusMessage;
		protected System.Web.UI.WebControls.Table Table1;
		protected System.Web.UI.WebControls.TextBox Riga1Text1;
		protected System.Web.UI.WebControls.TextBox Riga1Text2;
		protected System.Web.UI.WebControls.TextBox Riga1Text3;
		protected System.Web.UI.WebControls.TextBox Riga1Text4;
		protected System.Web.UI.WebControls.TextBox Riga2Text1;
		protected System.Web.UI.WebControls.TextBox Riga2Text2;
		protected System.Web.UI.WebControls.TextBox Riga2Text3;
		protected System.Web.UI.WebControls.TextBox Riga2Text4;
		private CCWordApp test ;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			// Put user code to initialize the page here
			if (Page.IsPostBack) 
			{
				// put the link on the page
				HyperLink1.Text = DocName.Text + ".doc";
				HyperLink1.NavigateUrl = ConfigurationSettings.AppSettings["WordDoc"] + DocName.Text + ".doc";
				Hyperlink2.Text = DocName.Text + ".html";
				Hyperlink2.NavigateUrl = ConfigurationSettings.AppSettings["WordDoc"] +  DocName.Text + ".html";
			}

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Button2.Click += new System.EventHandler(this.Button2_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button2_Click(object sender, System.EventArgs e)
		{
			DateTime dated ;
			dated =  DateTime.Now;
			
			try
			{

				test = new CCWordApp();
				test.Open (ConfigurationSettings.AppSettings["WordMod"] + "table.dot");

				test.GoToTheTable(1);
				
				test.GoToDownCell();
				test.InsertText(Riga1Text1.Text);
				test.GoToRightCell();
				test.InsertText(Riga1Text2.Text);
				test.SaveAs (ConfigurationSettings.AppSettings["WordDoc"] + DocName.Text + ".doc");
				// Save in html format
				test.SaveAsHtml(ConfigurationSettings.AppSettings["WordDoc"] + DocName.Text + ".html");
				test.Quit();
				
			}

			catch (Exception exc)
			{
				StatusMessage.Text = exc.Message;
				StatusMessage.Visible = true;
				test.Quit();
			}
		}

	
	}
}
