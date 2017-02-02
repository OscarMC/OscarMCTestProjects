using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace WordApplication
{
	/// <summary>
	/// Summary description for CreateDocModel.
	/// </summary>
	public partial class CreateDocModel : System.Web.UI.Page
	{
		private CCWordApp test ;
	   
		protected void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if (Page.IsPostBack) 
			{
				// put the link on the page
				//HyperLink1.Text = DocName.Text + ".doc";
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

		}
		#endregion



		private void Button2_Click(object sender, System.EventArgs e)
		{
			DateTime dated ;
			dated =  DateTime.Now;
			
			try
			{

				test = new CCWordApp();
				test.Open (ConfigurationSettings.AppSettings["WordMod"] + "template1.dot");

				test.SetAlignment("Center");
				test.GotoBookMark("Title");
				test.SetFont ("Bold");
				test.InsertText(TextTitle.Text);
				test.SetFont ("nothing");

				test.GotoBookMark("name");
				test.SetFont ("Italic");
				test.InsertText(TextName.Text);
				test.SetFont ("nothing");

				test.GotoBookMark("address");
				test.SetAlignment("Right");
				test.InsertText( dated.ToShortDateString() );
				test.InsertLineBreak();
				test.InsertText(TextAddress.Text);
				test.SetFont ("nothing");

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
