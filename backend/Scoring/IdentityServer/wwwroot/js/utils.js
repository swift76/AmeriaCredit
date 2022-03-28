
function validateForm(container){
	
	var validateTextInputs = function(){
		var textInputs = $(container).find("input[type='text'],input[type='password']");
		var result = true;
		for(var i = 0; i < textInputs.length; i++){
			var input = textInputs[i];
			if(input.getAttribute("req") == "true" && input.value == ""){
				showErrorOnField(input,REQUIRED_FIELD_ERROR);
				result = false;
				continue;
			}
			var vtype = input.getAttribute("vtype");
			if(vtype == "email" && !isValidEmailAddress(input.value)){
				showErrorOnField(input,EMAIL_FORMAT_ERROR);
				result = false;
				continue;
			}
			if(vtype == "date" && !isValidDate(input.value)){
				showErrorOnField(input,DATE_FORMAT_ERROR);
				result = false;
				continue;
			}
			if(vtype == "num" && !isValidNumber(input.value)){
				showErrorOnField(input,NUMBER_FORMAT_ERROR);
				result = false;
				continue;
			}
			if(input.hasAttribute("match")){
				var input2 = $(input.getAttribute("match"));
				if($(input).val() != input2.val()){
					showErrorOnField(input,PASSWORDS_DONT_MATCH);
					result = false;
					continue;
				}
			}
			
		}
		return result;
	}
	
	var validateFileInputs = function(){
		var fileInputs = $(container).find("input[type='file']");
		var result = true;
		for(var i = 0; i < fileInputs.length; i++){
			var input = fileInputs[i];
			if(input.getAttribute("req") == "true" && input.value == ""){
				showErrorOnField(input,REQUIRED_FIELD_ERROR);
				result = false;
				continue;
			}
		}
		return result;
	}
	
	var validateRadioInputs = function(){
		var radioInputs = $(container).find("input[type='radio']");
		var result = true;
		for(var i = 0; i < radioInputs.length; i++){
			var input = radioInputs[i];
			var name = input.name;
			if($(container).find('input[name='+name+']:checked').val() !== undefined){
				showErrorOnField(input,REQUIRED_FIELD_ERROR);
				result = false;
				continue;
			}
		}
		return result;
	}
	
	var validateSelects = function(){
		var selects = $(container).find("select");
		var result = true;
		for(var i = 0; i < selects.length; i++){
			var input = selects[i];
			if(input.getAttribute("req") == "true" && input.value == ""){
				showErrorOnField(input.parentNode,REQUIRED_FIELD_ERROR);
				result = false;
				continue;
			}
		}
		return result;
	}
	
	
	var result = true;
	removeAllErrors();
	if(!validateTextInputs()){
		result = false;
	}
	if(!validateFileInputs()){
		result = false;
	}
	if(!validateRadioInputs()){
		result = false;
	}
	if(!validateSelects()){
		result = false;
	}
	
	return result;
	
	
}

function showErrorOnField(field, error){
	$( '<span class="validationError">'+error+'</span>' ).insertAfter( field )
}

function removeAllErrors(){
	$( ".validationError" ).remove();
}

