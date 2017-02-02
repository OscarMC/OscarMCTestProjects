'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing

<DesignerCategory("")> _
Public Class GradientLabel
   Inherits Label



   Private m_BackColor2 As Color

   <Category("Appearance")> _
   Public Property BackColor2() As Color
      Get
         Return m_BackColor2
      End Get
      Set(ByVal value As Color)
         m_BackColor2 = value
         Me.Invalidate()
      End Set
   End Property



   Private m_GradientMode As Drawing2D.LinearGradientMode

   <Category("Appearance")> _
    <DefaultValue(GetType(Drawing2D.LinearGradientMode), "0")> _
   Public Property GradientMode() As Drawing2D.LinearGradientMode
      Get
         Return m_GradientMode
      End Get
      Set(ByVal value As Drawing2D.LinearGradientMode)
         m_GradientMode = value
         Me.Invalidate()
      End Set
   End Property



   Protected Overrides Sub OnPaintBackground(ByVal e As PaintEventArgs)
      MyBase.OnPaintBackground(e)
      Dim rect As Rectangle = New Rectangle(0, 0, Me.Width, Me.Height)
      Using br As New Drawing2D.LinearGradientBrush(rect, Me.BackColor, Me.BackColor2, Me.GradientMode)
         e.Graphics.FillRectangle(br, rect)
      End Using
   End Sub



End Class
