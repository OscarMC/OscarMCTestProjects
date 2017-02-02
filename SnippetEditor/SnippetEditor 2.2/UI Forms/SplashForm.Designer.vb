'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On




<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SplashForm
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
      Me.lblVersion = New System.Windows.Forms.Label
      Me.lblCopyright = New System.Windows.Forms.Label
      Me.SuspendLayout()
      '
      'tbnClose
      '
      Me.tbnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.tbnClose.BackColor = System.Drawing.Color.Transparent
      Me.tbnClose.BackgroundImage = Global.SnippetEditor.My.Resources.Resources.img_Close_trans
      Me.tbnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
      Me.tbnClose.FlatAppearance.BorderSize = 0
      Me.tbnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
      Me.tbnClose.Location = New System.Drawing.Point(505, 0)
      Me.tbnClose.Name = "tbnClose"
      Me.tbnClose.Size = New System.Drawing.Size(42, 22)
      Me.tbnClose.TabIndex = 0
      Me.tbnClose.UseVisualStyleBackColor = False
      '
      'lblVersion
      '
      Me.lblVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
      Me.lblVersion.AutoSize = True
      Me.lblVersion.BackColor = System.Drawing.Color.Transparent
      Me.lblVersion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblVersion.ForeColor = System.Drawing.Color.White
      Me.lblVersion.Location = New System.Drawing.Point(506, 337)
      Me.lblVersion.Name = "lblVersion"
      Me.lblVersion.Size = New System.Drawing.Size(24, 15)
      Me.lblVersion.TabIndex = 4
      Me.lblVersion.Text = "2.1"
      '
      'lblCopyright
      '
      Me.lblCopyright.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
      Me.lblCopyright.AutoSize = True
      Me.lblCopyright.BackColor = System.Drawing.Color.Transparent
      Me.lblCopyright.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.lblCopyright.ForeColor = System.Drawing.Color.White
      Me.lblCopyright.Location = New System.Drawing.Point(12, 335)
      Me.lblCopyright.Name = "lblCopyright"
      Me.lblCopyright.Size = New System.Drawing.Size(90, 15)
      Me.lblCopyright.TabIndex = 6
      Me.lblCopyright.Text = "© Bill McCarthy"
      '
      'SplashForm
      '
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
      Me.BackgroundImage = Global.SnippetEditor.My.Resources.Resources.splash
      Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
      Me.ClientSize = New System.Drawing.Size(548, 358)
      Me.ControlBox = False
      Me.Controls.Add(Me.lblCopyright)
      Me.Controls.Add(Me.lblVersion)
      Me.Controls.Add(Me.tbnClose)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "SplashForm"
      Me.Padding = New System.Windows.Forms.Padding(9)
      Me.ShowInTaskbar = False
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents tbnClose As System.Windows.Forms.Button
   Friend WithEvents lblVersion As System.Windows.Forms.Label
   Friend WithEvents lblCopyright As System.Windows.Forms.Label

End Class
