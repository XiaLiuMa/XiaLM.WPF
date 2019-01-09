var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('PassportHub'); //创建连接器
var settime;
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	parent.onload('Passport');
});


//结束识别
contosoChatHubProxy.on('DescriptionPush', function(e) {
	$('.page1').css('display', 'none');
	$('.page2').css('display', 'block');
	$('.Name').html(e.Name);
	$('.ID').html(e.ID);
	$('.Age').html(e.Age);
	$('.Sex').html(e.Sex);
	$('.En_Sex').html(e.En_Sex);
	$('.Country').html(e.Country);
	$('.En_Country').html(e.En_Country);
	$('.PassportType').html(e.PassportType);
	$('.Text').html(e.Text);
	try{
		clearTimeout(settime);
	}catch(e){}
	timer=10;
	daojishi();
});

var timer=10;

function daojishi(){
	if(timer>0){
		timer--;
		$('.time').text(timer)
		settime=setTimeout('daojishi()',1000);
	}else{
		timer=10;
		back();
	}
}
function back(){
	$('.page2').css('display', 'none');
	$('.page1').css('display', 'block');
}

//控制
document.ondragstart = function() {
	return false;
};
document.oncontextmenu = function() {
	return false;
}
$("p").on("taphold", function(e) {
	e.preventDefault();
});