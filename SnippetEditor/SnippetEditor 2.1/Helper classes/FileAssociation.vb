'-----------------------------------------------------------------
' Copyright (c) Bill McCarthy.  
' This code and information are provided "as is"  without warranty
' of any kind either expressed or implied. Use at your own risk.
'-----------------------------------------------------------------

Option Strict On : Option Explicit On : Option Compare Binary : Option Infer On

Imports System
Imports Microsoft.Win32





''' <summary>
''' class to enable or remove file associations
''' </summary>
''' <remarks>
'''  See GetWritableClassesRoot remarks for details about dealing with UAC and x86 virtualization on x86)
'''  </remarks>
Friend NotInheritable Class FileAssociation

   Private Const _AssociationName As String = "VB Snippet Editor"
   Private Const _previous As String = "previous"
   Private Const _Description As String = "Visual Studio Snippet"


   Private Sub New()
      ' shared class
   End Sub


   Public Shared Function IsAssociated(ByVal extension As String) As Boolean

      Using regkey As RegistryKey = Registry.ClassesRoot.OpenSubKey(extension)
         If (regkey IsNot Nothing) AndAlso (regkey.GetValue("", "").ToString() = _AssociationName) Then
            Return True
         Else
            Return False
         End If
      End Using

   End Function




   Public Shared Function Associate(ByVal extension As String, ByVal remove As Boolean) As Boolean
      If extension = Nothing Then Return False

      Dim regkey As RegistryKey = Nothing
      Dim oldValue As String
      Try

         If remove Then

            If IsAssociated(extension) Then
               regkey = GetWritableClassesRoot.OpenSubKey(extension, True)
               oldValue = regkey.GetValue(_previous, "").ToString
               regkey.SetValue("", oldValue, RegistryValueKind.String)
               regkey.DeleteValue(_previous, False)
            End If

         Else 'adding the key

            regkey = GetWritableClassesRoot.CreateSubKey(extension) 'gives us write permissions
            oldValue = regkey.GetValue("", "").ToString
            If oldValue <> _AssociationName Then
               regkey.SetValue(_previous, oldValue, RegistryValueKind.String)
               regkey.SetValue("", _AssociationName, RegistryValueKind.String)
            End If
            EnsureAppKey()
         End If

      Catch ex As Exception
         If Threading.Interlocked.Exchange(_UserMode, 2) = 1 Then
            ' try calling the method again using HKCU.  See GetWritableClassesRoot remarks for details.
            Associate(extension, remove)
         Else
            Throw
         End If
      Finally
         If regkey IsNot Nothing Then regkey.Close()
      End Try

   End Function



   Private Shared Sub EnsureAppKey()

      Dim appPath As String = System.Diagnostics.Process.GetCurrentProcess.MainModule.FileName
      If appPath.EndsWith(".vshost.exe") Then appPath = appPath.Substring(0, appPath.Length - 11) & ".exe"

      ' key doesn't exist so let's create it
      Using regkey As RegistryKey = GetWritableClassesRoot.CreateSubKey(_AssociationName)

         regkey.SetValue("", _Description)
         RegSetSubKeyValue(regkey, "DefaultIcon", "", """" & appPath & """,0", RegistryValueKind.String)
         RegSetSubKeyValue(regkey, "Shell", "", "open", RegistryValueKind.String)
         RegSetSubKeyValue(regkey, "Shell\open", "", "&Open", RegistryValueKind.String)
         RegSetSubKeyValue(regkey, "Shell\open\command", "", """" & appPath & """ ""%1""", RegistryValueKind.String)

      End Using
   End Sub


   ' helper function to set sub-key values
   Private Shared Sub RegSetSubKeyValue(ByVal root As RegistryKey, ByVal subkey As String, ByVal name As String, ByVal value As Object, ByVal kind As RegistryValueKind)

      Using regkey As RegistryKey = root.CreateSubKey(subkey)
         regkey.SetValue(name, value, kind)
      End Using

   End Sub


   Private Shared _UserMode As Int32 ' flag to determine to use HKLM (1) or HCKU (2). See GetWritableClassesRoot remarks for details.

   ''' <summary>
   ''' Gets the writable part of ClassesRoot.
   ''' </summary>
   ''' <returns>registryKey. Ensure you close it.</returns>
   ''' <remarks>
   ''' If the user can't write to HKLM, then HKCU is used. 
   ''' The result of the test is stored in the _UserMode field, HKLM (1) or HCKU (2)
   ''' An oddity exists when running with UAC on and as x86 on x64 (WOW64), where the HKLM open with write permissions but fails when attempted to be used.
   ''' To deal with this the _UserMode flag is checked in write methods, and if it is 1 (HKLM), it is changed to 2 (HKCU) and the method is retried.
   ''' </remarks>
   Private Shared Function GetWritableClassesRoot() As RegistryKey
      Dim regkey As RegistryKey = Nothing
      Select Case _UserMode
         Case 0 ' need to check if HKLM can be used
            Try
               regkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Classes", True)
               Threading.Interlocked.Exchange(_UserMode, 1)
            Catch ex As Exception
               regkey = Registry.CurrentUser.OpenSubKey("Software\Classes", True)
               Threading.Interlocked.Exchange(_UserMode, 2)
            End Try
         Case 1 ' can use HKLM
            regkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\Classes", True)
         Case 2 ' has to use HKCU
            regkey = Registry.CurrentUser.OpenSubKey("Software\Classes", True)
      End Select
      Return regkey
   End Function




End Class






