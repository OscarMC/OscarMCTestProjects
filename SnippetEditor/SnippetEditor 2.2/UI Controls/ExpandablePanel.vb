'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Drawing


<DesignerCategory("")> _
Public Class ExpandablePanel
   Inherits Panel
   Implements INotifyPropertyChanged


   Public Sub New()
      MyBase.new()
      _title = New GradientLabel
      InitializeTitle()
   End Sub


   Private WithEvents _title As GradientLabel
   Private _collapsed As Boolean = False


   <Category("Appearance")> _
   Public Property TitleBackColor1() As Color
      Get
         Return _title.BackColor
      End Get
      Set(ByVal value As Color)
         If value <> _title.BackColor Then
            _title.BackColor = value
            _title.Invalidate()
         End If
      End Set
   End Property


   <Category("Appearance")> _
   Public Property TitleBackColor2() As Color
      Get
         Return _title.BackColor2
      End Get
      Set(ByVal value As Color)
         If value <> _title.BackColor2 Then
            _title.BackColor2 = value
            _title.Invalidate()
         End If
      End Set
   End Property


   <Category("Appearance")> _
 Public Property TitleForeColor() As Color
      Get
         Return _title.ForeColor
      End Get
      Set(ByVal value As Color)
         If value <> _title.ForeColor Then
            _title.ForeColor = value
            _title.Invalidate()
         End If
      End Set
   End Property


   <Category("Appearance")> _
  Public Property TitleHeight() As Int32
      Get
         Return _title.Height
      End Get
      Set(ByVal value As Int32)
         If value <> _title.Height Then
            _title.Height = value
            _title.Invalidate()
         End If
      End Set
   End Property

   <Category("Appearance")> _
 Public Property TitleFont() As Font
      Get
         Return _title.Font
      End Get
      Set(ByVal value As Font)
         _title.Font = value
         _title.Invalidate()
      End Set
   End Property


   <Category("Appearance")> _
   Public Property GradientMode() As Drawing2D.LinearGradientMode
      Get
         Return _title.GradientMode
      End Get
      Set(ByVal value As Drawing2D.LinearGradientMode)
         If value <> _title.GradientMode Then
            _title.GradientMode = value
            _title.Invalidate()
         End If
      End Set
   End Property

   <Category("Appearance")> _
 Public Property Collapsed() As Boolean
      Get
         Return _collapsed
      End Get
      Set(ByVal value As Boolean)
         If value <> _collapsed Then
            ToggleCollapsed()
            OnPropertyChanged("Collapsed")
         End If
      End Set
   End Property


   Overrides Property Text() As String
      Get
         Return _title.Text
      End Get
      Set(ByVal value As String)
         If value <> _title.Text Then
            _title.Text = value
            _title.Invalidate()
            OnPropertyChanged("Text")
         End If
      End Set
   End Property


   Public Overrides Property Font() As System.Drawing.Font
      Get
         Return MyBase.Font
      End Get
      Set(ByVal value As System.Drawing.Font)
         If value IsNot MyBase.Font Then
            MyBase.Font = value
            _title.Font = value
            _title.Invalidate()
         End If
      End Set
   End Property



   Private Sub InitializeTitle()
      With Me._title
         .AutoEllipsis = True
         .AutoSize = False
         .Dock = System.Windows.Forms.DockStyle.Top
         .Image = My.Resources.img_Collapse_large
         .ImageAlign = System.Drawing.ContentAlignment.MiddleRight
         .TextAlign = System.Drawing.ContentAlignment.MiddleLeft
         .Visible = True
      End With
      Me.Controls.Add(_title)
   End Sub




   Private Sub title_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _title.Click
      Collapsed = Not Collapsed
   End Sub


   Private Sub ToggleCollapsed()
      Static _topPadding As Int32 = 2
      If _collapsed Then
         Me.AutoSize = True
         Me.Padding = New Padding(Me.Padding.Left, _topPadding, Me.Padding.Right, Me.Padding.Bottom)
         _collapsed = False
         _title.Image = My.Resources.img_Collapse_large
      Else
         _topPadding = Me.Padding.Top
         Me.Padding = New Padding(Me.Padding.Left, 2, Me.Padding.Right, Me.Padding.Bottom)
         Me.AutoSize = False
         Me.Height = _title.Height + 2
         _collapsed = True
         _title.Image = My.Resources.img_Expand_large
      End If
   End Sub

   Protected Overrides Sub OnControlAdded(ByVal e As System.Windows.Forms.ControlEventArgs)
      MyBase.OnControlAdded(e)
      _title.SendToBack()
   End Sub

   Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

   Protected Sub OnPropertyChanged(ByVal name As String)
      RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
   End Sub
End Class
