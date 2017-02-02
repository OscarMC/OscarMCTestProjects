	<FORM Name = "frmForm" Id="frmForm" Action="<%=Page.PageFormAction%>" Method="Post" onsubmit='return doSumbit()'>
	
		<INPUT Type="Hidden" Name="__FVERSION"   Id="__FVERSION"   Value="CJCA.YCMG.IACM.1.2">
		<INPUT Type="Hidden" Name="__ACTION"     Id="__ACTION"     Value="">
		<INPUT Type="Hidden" Name="__SOURCE"     Id="__SOURCE"     Value="">
		<INPUT Type="Hidden" Name="__INSTANCE"   Id="__INSTANCE"   Value="">
		<INPUT Type="Hidden" Name="__EXTRAMSG"   Id="__EXTRAMSG"   Value="">				
		<INPUT Type="Hidden" Name="__ISPOSTBACK" Id="__ISPOSTBACK" Value="<%=Page.IsPostBack%>">
		<INPUT Type="Hidden" Name="__ISREDIRECTEDPOSTBACK" Id="__ISREDIRECTEDPOSTBACK" Value="">
		<INPUT Type="Hidden" Name="__SCROLLTOP" Id="__SCROLLTOP" Value="">
		
<%'Call Main()%>
