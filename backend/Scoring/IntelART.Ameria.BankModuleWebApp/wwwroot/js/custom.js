var bankUsers = [];
var shopUsers = [];
var shops = [];
$(document).ready(function() {
	$('#exportCSVBank').click(function(){
		
		var arr = [];
		for(var i = 0; i < bankUsers.length; i++){
			var obj = {};
			obj[TABLE_HEADERS.user] = bankUsers[i]["LOGIN"];
			obj[TABLE_HEADERS.name] = bankUsers[i]['FIRST_NAME']+' '+bankUsers[i]['LAST_NAME'];
			obj[TABLE_HEADERS.state] = bankUsers[i]["OBJECT_STATE_DESCRIPTION"];
			obj[TABLE_HEADERS.openDate] = bankUsers[i]['CREATE_DATE']? dateFormat(bankUsers[i]['CREATE_DATE']):DEFAULT_COLUMN_VALUE;
			obj[TABLE_HEADERS.passwordExpireDate] = bankUsers[i]['PASSWORD_EXPIRY_DATE']? dateFormat(bankUsers[i]['PASSWORD_EXPIRY_DATE']):DEFAULT_COLUMN_VALUE;
			obj[TABLE_HEADERS.closeDate] = bankUsers[i]['CLOSE_DATE']? dateFormat(bankUsers[i]['CLOSE_DATE']):DEFAULT_COLUMN_VALUE;
			arr.push(obj);
		}
		JSONToCSVConvertor(arr, "bank_users");
	});
	
	$('#exportCSVShop').click(function(){
		var arr = [];
		for(var i = 0; i < shopUsers.length; i++){
			var obj = {};
			obj[TABLE_HEADERS.user] = shopUsers[i]["LOGIN"];
			obj[TABLE_HEADERS.name] = shopUsers[i]['FIRST_NAME']+' '+shopUsers[i]['LAST_NAME'];
			obj[TABLE_HEADERS.shop] = shopUsers[i]["SHOP_NAME"];
			obj[TABLE_HEADERS.manager] = shopUsers[i]["IS_MANAGER"]? "Այո":"Ոչ";
			obj[TABLE_HEADERS.phone] = formatPhoneNumber(shopUsers[i]["MOBILE_PHONE"]);
			//obj[TABLE_HEADERS.authNumber] = shopUsers[i]["AUTHORIZATION_NUMBER"];
			//obj[TABLE_HEADERS.authDate] = shopUsers[i]['AUTHORIZATION_DATE']? dateFormat(shopUsers[i]['AUTHORIZATION_DATE']):DEFAULT_COLUMN_VALUE;
			//obj[TABLE_HEADERS.authExpireDate] = shopUsers[i]['AUTHORIZATION_EXPIRY']? dateFormat(shopUsers[i]['AUTHORIZATION_EXPIRY']):DEFAULT_COLUMN_VALUE;
			obj[TABLE_HEADERS.state] = shopUsers[i]["OBJECT_STATE_DESCRIPTION"];
			obj[TABLE_HEADERS.openDate] = shopUsers[i]['CREATE_DATE']? dateFormat(shopUsers[i]['CREATE_DATE']):DEFAULT_COLUMN_VALUE;
			obj[TABLE_HEADERS.passwordExpireDate] = shopUsers[i]['PASSWORD_EXPIRY_DATE']? dateFormat(shopUsers[i]['PASSWORD_EXPIRY_DATE']):DEFAULT_COLUMN_VALUE;
			obj[TABLE_HEADERS.closeDate] = shopUsers[i]['CLOSE_DATE']? dateFormat(shopUsers[i]['CLOSE_DATE']):DEFAULT_COLUMN_VALUE;
			
			arr.push(obj);
		}
		JSONToCSVConvertor(arr, "shop_users");
		
	});
	//$( "#shopuserAuthDate" ).datepicker({maxDate:new Date()});
	//$('#shopuserAuthDate').datepicker( "option", "dateFormat", DEFAULT_DATE_FORMAT.replace("yyyy","yy") );

	//$( "#shopuserAuthExpireDate" ).datepicker({minDate:new Date()});
	//$('#shopuserAuthExpireDate').datepicker( "option", "dateFormat", DEFAULT_DATE_FORMAT.replace("yyyy","yy") );
	
	$("#shopuserPhone, #shopuserAuthCode").keydown(function (e) {
        // Allow: backspace, delete, tab, escape, enter
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13,  190]) !== -1 ||
             // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) || 
			// Allow: Ctrl+V, Command+V
            (e.keyCode === 86 && (e.ctrlKey === true || e.metaKey === true)) || 
			// Allow: Ctrl+C, Command+C
            (e.keyCode === 67 && (e.ctrlKey === true || e.metaKey === true)) || 
             // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
                 // let it happen, don't do anything
                 return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
	
	$("#shopuserType").val("");
	
    $("#shopuserPhone, #shopuserAuthCode").keyup(function (e) {
        // When user select text in the document, also abort.
			var selection = window.getSelection().toString();
			if ( selection !== '' ) {
				return;
			}
			// When the arrow keys are pressed, abort.
			if ( $.inArray( event.keyCode, [38,40,37,39] ) !== -1 ) {
				return;
			}
			var $this = $( this );
			// Get the value.
			var input = $this.val();
			var input = input.replace(/[\D\s\._\-]+/g, "");
			$this.val(input);
    });

	$("#shopuserPhone, #shopuserAuthCode").change(function (e) {
        // When user select text in the document, also abort.
			var selection = window.getSelection().toString();
			if ( selection !== '' ) {
				return;
			}
			// When the arrow keys are pressed, abort.
			if ( $.inArray( event.keyCode, [38,40,37,39] ) !== -1 ) {
				return;
			}
			var $this = $( this );
			// Get the value.
			var input = $this.val();
			var input = input.replace(/[\D\s\._\-]+/g, "");
			$this.val(input);
    });
	
	$("#shopuserPhone, #shopuserAuthCode").bind('paste', function(event) {
       var selection = window.getSelection().toString();
			if ( selection !== '' ) {
				return;
			}
			// When the arrow keys are pressed, abort.
			if ( $.inArray( event.keyCode, [38,40,37,39] ) !== -1 ) {
				return;
			}
			var $this = $( this );
			// Get the value.
			setTimeout( function() {
				var input = $this.val();
				var input = input.replace(/[\D\s\._\-]+/g, "");
				$this.val(input);
			}, 100);
    });

	
	
	$("#add-shop-user").on("hidden.bs.modal", function () {
		$("#shopuserUser").val("");
		$("#shopuserName").val("");
		$("#shopuserEmail").val("");
		$("#shopuserLastName").val("");
		$("#shopuserPhone").val("");
		$('#shopuserShop').combotree("setValue", "");	
		//$("#shopuserAuthCode").val("");
		//$("#shopuserAuthDate").val("");
		//$("#shopuserAuthExpireDate").val("");
		$("#shopuserType").val("");
        $("#shopUserId").val("");
        $("#shopUserStateId").val("");
		$("#shopuserPrefix").val("");
		$("#add-shop-user .btn-success").html("Ավելացնել");
		$("#add-shop-user .modal-title").html("Ավելացնել խանութի օգտագործող");
		removeAllErrors();
		$("#add-shop-user input, #add-shop-user select, #add-shop-user .btn-success").prop('disabled', false);
		$("#shopuserPrefix").prop('disabled', true);
	});
	
	$("#add-bank-user").on("hidden.bs.modal", function () {
		$("#bankuserUser").val("");
		$("#bankuserName").val("");
		$("#bankuserLastName").val("");
		$("#bankuserEmail").val("");
        $("#bankUserId").val("");
        $("#bankUserStateId").val("");
		$("#add-bank-user .btn-success").html("Ավելացնել");
		$("#add-bank-user .modal-title").html("Ավելացնել բանկի օգտագործող");
		removeAllErrors();
		$("#add-bank-user input, #add-bank-user .btn-success").prop('disabled', false);
		
	});
	getShops();
	getBankUsers();
	getShopUsers();
	
	window.setInterval(function(){
		getBankUsers();
		getShopUsers();
	},DATA_REFRESH_INTERVAL*1000);
} );

