VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "Crypto"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'Option Explicit
'Option Compare Text
Private Declare Function compress Lib "ZLIB.DLL" _
        (ByVal compr As String, comprLen As Any, _
        ByVal buf As String, ByVal buflen As Long) As Long
        
Private Declare Function uncompress Lib "ZLIB.DLL" _
        (ByVal uncompr As String, uncomprLen As Any, _
        ByVal compr As String, ByVal lcompr As Long) As Long

Private Const Z_OK              As Long = 0
Private Const Z_STREAM_END      As Long = 1
Private Const Z_NEED_DICT       As Long = 2
Private Const Z_ERRNO           As Long = -1
Private Const Z_STREAM_ERROR    As Long = -2
Private Const Z_DATA_ERROR      As Long = -3
Private Const Z_MEM_ERROR       As Long = -4
Private Const Z_BUF_ERROR       As Long = -5
Private Const Z_VERSION_ERROR   As Long = -6


Private aDecTab(255) As Integer
Private aEncTab(63) As Byte

Private Type Pattern
    Text As String
    TimesRepeated As Integer
    Position As Long
End Type



Public Function EncodeStr64(ByRef sInput As String) As String
' Return radix64 encoding of string of binary values
' Does not insert CRLFs. Just returns one long string,
' so it's up to the user to add line breaks or other formatting.
' Version 4: Use Byte array and StrConv - much faster
    Dim abOutput() As Byte  ' Version 4: now a Byte array
    Dim sLast As String
    Dim b(3) As Byte    ' Version 4: Now 3 not 2
    Dim j As Integer
    Dim i As Long, nLen As Long, nQuants As Long
    Dim iIndex As Long
    
    EncodeStr64 = ""
    nLen = Len(sInput)
    nQuants = nLen \ 3
    iIndex = 0
    Call MakeEncTab
    If (nQuants > 0) Then
        ReDim abOutput(nQuants * 4 - 1)
        ' Now start reading in 3 bytes at a time
        For i = 0 To nQuants - 1
            For j = 0 To 2
               b(j) = Asc(Mid(sInput, (i * 3) + j + 1, 1))
            Next
            Call EncodeQuantumB(b)
            abOutput(iIndex) = b(0)
            abOutput(iIndex + 1) = b(1)
            abOutput(iIndex + 2) = b(2)
            abOutput(iIndex + 3) = b(3)
            iIndex = iIndex + 4
        Next
        EncodeStr64 = StrConv(abOutput, vbUnicode)
    End If
    
    ' Cope with odd bytes
    ' (no real performance hit by using strings here)
    Select Case nLen Mod 3
    Case 0
        sLast = ""
    Case 1
        b(0) = Asc(Mid(sInput, nLen, 1))
        b(1) = 0
        b(2) = 0
        Call EncodeQuantumB(b)
        sLast = StrConv(b(), vbUnicode)
        ' Replace last 2 with =
        sLast = Left(sLast, 2) & "=="
    Case 2
        b(0) = Asc(Mid(sInput, nLen - 1, 1))
        b(1) = Asc(Mid(sInput, nLen, 1))
        b(2) = 0
        Call EncodeQuantumB(b)
        sLast = StrConv(b(), vbUnicode)
        ' Replace last with =
        sLast = Left(sLast, 3) & "="
    End Select
    
    EncodeStr64 = EncodeStr64 & sLast
End Function

