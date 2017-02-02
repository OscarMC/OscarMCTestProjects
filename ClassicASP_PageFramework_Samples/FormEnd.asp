<% 
   Dim BOLCOMPRESS___
   BOLCOMPRESS___ = FALSE
   
   If Page.ViewStateMode = VIEW_STATE_MODE_CLIENT  Then %>
		<INPUT Type="Hidden" Name="__VIEWSTATE"     Id="__VIEWSTATE"     Value="<%=Page.ViewState.GetViewStateBase64(Page.CompressFactor,BOLCOMPRESS___)%>">
<% Else
		Session("VS") = Page.ViewState.GetViewStateBase64(Page.CompressFactor,BOLCOMPRESS___)
   End If

   Page.IsCompressed = BOLCOMPRESS___
   Page.TraceImportantCall Page.Control, "View State Compressed: " & Page.IsCompressed
%>
<INPUT Type="Hidden" Name="__VIEWSTATECOMPRESSED"  Id="__VIEWSTATECOMPRESSED"  Value="<%=Page.IsCompressed%>">

</FORM>
