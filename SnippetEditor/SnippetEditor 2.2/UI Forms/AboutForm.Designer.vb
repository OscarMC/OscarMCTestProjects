'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On




<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
         If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
         End If
      Finally
         MyBase.Dispose(disposing)
      End Try
   End Sub


   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      Me.tbnClose = New System.Windows.Forms.Button
      Me.PictureBox1 = New System.Windows.Forms.PictureBox
      Me.lblVersion = New System.Windows.Forms.Label
      Me.wbMessage = New System.Windows.Forms.WebBrowser
      Me.lblCopyright = New System.Windows.Forms.Label
      CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'tbnClose
      '
      Me.tbnClose.BackColor = System.Drawing.Color.Transparent
      Me.tbnClose.BackgroundImage = Global.SnippetEditor.My.Resources.Resources.img_Close_trans
      Me.tbnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
      Me.tbnClose.FlatAppearance.BorderSize = 0
      Me.tbnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
      Me.tbnClose.Location = New System.Drawing.Point(565, 0)
      Me.tbnClose.Name = "tbnClose"
      Me.tbnClose.Size = New System.Drawing.Size(42, 22)
      Me.tbnClose.TabIndex = 0
      Me.tbnClose.UseVisualStyleBackColor = False
      '
      'PictureBox1
      '
      Me.PictureBox1.Image = Global.SnippetEditor.My.Resources.Resources.ani_bottle
      Me.PictureBox1.Location = New System.Drawing.Point(233, 179)
      Me.PictureBox1.Name = "PictureBox1"
      Me.PictureBox1.Size = New System.Drawing.Size(94, 83)
      Me.PictureBox1.TabIndex = 1
      Me.PictureBox1.TabStop = False
      '
      'lblVersion
      '
      Me.lblVersion.AutoSize = True
      Me.lblVersion.BackColor = System.Drawing.Color.Transparent
      Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblVersion.ForeColor = System.Drawing.Color.White
      Me.lblVersion.Location = New System.Drawing.Point(557, 417)
      Me.lblVersion.Name = "lblVersion"
      Me.lblVersion.Size = New System.Drawing.Size(24, 15)
      Me.lblVersion.TabIndex = 4
      Me.lblVersion.Text = "2.1"
      '
      'wbMessage
      '
      Me.wbMessage.AllowWebBrowserDrop = False
      Me.wbMessage.IsWebBrowserContextMenuEnabled = False
      Me.wbMessage.Location = New System.Drawing.Point(46, 93)
      Me.wbMessage.MinimumSize = New System.Drawing.Size(20, 20)
      Me.wbMessage.Name = "wbMessage"
      Me.wbMessage.ScriptErrorsSuppressed = True
      Me.wbMessage.Size = New System.Drawing.Size(514, 287)
      Me.wbMessage.TabIndex = 5
      Me.wbMessage.TabStop = False
      Me.wbMessage.Visible = False
      Me.wbMessage.WebBrowserShortcutsEnabled = False
      '
      'lblCopyright
      '
      Me.lblCopyright.AutoSize = True
      Me.lblCopyright.BackColor = System.Drawing.Color.Transparent
      Me.lblCopyright.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblCopyright.ForeColor = System.Drawing.Color.White
      Me.lblCopyright.Location = New System.Drawing.Point(12, 415)
      Me.lblCopyright.Name = "lblCopyright"
      Me.lblCopyright.Size = New System.Drawing.Size(90, 15)
      Me.lblCopyright.TabIndex = 6
      Me.lblCopyright.Text = "© Bill McCarthy"
      '
      'AboutForm
      '
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
      Me.BackgroundImage = Global.SnippetEditor.My.Resources.Resources.message_in_a_bottle
      Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
      Me.ClientSize = New System.Drawing.Size(608, 439)
      Me.ControlBox = False
      Me.Controls.Add(Me.lblCopyright)
      Me.Controls.Add(Me.wbMessage)
      Me.Controls.Add(Me.lblVersion)
      Me.Controls.Add(Me.PictureBox1)
      Me.Controls.Add(Me.tbnClose)
      Me.DoubleBuffered = True
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "AboutForm"
      Me.Padding = New System.Windows.Forms.Padding(9)
      Me.ShowInTaskbar = False
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
      CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents tbnClose As System.Windows.Forms.Button
   Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
   Friend WithEvents lblVersion As System.Windows.Forms.Label
   Friend WithEvents wbMessage As System.Windows.Forms.WebBrowser
   Friend WithEvents lblCopyright As System.Windows.Forms.Label

End Class