Public Function DecodeStr64(ByRef sEncoded As String) As String
' Return string of decoded binary values given radix64 string
' Ignores any chars not in the 64-char subset
' Version 4: Use Byte array and StrConv - much faster
    Dim abDecoded() As Byte 'Version 4: Now a Byte array
    Dim d(3) As Byte
    Dim C As Integer        ' NB Integer to catch -1 value
    Dim di As Integer
    Dim i As Long
    Dim nLen As Long
    Dim iIndex As Long
    
    nLen = Len(sEncoded)
    If nLen < 4 Then
        Exit Function
    End If
    ReDim abDecoded(((nLen \ 4) * 3) - 1) 'Version 4: Now base zero
    
    iIndex = 0  ' Version 4: Changed to base 0
    di = 0
    Call MakeDecTab
    ' Read in each char in turn
    For i = 1 To Len(sEncoded)
        C = CByte(Asc(Mid(sEncoded, i, 1)))
        C = aDecTab(C)
        If C >= 0 Then
            d(di) = CByte(C)    ' Version 3.1: add CByte()
            di = di + 1
            If di = 4 Then
                abDecoded(iIndex) = SHL2(d(0)) Or (SHR4(d(1)) And &H3)
                iIndex = iIndex + 1
                abDecoded(iIndex) = SHL4(d(1) And &HF) Or (SHR2(d(2)) And &HF)
                iIndex = iIndex + 1
                abDecoded(iIndex) = SHL6(d(2) And &H3) Or d(3)
                iIndex = iIndex + 1
                If d(3) = 64 Then
                    iIndex = iIndex - 1
                    abDecoded(iIndex) = 0
                End If
                If d(2) = 64 Then
                    iIndex = iIndex - 1
                    abDecoded(iIndex) = 0
                End If
                di = 0
            End If
        End If
    Next i
    ' Convert to a string
    DecodeStr64 = StrConv(abDecoded(), vbUnicode)
    ' Remove any unwanted trailing chars
    DecodeStr64 = Left(DecodeStr64, iIndex)
End Function

Private Sub EncodeQuantumB(b() As Byte)
' Expects at least 4 bytes in b, i.e. Dim b(3) As Byte
    
    Dim b0 As Byte, b1 As Byte, b2 As Byte, b3 As Byte
     
    b0 = SHR2(b(0)) And &H3F
    b1 = SHL4(b(0) And &H3) Or (SHR4(b(1)) And &HF)
    b2 = SHL2(b(1) And &HF) Or (SHR6(b(2)) And &H3)
    b3 = b(2) And &H3F
    
    b(0) = aEncTab(b0)
    b(1) = aEncTab(b1)
    b(2) = aEncTab(b2)
    b(3) = aEncTab(b3)
    
End Sub


Private Function MakeDecTab()
' Set up Radix 64 decoding table
    Dim t As Integer
    Dim C As Integer

    For C = 0 To 255
        aDecTab(C) = -1
    Next
  
    t = 0
    For C = Asc("A") To Asc("Z")
        aDecTab(C) = t
        t = t + 1
    Next
  
    For C = Asc("a") To Asc("z")
        aDecTab(C) = t
        t = t + 1
    Next
    
    For C = Asc("0") To Asc("9")
        aDecTab(C) = t
        t = t + 1
    Next
    
    C = Asc("+")
    aDecTab(C) = t
    t = t + 1
    
    C = Asc("/")
    aDecTab(C) = t
    t = t + 1
    
    C = Asc("=")    ' flag for the byte-deleting char
    aDecTab(C) = t  ' should be 64

End Function

Private Function MakeEncTab()
' Set up Radix 64 encoding table in bytes
    Dim i As Integer
    Dim C As Integer

    i = 0
    For C = Asc("A") To Asc("Z")
        aEncTab(i) = C
        i = i + 1
    Next
  
    For C = Asc("a") To Asc("z")
        aEncTab(i) = C
        i = i + 1
    Next
    
    For C = Asc("0") To Asc("9")
        aEncTab(i) = C
        i = i + 1
    Next
    
    C = Asc("+")
    aEncTab(i) = C
    i = i + 1
    
    C = Asc("/")
    aEncTab(i) = C
    i = i + 1
    
End Function

' Version 3: ShiftLeft and ShiftRight functions improved.
Private Function SHL2(ByVal bytValue As Byte) As Byte
' Shift 8-bit value to left by 2 bits
' i.e. VB equivalent of "bytValue << 2" in C
    SHL2 = (bytValue * &H4) And &HFF
End Function

Private Function SHL4(ByVal bytValue As Byte) As Byte
' Shift 8-bit value to left by 4 bits
' i.e. VB equivalent of "bytValue << 4" in C
    SHL4 = (bytValue * &H10) And &HFF
End Function

Private Function SHL6(ByVal bytValue As Byte) As Byte
' Shift 8-bit value to left by 6 bits
' i.e. VB equivalent of "bytValue << 6" in C
    SHL6 = (bytValue * &H40) And &HFF