function getShops(){
	GetRequest("/api/bank/Shops", shopsResponse);
}

function shopsResponse(data){
	if(data && data.length){
		shops = data;
		var options = "";
		for(var i = 0; i < data.length; i++){
			options += '<option value="'+data[i]["CODE"]+'">'+data[i]["NAME"]+'</option>';
		}
		$("#shopuserShop").html(options);
		$("#shopuserShop").val('');
		var shopData = [];
		for(var i = 0; i < data.length; i++){
			if(data[i]["HEAD_CODE"].trim()){
				var obj = getObjectByCriteria(shopData, "id", data[i]["HEAD_CODE"]);
				if(obj){
					obj.children.push({id:data[i]["CODE"], text: data[i]["NAME"]});
				}
			}else{
				shopData.push({id:data[i]["CODE"], text: data[i]["NAME"], children:[]});
			}
		}
		$('#shopuserShop').combotree();
		$('#shopuserShop').combotree("loadData", shopData);	
		
		$('#shopuserShop').combotree({onChange:selectShop});
		$('#shopuserShop').combotree("setValue", "");
		
	}
}

function getBankUsers(){
	//send request
	//call drawBankUsers on response 
	showLoading($("#data-admin tbody"));
    GetRequest("/api/bank/BankUsers",drawBankUsers);
}

