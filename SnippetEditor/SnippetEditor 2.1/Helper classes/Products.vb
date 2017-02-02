'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports Microsoft.Win32
Imports System.Text
Imports VB = Microsoft.VisualBasic
Imports System.Collections.Generic


Namespace Utility

   ' used by My.Settings.Products.
   '****************************************
   '<ArrayOfProduct xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
   '    xmlns:xsd="http://www.w3.org/2001/XMLSchema">
   '   <Product>
   '      <Name>Visual Studio 2005</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VisualStudio\8.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual Basic 2005 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VBExpress\8.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual C# 2005 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VCSExpress\8.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual Web Developer 2005 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VWDExpress\8.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual Studio 2008</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VisualStudio\9.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual Basic 2008 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VBExpress\9.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual C# 2008 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VCSExpress\9.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual Web Developer 2008 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VWDExpress\9.0</RegistryPath>
   '   </Product >
   '   <Product>
   '      <Name>Visual Studio 2010</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VisualStudio\10.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual Basic 2010 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VBExpress\10.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual C# 2010 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VCSExpress\10.0</RegistryPath>
   '   </Product>
   '   <Product>
   '      <Name>Visual Web Developer 2010 Express Edition</Name>
   '      <RegistryPath>SOFTWARE\Microsoft\VWDExpress\10.0</RegistryPath>
   '   </Product >
   '</ArrayOfProduct>
   '*********************************


   Public Class ProductCollection
      Inherits List(Of Product)

      Public Overloads Function Find(ByVal name As String) As Product
         For Each prod As Product In Me
            If prod.Name = name Then Return prod
         Next
         Return Nothing
      End Function

   End Class




   Public Class Product

      Friend CodeExpansionsPath As String = "Languages\CodeExpansions"

      Private _Name As String
      Private _RegistryPath As String
      Private _Languages As Language.LanguageCollection
      Private _LCID As Int32

      Public Property Name() As String
         Get
            Return _Name
         End Get
         Set(ByVal value As String)
            _Name = value
         End Set
      End Property



      Public Property RegistryPath() As String
         Get
            Return _RegistryPath
         End Get
         Set(ByVal value As String)
            _RegistryPath = value
         End Set
      End Property

      Public ReadOnly Property HKLMRegistryPath() As String
         Get
            If IntPtr.Size = 8 Then
               ' 64 bit product change path to Wow6432Node
               ' Thanks to Ken Tucker for providng this 64 bit fix.
               Return RegistryPath.Replace("SOFTWARE\", "SOFTWARE\Wow6432Node\")
            Else
               Return RegistryPath
            End If
         End Get
      End Property



      <System.Xml.Serialization.XmlIgnore()> _
      Public ReadOnly Property Languages() As Language.LanguageCollection
         Get
            If _Languages Is Nothing Then
               _Languages = New Language.LanguageCollection(Me)

            End If
            Return _Languages
         End Get
      End Property


      Public ReadOnly Property IsInstalled() As Boolean
         Get
            Dim path As String = Me.RegistryPath
            If path = Nothing Then Return False
            Using hkey As RegistryKey = Registry.CurrentUser.OpenSubKey(Me.RegistryPath & "\General")
               Return (hkey IsNot Nothing)
            End Using
         End Get
      End Property


      Public ReadOnly Property LCID() As Int32
         Get
            If _LCID = 0 Then
               Using hkey As RegistryKey = Registry.CurrentUser.OpenSubKey(Me.RegistryPath & "\General")
                  _LCID = CInt(hkey.GetValue("UILanguage", -1))
               End Using
               If _LCID < 0 Then   'it's the system lanugage
                  _LCID = Globalization.CultureInfo.CurrentUICulture.LCID
               End If
            End If
            Return _LCID
         End Get
      End Property



      'Friend Sub InitializeUserRegistry()
      '   ' loop through the keys in HKLM\Software\Microsoft\VisualStudio\9.0\Languages\CodeExpansions
      '   ' then find the package and find the package satelliteDll, then get the resource string
      '   ' using that string then create the sub key in HKCU\\Software\Microsoft\VisualStudio\9.0\Languages\CodeExpansions
      '   ' then set the path and copy the Paths sub nodes over (optional)

      '   If Not IsInstalled Then Return

      '   Dim snippetLangID As String
      '   Dim langName As String


      '   Using rootKey As RegistryKey = Registry.LocalMachine.OpenSubKey(Me.HKLMRegistryPath, False), _
      '       languageKey As RegistryKey = rootKey.OpenSubKey(Me.CodeExpansionsPath, False), _
      '       hkcuKey As RegistryKey = Registry.CurrentUser.CreateSubKey(Me.RegistryPath & "\" & Me.CodeExpansionsPath)

      '      If rootKey Is Nothing Or languageKey Is Nothing Or hkcuKey Is Nothing Then
      '         Return
      '      End If

      '      For Each subkeyName As String In languageKey.GetSubKeyNames

      '         Dim subkey As RegistryKey = languageKey.OpenSubKey(subkeyName, False)

      '         If subkey Is Nothing Then Continue For

      '         Dim snippetLangID_Object As Object = subkey.GetValue("")

      '         If snippetLangID_Object Is Nothing Then Continue For

      '         snippetLangID = snippetLangID_Object.ToString

      '         langName = subkey.GetValue("DisplayName").ToString

      '         ' XML language has resid as #200 so need ot strip of leading "#"
      '         If langName.StartsWith("#") Then langName = langName.Substring(1)

      '         ' if the langName is numeric then need to get the langName from the package resource string table
      '         If VB.IsNumeric(langName) Then
      '            Dim resID As UInt32 = CUInt(langName)
      '            Dim package As String = subkey.GetValue("Package").ToString
      '            Using packageKey As RegistryKey = rootKey.OpenSubKey("Packages\" & package & "\SatelliteDll", False)
      '               Dim path As String = packageKey.GetValue("Path", "").ToString
      '               path = IO.Path.Combine(path, Me.LCID.ToString)
      '               path = IO.Path.Combine(path, packageKey.GetValue("DllName", "").ToString)
      '               langName = NativeMethods.GetResourceString(path, resID)
      '               If langName = Nothing Then langName = subkeyName
      '            End Using
      '         End If


      '         'concat path

      '         Dim sb As New Text.StringBuilder
      '         Using regKey As RegistryKey = languageKey.OpenSubKey(subkeyName & "\Paths", False)

      '            For Each subName As String In regKey.GetValueNames
      '               Dim temp As String = regKey.GetValue(subName, "").ToString.Trim
      '               If temp.Length > 0 Then
      '                  sb.Append(temp)
      '                  If Not temp.EndsWith(";") Then sb.Append(";")
      '               End If
      '            Next
      '         End Using


      '         ' now create the HKCU keys.

      '         Using userKey As RegistryKey = hkcuKey.CreateSubKey(langName)
      '            userKey.SetValue("", snippetLangID, RegistryValueKind.String)
      '            userKey.SetValue("Path", sb.ToString, RegistryValueKind.String)
      '         End Using

      '      Next

      '   End Using
      'End Sub




   End Class



   Public Class Language


      Public Class LanguageCollection
         Inherits System.Collections.ObjectModel.Collection(Of Language)

         Private _product As Product

         Public Sub New(ByVal parent As Product, Optional ByVal loadFromRegistry As Boolean = True)
            _product = parent
            If loadFromRegistry Then LoadLanguagesFromRegistry()
         End Sub

         Protected Overrides Sub InsertItem(ByVal index As Integer, ByVal item As Language)
            item._Product = _product
            MyBase.InsertItem(index, item)
         End Sub

         Private Sub LoadLanguagesFromRegistry()
            Using languageKey As RegistryKey = Registry.LocalMachine.OpenSubKey(_product.HKLMRegistryPath & "\" & _product.CodeExpansionsPath & "\", False)
               If languageKey IsNot Nothing Then
                  For Each key In languageKey.GetSubKeyNames
                     If key <> Nothing Then Me.Add(New Language(key))
                  Next
               End If
            End Using
         End Sub

      End Class


      Public Shared ReadOnly UnCatalogued As New Language("UnCatalogued")

      Private _Name As String
      Private _LocalisedName As String
      Private _Product As Product


      Public Sub New()
         '
      End Sub


      Public Sub New(ByVal name As String)
         Me._Name = name
         If name = "UnCatalogued" Then _LocalisedName = "UnCatalogued"
      End Sub


      Public Property Name() As String
         Get
            Return Me._Name
         End Get
         Set(ByVal value As String)
            Me._Name = value
         End Set
      End Property


      <System.Xml.Serialization.XmlIgnore()> _
      Public ReadOnly Property LocalisedName() As String
         Get
            If Me._LocalisedName = Nothing Then
               If Me.Product.IsInstalled Then
                  Me._LocalisedName = GetLocalisedName()
               End If
            End If
            If Me._LocalisedName = Nothing Then
               Me._LocalisedName = Me.Name
            End If
            Return Me._LocalisedName
         End Get
      End Property


      Public ReadOnly Property IsInstalled() As Boolean
         Get
            Using languageKey As RegistryKey = Registry.LocalMachine.OpenSubKey(Me.Product.HKLMRegistryPath & "\" & Me.Product.CodeExpansionsPath & "\" & Me.Name, False)
               Return CStr(languageKey.GetValue("DisplayName", "")) <> Nothing
            End Using
         End Get
      End Property


      Public ReadOnly Property Product() As Product
         Get
            Return Me._Product
         End Get
      End Property


      Public Function GetExpandedPaths() As String()
         Dim paths As String = Me.SnippetsPath
         paths = ReplaceVars(paths)
         Return paths.Split(";"c)
      End Function


      <System.Xml.Serialization.XmlIgnore()> _
      Public Property SnippetsPath() As String
         Get
            Using regKey As RegistryKey = GetRegLanguagePathKey()
               If regKey Is Nothing Then Return ""
               Return regKey.GetValue("Path", "").ToString
            End Using
         End Get
         Set(ByVal value As String)
            Using regKey As RegistryKey = GetRegLanguagePathKey()
               If regKey Is Nothing Then Return
               regKey.SetValue("Path", value)
            End Using
         End Set
      End Property


#Region "private helper methods"

      Private Function ReplaceVars(ByVal strIn As String) As String
         Dim strOut As String
         If strIn = Nothing Then Return ""
         strOut = strIn.Replace("%MyDocs%", GetMyDocs)
         strOut = strOut.Replace("%InstallRoot%\", GetInstallRoot)
         strOut = strOut.Replace("%LCID%", Me.Product.LCID.ToString)
         Return strOut
      End Function



      Private Function GetInstallRoot() As String
         Static m_InstallRoot As String
         If m_InstallRoot = Nothing Then
            Using hkey As RegistryKey = Registry.LocalMachine.OpenSubKey(Me.Product.HKLMRegistryPath & "\Setup\VS")
               m_InstallRoot = hkey.GetValue("ProductDir", IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.ProgramFiles, "Microsoft Visual Studio 9")).ToString
            End Using
         End If
         If Not m_InstallRoot.EndsWith("\") Then m_InstallRoot &= "\"
         Return m_InstallRoot
      End Function


      Private Function GetMyDocs() As String
         If Me.Product Is Nothing Then Return My.Computer.FileSystem.SpecialDirectories.MyDocuments
         Dim docsPath As String
         Using hkey As RegistryKey = Registry.CurrentUser.OpenSubKey(Me.Product.RegistryPath)
            docsPath = hkey.GetValue("VisualStudioLocation", "").ToString
         End Using
         If docsPath = Nothing Then docsPath = My.Computer.FileSystem.SpecialDirectories.MyDocuments
         Return docsPath
      End Function



      Private Function GetLocalisedName() As String

         Using rootKey As RegistryKey = Registry.LocalMachine.OpenSubKey(Me.Product.HKLMRegistryPath & "\" & Product.CodeExpansionsPath & "\" & Me.Name, False)

            If rootKey Is Nothing Then Return Me.Name

            Dim resIDString As String = rootKey.GetValue("DisplayName", "").ToString

            ' special handling for XML language as it has resid as #200 so need to strip of leading "#"
            If resIDString.StartsWith("#") Then resIDString = resIDString.Substring(1)

            ' if the resID is numeric then need to get the LocalisedName from the package resource string table
            If VB.IsNumeric(resIDString) Then
               Dim id As UInt32 = CUInt(resIDString)
               Dim package As String = rootKey.GetValue("Package").ToString
               Using packageKey As RegistryKey = Registry.LocalMachine.OpenSubKey(Me.Product.HKLMRegistryPath & "\Packages\" & package & "\SatelliteDll", False)
                  Dim path As String = packageKey.GetValue("Path").ToString
                  If path = Nothing Then Return resIDString
                  path = IO.Path.Combine(path, Me.Product.LCID.ToString)
                  path = IO.Path.Combine(path, packageKey.GetValue("DllName").ToString)
                  resIDString = NativeMethods.GetResourceString(path, id)
               End Using
            End If

            Return resIDString
         End Using
      End Function



      Private Function GetRegLanguagePathKey() As RegistryKey
         If Me.Product Is Nothing Then Return Nothing

         Dim CodeExpansionsRegKeyPath As String = Me.Product.RegistryPath & "\" & Me.Product.CodeExpansionsPath

         Dim regKey As RegistryKey = Registry.CurrentUser.OpenSubKey(CodeExpansionsRegKeyPath & "\" & Me.LocalisedName, True)

         If regKey Is Nothing OrElse regKey.ValueCount = 0 Then
            If Me.IsInstalled Then Me.InitializeUserRegistry()
         End If

         If regKey Is Nothing Then
            regKey = Registry.CurrentUser.OpenSubKey(CodeExpansionsRegKeyPath & "\" & Me.LocalisedName, True)
         End If

         Return regKey

      End Function


      Friend Sub InitializeUserRegistry()
         ' find the package and find the package satelliteDll, then get the resource string
         ' using that string then create the sub key in HKCU\\Software\Microsoft\VisualStudio\9.0\Languages\CodeExpansions
         ' then set the path and copy the Paths sub nodes over (optional)

         If (Not IsInstalled) Or (Me.Product Is Nothing) Then Return

         Dim snippetLangID As String
         Dim langName As String


         Using rootKey As RegistryKey = Registry.LocalMachine.OpenSubKey(Me.Product.HKLMRegistryPath, False), _
             languageKey As RegistryKey = rootKey.OpenSubKey(Me.Product.CodeExpansionsPath, False), _
             hkcuKey As RegistryKey = Registry.CurrentUser.CreateSubKey(Me.Product.RegistryPath & "\" & Me.Product.CodeExpansionsPath)

            If rootKey Is Nothing Or languageKey Is Nothing Or hkcuKey Is Nothing Then
               Return
            End If



            Dim subkey As RegistryKey = languageKey.OpenSubKey(Me.Name, False)

            If subkey Is Nothing Then Return

            Dim snippetLangID_Object As Object = subkey.GetValue("")

            If snippetLangID_Object Is Nothing Then Return

            snippetLangID = snippetLangID_Object.ToString

            langName = subkey.GetValue("DisplayName").ToString

            ' XML language has resid as #200 so need ot strip of leading "#"
            If langName.StartsWith("#") Then langName = langName.Substring(1)

            ' if the langName is numeric then need to get the langName from the package resource string table
            If VB.IsNumeric(langName) Then
               Dim resID As UInt32 = CUInt(langName)
               Dim package As String = subkey.GetValue("Package").ToString
               Using packageKey As RegistryKey = rootKey.OpenSubKey("Packages\" & package & "\SatelliteDll", False)
                  Dim path As String = packageKey.GetValue("Path", "").ToString
                  path = IO.Path.Combine(path, Me.Product.LCID.ToString)
                  path = IO.Path.Combine(path, packageKey.GetValue("DllName", "").ToString)
                  langName = NativeMethods.GetResourceString(path, resID)
                  If langName = Nothing Then langName = Me.Name
               End Using
            End If


            'concat path

            Dim sb As New Text.StringBuilder
            Using regKey As RegistryKey = languageKey.OpenSubKey(Me.Name & "\Paths", False)

               For Each subName As String In regKey.GetValueNames
                  Dim temp As String = regKey.GetValue(subName, "").ToString.Trim
                  If temp.Length > 0 Then
                     sb.Append(temp)
                     If Not temp.EndsWith(";") Then sb.Append(";")
                  End If
               Next
            End Using


            ' now create the HKCU keys.

            Using userKey As RegistryKey = hkcuKey.CreateSubKey(langName)
               userKey.SetValue("", snippetLangID, RegistryValueKind.String)
               userKey.SetValue("Path", sb.ToString, RegistryValueKind.String)
            End Using


         End Using
      End Sub




#End Region


   End Class




   Partial Friend Class NativeMethods
      Private Sub New()

      End Sub
      Private Declare Auto Function LoadString Lib "user32.dll" (ByVal hInstance As IntPtr, ByVal uID As UInt32, ByVal lpBuffer As StringBuilder, ByVal nBufferMax As Int32) As Int32
      Private Declare Auto Function LoadLibrary Lib "kernel32.dll" (ByVal lpFileName As String) As IntPtr
      Private Declare Auto Function FreeLibrary Lib "kernel32.dll" (ByVal hModule As IntPtr) As <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.Bool)> Boolean

      Friend Shared Function GetResourceString(ByVal path As String, ByVal resourceID As UInt32) As String
         Dim hModule As IntPtr
         Dim sb As New StringBuilder(256)
         Dim retVal As String
         Try
            hModule = LoadLibrary(path)
            LoadString(hModule, resourceID, sb, 255)
            retVal = sb.ToString
         Finally
            FreeLibrary(hModule)
         End Try
         Return retVal
      End Function

   End Class




End Namespace
