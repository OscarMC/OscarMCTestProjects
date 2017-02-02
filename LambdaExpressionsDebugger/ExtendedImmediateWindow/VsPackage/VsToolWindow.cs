using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE80;
using uboot.ExtendedImmediateWindow.Implementation;

namespace uboot.ExtendedImmediateWindow
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsWindowPane interface.
    /// </summary>
    [Guid("832081c5-7814-4253-80e7-c66d82f0540d")]
    public class VsToolWindow : ToolWindowPane
    {
        // This is the user control hosted by the tool window; it is exposed to the base class 
        // using the Window property. Note that, even if this class implements IDispose, we are
        // not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
        // the object returned by the Window property.
        internal ToolWindowControl Control { get; private set; }

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public VsToolWindow() :
            base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = Resources.ToolWindowTitle;
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;                      
            Control = new ToolWindowControl();

            //Den Controller für das Toolwindow gleich mit erzeugen
            //Controller initialisieren            
            var controller = new MainController(
                this, 
                ExtendedImmediateWindowPackage.DTE, 
                ExtendedImmediateWindowPackage.ThisPackage);
            controller.Connect();
        }

        /// <summary>
        /// This property returns the handle to the user control that should
        /// be hosted in the Tool Window.
        /// </summary>
        override public IWin32Window Window
        {
            get
            {
                return (IWin32Window)Control;
            }
        }        

    }
}