function GetRequest(url, callback) {
   $.get( url,callback);
}

function PostRequest(url, postData, callback){
	
	$.ajax({
		url: url,
		method: "POST",
		data: postData
	}).done(callback)
	.fail(function(xhr){
		callback(xhr.responseJSON);
	})
}

function drawBankUsers(data) {
	removeLoading($("#data-admin tbody"));
	bankUsers = data;
	var headers = [
		{title:TABLE_HEADERS.user},
		{title:TABLE_HEADERS.name}, 
		{title:TABLE_HEADERS.state},
		{title:TABLE_HEADERS.openDate},
		{title:TABLE_HEADERS.passwordExpireDate},
		{title:TABLE_HEADERS.closeDate},
		{title:TABLE_HEADERS.action}
	];
	var tableData = [];
	
	for(var i = 0; i < data.length; i++){
		var obj = [];
		obj.push(data[i]["LOGIN"]);
		obj.push(data[i]['FIRST_NAME']+' '+data[i]['LAST_NAME']);
		obj.push(data[i]['OBJECT_STATE_DESCRIPTION']);
		obj.push(data[i]['CREATE_DATE']? dateFormat(data[i]['CREATE_DATE']):DEFAULT_COLUMN_VALUE);
		obj.push(data[i]['PASSWORD_EXPIRY_DATE']? dateFormat(data[i]['PASSWORD_EXPIRY_DATE']):DEFAULT_COLUMN_VALUE);
		obj.push(data[i]['CLOSE_DATE']? dateFormat(data[i]['CLOSE_DATE']):DEFAULT_COLUMN_VALUE);
		obj.push('<a onclick="showOpenCloseBankUser(event,'+i+')" title="'+(data[i]["OBJECT_STATE_ID"]==2? UNLOCK:LOCK)+'"  class="'+(data[i]["OBJECT_STATE_ID"]==2?"unlock":"lock")+'" href="#"></a>');
		tableData.push(obj);
	}
	$('#data-admin').DataTable({
		destroy: true,
		data:tableData,
		columns:headers,
		'createdRow': function( row, data, dataIndex ) {
      		$(row).attr('index', dataIndex);
  		},
		"oLanguage": {
			"sEmptyTable": EMPTY_REQUEST_RESPONSE
		}
	});
	
	$('#data-admin').off('click', 'tr').on('click', 'tr', function () {
        var index = parseInt($(this).attr("index"));
		if(isNaN(index)){
			return;
		}
		var obj = bankUsers[index];
		$("#bankuserUser").val(obj["LOGIN"]);
		$("#bankuserName").val(obj["FIRST_NAME"]);
		$("#bankuserLastName").val(obj["LAST_NAME"]);
		$("#bankuserEmail").val(obj["EMAIL"]);
        $("#bankUserId").val(obj["ID"]);
        $("#bankUserStateId").val(obj["OBJECT_STATE_ID"]);
		$("#add-bank-user .modal-title").html("Բանկի օգտագործողի տվյալների խմբագրում");
		$("#add-bank-user .btn-success").html("Կատարել");
		$('#add-bank-user').modal('toggle');
		
		if(obj["OBJECT_STATE_ID"] == 2){
			$("#add-bank-user input, #add-bank-user .btn-success").prop('disabled', true);
		}
    } );
}