End Function

Private Function SHR2(ByVal bytValue As Byte) As Byte
' Shift 8-bit value to right by 2 bits
' i.e. VB equivalent of "bytValue >> 2" in C
    SHR2 = bytValue \ &H4
End Function

Private Function SHR4(ByVal bytValue As Byte) As Byte
' Shift 8-bit value to right by 4 bits
' i.e. VB equivalent of "bytValue >> 4" in C
    SHR4 = bytValue \ &H10
End Function

Private Function SHR6(ByVal bytValue As Byte) As Byte
' Shift 8-bit value to right by 6 bits
' i.e. VB equivalent of "bytValue >> 6" in C
    SHR6 = bytValue \ &H40
End Function



'*******************************************************************************
' CompressString (FUNCTION)
'
' PARAMETERS:
' (In) - StringToCompress - Variant - String to compress
'
' RETURN VALUE:
' String - Compressed string
'
' DESCRIPTION:
' Compresses a string with the Zlib compress routine. Sticks the number of
' characters on the front of the string to aid with decompressing the data
' later.
'*******************************************************************************
Public Function CompressString(ByRef StringToCompress As String) As String
    Dim sCompressed     As String
    Dim lCompressedLen  As Long
    Dim lStringLen      As String
    Dim lReturn         As Long
    
    lStringLen = Len(StringToCompress)
    lCompressedLen = (lStringLen * 1.01) + 13
    sCompressed = Space(lCompressedLen)
    
    lReturn = compress(sCompressed, lCompressedLen, StringToCompress, lStringLen)
    
    Select Case lReturn
        Case Z_OK
            sCompressed = Left(sCompressed, lCompressedLen)
            CompressString = lStringLen & ":" & sCompressed
        Case Z_MEM_ERROR
            Err.Raise vbObjectError + Abs(lReturn), "CompressString", _
                "Insufficient memory to compress string"
        Case Z_BUF_ERROR
            Err.Raise vbObjectError + Abs(lReturn), "CompressString", _
                "Insufficient space in output buffer to compress string"
        Case Else
            Err.Raise vbObjectError + Abs(lReturn), "CompressString", _
                "Unknown error during compress operation"
    End Select
End Function

'*******************************************************************************
' UncompressString (FUNCTION)
'
' PARAMETERS:
' (In) - CompressedString - Variant -
'
' RETURN VALUE:
' String -
'
' DESCRIPTION:
' Uncompresses a string with the Zlib uncompress routine that has been previously
' compressed with the CompressString function in this module as it relies on the
' string starting with the number of characters required to output the string.
'*******************************************************************************
Public Function UncompressString(ByRef CompressedString As String) As String
    Dim sUncompressedString As String
    Dim lUncompressedLen    As Long
    Dim sBuffer             As String
    Dim lBufferLen          As Long
    Dim lColonPosition      As Long
    Dim lReturn             As Long
    
    lUncompressedLen = Val(CompressedString)
    sUncompressedString = Space(lUncompressedLen)
    
    lColonPosition = Len(CStr(lUncompressedLen)) + 1
    sBuffer = Mid(CompressedString, lColonPosition + 1)
    lBufferLen = Len(sBuffer)
    
    lReturn = uncompress(sUncompressedString, lUncompressedLen, sBuffer, lBufferLen)
    
    Select Case lReturn
        Case Z_OK
            UncompressString = sUncompressedString
        Case Z_MEM_ERROR
            Err.Raise vbObjectError + Abs(lReturn), "UncompressString", _
                "Insufficient memory to uncompress string"
        Case Z_BUF_ERROR
            Err.Raise vbObjectError + Abs(lReturn), "UncompressString", _
                "Insufficient space in output buffer to uncompress string"
        Case Z_DATA_ERROR
            Err.Raise vbObjectError + Abs(lReturn), "UncompressString", _
                "Cannot uncompress corrupt data"
        Case Else
            Err.Raise vbObjectError + Abs(lReturn), "UncompressString", _
                "Unknown error during uncompress operation"
    End Select
End Function
