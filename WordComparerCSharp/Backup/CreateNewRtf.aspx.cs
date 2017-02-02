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
using System.IO;


namespace WordApplication
{
	/// <summary>
	/// Summary description for CreateNewRtf.
	/// </summary>
	public class CreateNewRtf : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.TextBox DocName;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox Text;
		protected System.Web.UI.WebControls.Button Button2;
		protected System.Web.UI.WebControls.HyperLink HyperLink1;
		protected System.Web.UI.WebControls.HyperLink Hyperlink2;
		protected System.Web.UI.WebControls.Label StatusMessage;
		private CCWordApp test ;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			// Put user code to initialize the page here
			if (Page.IsPostBack) 
			{
				// put the link on the page
				HyperLink1.Text = DocName.Text + ".rtf";
				HyperLink1.NavigateUrl = ConfigurationSettings.AppSettings["WordDoc"] + DocName.Text + ".rtf";
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
			StreamWriter objWriter;
			//objWriter = new StreamWriter(Server.MapPath(ConfigurationSettings.AppSettings["WordDoc"] + DocName.Text + ".rtf"), False, System.Text.Encoding.ASCII);
			objWriter = new StreamWriter(ConfigurationSettings.AppSettings["WordDoc"] + DocName.Text + ".rtf",false, System.Text.Encoding.ASCII);
			String sRTF;

			try
			{
				objWriter.WriteLine("{\\rtf1");


				//'Write the color table (for use in background and foreground colors)
				//sRTF = "{\\colortbl;\\red0\\green0\\blue0;\\red0\\green0\\blue255;" +
//						"\\red0\\green255\\blue255;\\red0\\green255\\blue0;" +
//						"\\red255\\green0\\blue255;\\red255\\green0\\blue0;" +
//						"\\red255\\green255\\blue0;\\red255\\green255\\blue255;}";
//				//objWriter.WriteLine(sRTF);

				//'Write the title and author for the document properties
				objWriter.WriteLine("{\\info{\\title Sample RTF Document}" +
									"{\\author Microsoft Developer Support}}");

				//'Write the page header and footer
				objWriter.WriteLine("{\\header\\pard\\qc{\\fs50 " +
									"ASP-Generated RTF\\par}{\\fs18\\chdate\\par}\\par\\par}");
				objWriter.WriteLine("{\\footer\\pard\\qc\\brdrt\\brdrs\\brdrw10\\brsp100" +
									"\\fs18 Page " +
									"{\\field{\\*\\fldinst PAGE}{\\fldrslt 1}} of " +
									"{\\field{\\*\\fldinst NUMPAGES}{\\fldrslt 1}} \\par}");

				//'Write a sentence in the first paragraph of the document
				objWriter.WriteLine("\\par\\fs24\\cf2 This is a sample \\b RTF \\b0 " +
									"document created with ASP.\\cf0");

				objWriter.WriteLine("\\par 1\\page");
				objWriter.WriteLine("\\pard\\fs18\\cf2\\qc " +
                    "This sample provided by Microsoft Developer Support.");


				//'close the RTF string and file
				objWriter.WriteLine("}");
				objWriter.Close();
			}

			catch (Exception exc)
			{
				StatusMessage.Text = exc.Message;
				StatusMessage.Visible = true;
			}

		
		}

		
	}
}
