'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On



<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Friend Class MessageDialogue
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
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
      Me.components = New System.ComponentModel.Container
      Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
      Me.SuspendLayout()
      '
      'MessageDialogue
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
      Me.CausesValidation = False
      Me.ClientSize = New System.Drawing.Size(219, 97)
      Me.ControlBox = False
      Me.Enabled = False
      Me.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
      Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "MessageDialogue"
      Me.ShowIcon = False
      Me.ShowInTaskbar = False
      Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
      Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
      Me.TopMost = True
      Me.ResumeLayout(False)

   End Sub
   Friend WithEvents Timer1 As System.Windows.Forms.Timer
End Class
