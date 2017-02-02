using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace uboot.ExtendedImmediateWindow.Implementation
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            tmrLeave.Start();
        }

        private void tmrLeave_Tick(object sender, EventArgs e)
        {
            tmrLeave.Stop();
            for (double opacity = .95d; opacity > 0; opacity -= .1d)
            {
                this.Opacity = opacity;
                Application.DoEvents();
                for(int i = 0; i < 20; i++)
                    System.Threading.Thread.Sleep(2);
            }
            this.Close();
        }

    }
}
