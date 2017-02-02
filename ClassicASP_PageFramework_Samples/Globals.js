
	var _validationArray = new Array()
	
	function doSubmit() {
		
		//doPostBack("Page_Submit","Page")
		//Just Like this?
		return false
	}
	
	function doPostBack(action,object,instance,xmsg,frmaction) {
				
		var frm = document.frmForm;
		
		if(_validationArray.length>0)
			if(!validate()) return;
						
		if(typeof(instance)=='object') {				
		//alert(instance.type)
				if(instance.type == 'select-one' || instance.type == 'select-multiple')
					frm.__INSTANCE.value   = instance.selectedIndex;	
		}
		else {
			frm.__INSTANCE.value   = instance?instance:'0';	
		}
		
		frm.__ISPOSTBACK.value = "True";
		frm.__ACTION.value     = action;
		frm.__SOURCE.value     = object;
		frm.__EXTRAMSG.value   = xmsg;
		
		//Netscape 4.7 sucks
		if(document.layers && !frmaction) {
			frm.action  = window.location;		
		}
		if(frmaction) { frm.action = frmaction;
			frm.__ISREDIRECTEDPOSTBACK.value = 1
		}
		
		frm.submit();	
		return;
	}
	
	
	function __Validation(objectName,validationType,msg,def) {
		this.objName = objectName
		this.obj = eval("document.frmForm." + objectName)
		this.type = validationType
		this.def  = def
		this.msg = msg
		_validationArray.push(this)
	}

	function __ValidateObject(objectName,validationType,msg,def) {
		var dummy =  new __Validation(objectName,validationType,msg,def)
		
	}	
	function validate() {	
		var msg = "";		
		var validationOK = true
		for(x=0;x<_validationArray.length;x++) {
			var obj = _validationArray[x].obj;
			var vobj = _validationArray[x];
			var objtype = ""
			var hasValue = true			
			
			if(!obj.type) {				
					if(obj.length)
						objtype = 'radio'
					else 
						objtype = obj.type
			}else objtype = obj.type
			
			//if obj.value == vobj.def
			//if obj.CausesValidation
								
			switch(objtype) {
				case 'text':
					hasValue =(obj.value!='')
					break;
				case 'checkbox':
					hasValue = (obj.checked)
					break;
				case 'radio':
					hasValue = (__validatOptionList(obj))
					break;
				case 'select-one':
					hasValue = (obj.selectedIndex>0)
					break;
				case 'select-multiple':
					hasValue = (obj.selectedIndex>0)
					break;
				default:
			}		
			if(vobj.type == 1) {
				if(!hasValue) {
					validationOK = false
					msg = msg + vobj.msg + '\n'
				}
			}
									
		}//for
				
		if(!validationOK)
			alert(msg)
		return validationOK		
	}//fnc	
	
	function __validatOptionList(obj) {
		for(i=0;i<obj.length;i++) {
			if(obj[i].checked)
				return true
		}
		return false
	}
	
	//defaultValue
	function __getValue(o) {
		var x;		
		var obj;
		var value = "";
		
		if(typeof(o) == 'string')
			obj = eval("document.frmForm." + o);
		else
			obj = o;
											
		return obj.value;

	}//__getvalue
	
	
	function AssingObjectStyle(obj,stylename,value) {						
		var r = eval("obj.style." + stylename + "='" + value + "'")
	}
	
	//Do something for the onChange to raise changedevents...