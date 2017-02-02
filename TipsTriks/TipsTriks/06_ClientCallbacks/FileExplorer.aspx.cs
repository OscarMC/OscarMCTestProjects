using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class FileExplorer : System.Web.UI.Page
{
    protected void PopulateNode(Object source, TreeNodeEventArgs e)
    {
        TreeNode node = e.Node;

        if (e.Node.Value == "Demos")
        {
            e.Node.Value = Server.MapPath("~/");
        }

        String[] dirs = Directory.GetDirectories(node.Value);

        // Enumerate directories
        foreach (String dir in dirs)
        {
            TreeNode newNode = new TreeNode(Path.GetFileName(dir), dir);

            if (Directory.GetFiles(dir).Length > 0 || Directory.GetDirectories(dir).Length > 0)
            {
                newNode.PopulateOnDemand = true;
            }

            node.ChildNodes.Add(newNode);
        }

        // Enumerate files
        String[] files = Directory.GetFiles(node.Value);

        foreach (String file in files)
        {
            TreeNode newNode = new TreeNode(Path.GetFileName(file), file);
            node.ChildNodes.Add(newNode);
        }
    }
}
