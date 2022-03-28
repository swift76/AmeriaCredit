var DEFAULT_DATE_FORMAT = "dd/mm/yyyy";
var DEFAULT_COLUMN_VALUE = "";
var REQUIRED_FIELD_ERROR = "Պարտադիր լրացման դաշտ";
var EMAIL_FORMAT_ERROR = "Էլ․ հասցեի ֆորմատը սխալ է";
var DATE_FORMAT_ERROR = "Ամսաթվի ֆորմատը սխալ է";
var NUMBER_FORMAT_ERROR = "Թվի ֆորմատը սխալ է";
var PASSWORDS_DONT_MATCH = "Գաղտնաբառերը չեն համընկնում ";
var EMPTY_REQUEST_RESPONSE = "Ձեր կատարած հարցմանը բավարարող տվյալ չկա";
var UNLOCK = "Բացել";
var LOCK = "Փակել";
var DATA_REFRESH_INTERVAL = 300; //interval to refresh bank and shop users data in seconds
var TABLE_HEADERS = {
	user:"Օգտագործող",
	name:"Անուն",
	state:"Կարգավիճակ",
	openDate:"Բացման ամսաթիվ",
	passwordExpireDate:"Գաղտնաբառի ժամկետ",
	closeDate:"Փակման ամսաթիվ",
	action:"Գործողություն",
	shop:"Խանութ",
	manager:"Ղեկավար",
	phone:"Հեռախոս",
	authNumber:"Լիազ. համար",
	authDate:"Լիազ. ամսաթիվ",
	authExpireDate:"Լիազ. վերջնաժամկետ"
};