function addBankUser(){
	if(validateForm($('#add-bank-user'))){
		showLoading($("#add-bank-user .modal-body"))
		var bankUser = {};
		bankUser.FIRST_NAME = $("#bankuserName").val();
		bankUser.LAST_NAME = $("#bankuserLastName").val();
		bankUser.LOGIN = $("#bankuserUser").val();
        bankUser.EMAIL = $("#bankuserEmail").val();
		bankUser.IS_ADMINISTRATOR = 1;
		var id = $("#bankUserId").val();
		if(id){
			bankUser.ID = id;
        }
        var objectStateId = $("#bankUserStateId").val();
        if(objectStateId) {
            bankUser.OBJECT_STATE_ID = objectStateId;
        }
		PostRequest("/api/bank/BankUsers",JSON.stringify(bankUser), addBankUserResponse);
		
	}
}

function addBankUserResponse(data){
	removeLoading($("#add-bank-user .modal-body"));
	if(data && data.Message){
		$("#addBankUserError").text(data.Message);
	}else{
		$('#add-bank-user').modal('toggle');
		getBankUsers();
	}
}

function getShopUsers(){
	//send Request
	//call drawShopUsers on response
	showLoading($("#data-shop tbody"));
	GetRequest("/api/bank/ShopUsers",drawShopUsers);
}

