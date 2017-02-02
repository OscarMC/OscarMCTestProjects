'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On





<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InputForm
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      If disposing AndAlso components IsNot Nothing Then
         components.Dispose()
      End If
      MyBase.Dispose(disposing)
   End Sub

   'Required by the Windows Form Designer
   Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InputForm))
      Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
      Me.OK_Button = New System.Windows.Forms.Button
      Me.Cancel_Button = New System.Windows.Forms.Button
      Me.txtInput = New System.Windows.Forms.TextBox
      Me.lblPrompt = New System.Windows.Forms.Label
      Me.TableLayoutPanel1.SuspendLayout()
      Me.SuspendLayout()
      '
      'TableLayoutPanel1
      '
      resources.ApplyResources(Me.TableLayoutPanel1, "TableLayoutPanel1")
      Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
      Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
      Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
      '
      'OK_Button
      '
      resources.ApplyResources(Me.OK_Button, "OK_Button")
      Me.OK_Button.Name = "OK_Button"
      '
      'Cancel_Button
      '
      resources.ApplyResources(Me.Cancel_Button, "Cancel_Button")
      Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
      Me.Cancel_Button.Name = "Cancel_Button"
      '
      'txtInput
      '
      resources.ApplyResources(Me.txtInput, "txtInput")
      Me.txtInput.Name = "txtInput"
      '
      'lblPrompt
      '
      resources.ApplyResources(Me.lblPrompt, "lblPrompt")
      Me.lblPrompt.Name = "lblPrompt"
      '
      'InputForm
      '
      Me.AcceptButton = Me.OK_Button
      resources.ApplyResources(Me, "$this")
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.CancelButton = Me.Cancel_Button
      Me.Controls.Add(Me.lblPrompt)
      Me.Controls.Add(Me.txtInput)
      Me.Controls.Add(Me.TableLayoutPanel1)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "InputForm"
      Me.ShowInTaskbar = False
      Me.TableLayoutPanel1.ResumeLayout(False)
      Me.ResumeLayout(False)
      Me.PerformLayout()

   End Sub
   Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
   Friend WithEvents OK_Button As System.Windows.Forms.Button
   Friend WithEvents Cancel_Button As System.Windows.Forms.Button
   Friend WithEvents txtInput As System.Windows.Forms.TextBox
   Friend WithEvents lblPrompt As System.Windows.Forms.Label

End Class
