<%
Class StringBuilder
	Private strArray()
	Private intGrowRate
	Private intItemCount
	Private Sub Class_Initialize()
		
		intGrowRate = 50
		intItemCount = 0
		
		'Redim strArray(intGrowRate)
		
	End Sub
	
	Public Property Get GrowRate
		GrowRate = intGrowRate
	End Property

	Public Property Let GrowRate(value)
		 intGrowRate = value
	End Property
	
	Private Sub InitArray()
			Redim Preserve strArray(intGrowRate)
	End Sub
	
	Public Sub Append(str)
			
		If intItemCount = 0 Then
			Call InitArray
		ElseIf intItemCount > UBound(strArray) Then			
			Redim Preserve strArray(Ubound(strArray) + intGrowRate)
		End If
		
		strArray(intItemCount) = str
		
		intItemCount = intItemCount + 1
	
	End Sub
	
	Public Default Function ToString()
		If intItemCount = 0 Then
			ToString = ""
		Else
			ToString = Join(strArray)
		End If		
	End Function

End Class

%>