function drawShopUsers(data){
	removeLoading($("#data-shop tbody"));
	shopUsers = data;
	var headers = [
		{title:TABLE_HEADERS.user},
		{title:TABLE_HEADERS.name}, 
		{title:TABLE_HEADERS.shop}, 
		{title:TABLE_HEADERS.manager}, 
		{title:TABLE_HEADERS.phone}, 
		//{title:TABLE_HEADERS.authNumber}, 
		//{title:TABLE_HEADERS.authDate}, 
		//{title:TABLE_HEADERS.authExpireDate}, 
		{title:TABLE_HEADERS.state}, 
		{title:TABLE_HEADERS.openDate}, 
		{title:TABLE_HEADERS.passwordExpireDate}, 
		{title:TABLE_HEADERS.closeDate},
		{title:TABLE_HEADERS.action}
	];
	var tableData = [];
	
	for(var i = 0; i < data.length; i++){
		var obj = [];
		obj.push(data[i]["LOGIN"]);
		obj.push(data[i]['FIRST_NAME']+' '+data[i]['LAST_NAME']);
		obj.push(data[i]['SHOP_NAME']);
		obj.push('<input type="checkbox" disabled="disabled" '+(data[i]["IS_MANAGER"]?'checked="checked"':'')+'></input>');
		obj.push(formatPhoneNumber(data[i]['MOBILE_PHONE']));
		//obj.push(data[i]['AUTHORIZATION_NUMBER']);
		//obj.push(data[i]['AUTHORIZATION_DATE']? dateFormat(data[i]['AUTHORIZATION_DATE']):DEFAULT_COLUMN_VALUE);
		//obj.push(data[i]['AUTHORIZATION_EXPIRY']? dateFormat(data[i]['AUTHORIZATION_EXPIRY']):DEFAULT_COLUMN_VALUE);
		obj.push(data[i]['OBJECT_STATE_DESCRIPTION']);
		obj.push(data[i]['CREATE_DATE']? dateFormat(data[i]['CREATE_DATE']):DEFAULT_COLUMN_VALUE);
		obj.push(data[i]['PASSWORD_EXPIRY_DATE']? dateFormat(data[i]['PASSWORD_EXPIRY_DATE']):DEFAULT_COLUMN_VALUE);
		obj.push(data[i]['CLOSE_DATE']? dateFormat(data[i]['CLOSE_DATE']):DEFAULT_COLUMN_VALUE);
		obj.push('<a onclick="showOpenCloseShopUser(event,'+i+')" title="'+(data[i]["OBJECT_STATE_ID"]==2? UNLOCK:LOCK)+'" class="'+(data[i]["OBJECT_STATE_ID"]==2?"unlock":"lock")+'" href="#"></a>');
		tableData.push(obj);
	}
	$('#data-shop').DataTable({
		destroy: true,
		data:tableData,
		columns:headers,
		'createdRow': function( row, data, dataIndex ) {
      		$(row).attr('index', dataIndex);
  		},
		"oLanguage": {
			"sEmptyTable": EMPTY_REQUEST_RESPONSE
		}
	});
	$('#data-shop').off('click', 'tr').on('click', 'tr', function () {
        var index = parseInt($(this).attr("index"));
		if(isNaN(index)){
			return;
		}
		var obj = shopUsers[index];
		var login = obj["LOGIN"];
		$("#shopuserUser").val(login.substring(login.indexOf('_')+1));
		$("#shopuserName").val(obj["FIRST_NAME"]);
		$("#shopuserLastName").val(obj["LAST_NAME"]);
		$("#shopuserPhone").val(obj["MOBILE_PHONE"]);
		$('#shopuserShop').combotree("setValue", obj["SHOP_CODE"]);
		//$("#shopuserPrefix").val(obj["SHOP_CODE"]+"_");
		//$("#shopuserAuthCode").val(obj["AUTHORIZATION_NUMBER"]);
		//$("#shopuserAuthDate").datepicker("setDate",new Date(obj["AUTHORIZATION_DATE"]));
		//$("#shopuserAuthExpireDate").datepicker("setDate",new Date(obj["AUTHORIZATION_EXPIRY"]));
		$("#shopuserEmail").val(obj["EMAIL"]);
		$("#shopuserType").val((obj["IS_MANAGER"]? "manager" : "worker"));
        $("#shopUserId").val(obj["ID"]);
        $("#shopUserStateId").val(obj["OBJECT_STATE_ID"]);
		$("#add-shop-user .modal-title").html("Խանութի օգտագործողի տվյալների խմբագրում");
		$("#add-shop-user .btn-success").html("Կատարել");
		
		$('#add-shop-user').modal('toggle');
		
		if(obj["OBJECT_STATE_ID"] == 2){
			$("#add-shop-user input, #add-shop-user select, #add-shop-user .btn-success").prop('disabled', true);
			
		}else{
			$("#shopuserShop").prop("disabled",true);
		}
		
    } );
}

function addShopUser(){
	if(validateForm($('#add-shop-user'))){
		showLoading($("#add-shop-user .modal-body"))
		var shopUser = {};
		shopUser.LOGIN = $("#shopuserUser").val();
        shopUser.EMAIL = $("#shopuserEmail").val();
        shopUser.FIRST_NAME = $("#shopuserName").val();
        shopUser.LAST_NAME = $("#shopuserLastName").val();
        shopUser.MOBILE_PHONE = $("#shopuserPhone").val();
        shopUser.SHOP_CODE = $("#shopuserShop").combotree('getValue');
		for(var i = 0; i < shops.length; i++){
			if(shops[i]["CODE"] == shopUser.SHOP_CODE){
				shopUser.SHOP_NAME = shops[i]["NAME"];
				break;
			}
		}
        //shopUser.AUTHORIZATION_NUMBER = $("#shopuserAuthCode").val();
        //shopUser.AUTHORIZATION_DATE = $("#shopuserAuthDate").datepicker("getDate").toISOString();
        //shopUser.AUTHORIZATION_EXPIRY = $("#shopuserAuthExpireDate").datepicker("getDate").toISOString();
        shopUser.IS_MANAGER = $("#shopuserType").val() == "manager"? true : false;
		var id = $("#shopUserId").val();
		if(id){
			shopUser.ID = id;
        }
        var objectStateId = $("#shopUserStateId").val();
        if (objectStateId) {
            shopUser.OBJECT_STATE_ID = objectStateId;
        }
		PostRequest("/api/bank/ShopUsers",JSON.stringify(shopUser), addShopUserResponse);
	}
}