function isValidEmailAddress(emailAddress) {
    var pattern = /^([a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+(\.[a-z\d!#$%&'*+\-\/=?^_`{|}~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+)*|"((([ \t]*\r\n)?[ \t]+)?([\x01-\x08\x0b\x0c\x0e-\x1f\x7f\x21\x23-\x5b\x5d-\x7e\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|\\[\x01-\x09\x0b\x0c\x0d-\x7f\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))*(([ \t]*\r\n)?[ \t]+)?")@(([a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\d\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.)+([a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]|[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF][a-z\d\-._~\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]*[a-z\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])\.?$/i;
    return pattern.test(emailAddress);
};

function isValidDate(date){
	var pattern = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
	return pattern.test(date);
}

function isValidNumber(num){
	
	return $.isNumeric(num);
}


var dateFormat = function () {
	var	token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
		timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
		timezoneClip = /[^-+\dA-Z]/g,
		pad = function (val, len) {
			val = String(val);
			len = len || 2;
			while (val.length < len) val = "0" + val;
			return val;
		};

	// Regexes and supporting functions are cached through closure
	return function (date, mask, utc) {
		var dF = dateFormat;

		// You can't provide utc if you skip other args (use the "UTC:" mask prefix)
		if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
			mask = date;
			date = undefined;
		}

		// Passing date through Date applies Date.parse, if necessary
		date = date ? new Date(date) : new Date;
		if (isNaN(date.getTime())) throw SyntaxError("invalid date");

		mask = String(mask || DEFAULT_DATE_FORMAT);

		// Allow setting the utc argument via the mask
		if (mask.slice(0, 4) == "UTC:") {
			mask = mask.slice(4);
			utc = true;
		}

		var	_ = utc ? "getUTC" : "get",
			d = date[_ + "Date"](),
			D = date[_ + "Day"](),
			m = date[_ + "Month"](),
			y = date[_ + "FullYear"](),
			H = date[_ + "Hours"](),
			M = date[_ + "Minutes"](),
			s = date[_ + "Seconds"](),
			L = date[_ + "Milliseconds"](),
			o = utc ? 0 : date.getTimezoneOffset(),
			flags = {
				d:    d,
				dd:   pad(d),
				ddd:  dF.i18n.dayNames[D],
				dddd: dF.i18n.dayNames[D + 7],
				m:    m + 1,
				mm:   pad(m + 1),
				mmm:  dF.i18n.monthNames[m],
				mmmm: dF.i18n.monthNames[m + 12],
				yy:   String(y).slice(2),
				yyyy: y,
				h:    H % 12 || 12,
				hh:   pad(H % 12 || 12),
				H:    H,
				HH:   pad(H),
				M:    M,
				MM:   pad(M),
				s:    s,
				ss:   pad(s),
				l:    pad(L, 3),
				L:    pad(L > 99 ? Math.round(L / 10) : L),
				t:    H < 12 ? "a"  : "p",
				tt:   H < 12 ? "am" : "pm",
				T:    H < 12 ? "A"  : "P",
				TT:   H < 12 ? "AM" : "PM",
				Z:    utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
				o:    (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
				S:    ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
			};

		return mask.replace(token, function ($0) {
			return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
		});
	};
}();

// Internationalization strings
dateFormat.i18n = {
	"dayNames":["Sun","Mon","Tue","Wed","Thu","Fri","Sat","Sunday","Monday","Tuesday","Wednesday","Thursday","Friday","Saturday"],
	"monthNames":["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sept","Oct","Nov","Dec","January","February","March","April","May","June","July","August","September","October","November","December"]
};

function showLoading(container){
	var div = document.createElement("div");
	div.className = "loadingBackground";
	$(container).append(div);
}

function removeLoading(container){
	$(container).find( ".loadingBackground" ).remove();
}


function JSONToCSVConvertor(JSONData,fileName) {
    //If JSONData is not an object then JSON.parse will parse the JSON string in an Object
    var arrData = typeof JSONData != 'object' ? JSON.parse(JSONData) : JSONData;
    
    var CSV = '';    
    //Set Report title in first row or line
    
	var row = "";
		
	//This loop will extract the label from 1st index of on array
	for (var index in arrData[0]) {
		
		//Now convert each value to string and comma-seprated
		row += index + ',';
	}

	row = row.slice(0, -1);
	
	//append Label row with line break
	CSV += row + '\r\n';
   
    //1st loop is to extract each row
    for (var i = 0; i < arrData.length; i++) {
        var row = "";
        
        //2nd loop will extract each column and convert it in string comma-seprated
        for (var index in arrData[i]) {
            row += '"' + arrData[i][index] + '",';
        }

        row.slice(0, row.length - 1);
        
        //add a line break after each row
        CSV += row + '\r\n';
    }

    if (CSV == '') {        
        alert("Invalid data");
        return;
    }   
    
   
    //Initialize file format you want csv or xls
    var uri = 'data:text/csv;charset=utf-8,' + encodeURIComponent(CSV);
    
    // Now the little tricky part.
    // you can use either>> window.open(uri);
    // but this will not work in some browsers
    // or you will not get the correct file extension    
    
    //this trick will generate a temp <a /> tag
    var link = document.createElement("a");    
    link.href = uri;
    
    //set the visibility hidden so it will not effect on your web-layout
    link.style = "visibility:hidden";
    link.download = fileName + ".csv";
    
    //this part will append the anchor tag and remove it after automatic click
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