function addShopUserResponse(data){
	removeLoading($("#add-shop-user .modal-body"));
	if(data && data.Message){
		$("#addShopUserError").text(data.Message);
	}else{
		$('#add-shop-user').modal('toggle');
		getShopUsers();
	}
	
}


function showOpenCloseBankUser(event, index){
	event.stopPropagation();
	window.event.cancelBubble = "true";
	var index = parseInt(index);
	var obj = bankUsers[index];
	
	if(obj.OBJECT_STATE_ID == 1){
		$("#openCloseBankUserText").html("Կատարե՞լ փակում");
		$("#openCloseBankUser .modal-title").html("Բանկի օգտագործողի փակում");
	}else{
		$("#openCloseBankUser .modal-title").html("Բանկի օգտագործողի բացում");
		$("#openCloseBankUserText").html("Կատարե՞լ բացում");
	}
	
	$("#openCloseBankUserId").val(index);
	$("#openCloseBankUser").modal("toggle");
}

function openCloseBankUser(){
	var index = parseInt($("#openCloseBankUserId").val());
	var obj = bankUsers[index];
	var url = "/api/bank/BankUsers/close_open";
	var newState = 3-obj.OBJECT_STATE_ID; //if OBJECT_STATE_ID == 1 chenge to 2, and if OBJECT_STATE_ID == 2 change to 1
	var dataObj = {
		ID:obj.ID,
		OBJECT_STATE_ID: newState
	};
	showLoading($("#openCloseBankUser .modal-body"));
	PostRequest(url, JSON.stringify(dataObj), openCloseBankUserResponse);
}

function openCloseBankUserResponse(data){
	removeLoading($("#openCloseBankUser .modal-body"));
	if(data && data.Message){
		$("#openCloseBankUserError").text(data.Message);
	}else{
		$('#openCloseBankUser').modal('toggle');
		getBankUsers();
	}
}



function showOpenCloseShopUser(event, index){
	event.stopPropagation();
	window.event.cancelBubble = "true";
	var index = parseInt(index);
	var obj = shopUsers[index];
	
	if(obj.OBJECT_STATE_ID == 1){
		$("#openCloseShopUserText").html("Կատարե՞լ փակում");
		$("#openCloseShopUser .modal-title").html("Խանութի օգտագործողի փակում");
	}else{
		$("#openCloseShopUser .modal-title").html("Խանութի օգտագործողի բացում");
		$("#openCloseShopUserText").html("Կատարե՞լ բացում");
	}
	
	$("#openCloseShopUserId").val(index);
	$("#openCloseShopUser").modal("toggle");
}

function openCloseShopUser(){
	var index = parseInt($("#openCloseShopUserId").val());
	var obj = shopUsers[index];
	var url = "/api/bank/ShopUsers/close_open";
	var newState = 3-obj.OBJECT_STATE_ID; //if OBJECT_STATE_ID == 1 chenge to 2, and if OBJECT_STATE_ID == 2 change to 1
	var dataObj = {
		ID:obj.ID,
		OBJECT_STATE_ID: newState
	};
	showLoading($("#openCloseShopUser .modal-body"));
	PostRequest(url, JSON.stringify(dataObj), openCloseShopUserResponse);
}

function openCloseShopUserResponse(data){
	removeLoading($("#openCloseShopUser .modal-body"));
	if(data && data.Message){
		$("#openCloseShopUserError").text(data.Message);
	}else{
		$('#openCloseShopUser').modal('toggle');
		getShopUsers();
	}
}

function selectShop(newValue){
	if(newValue){
		$("#shopuserPrefix").val(newValue+"_");
	}else{
		$("#shopuserPrefix").val("");
	}
	
}
