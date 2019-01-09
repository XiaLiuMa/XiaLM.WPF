var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('PassportHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	isRunning = true;
	parent.onload('Passport');
});

//断线重新连接
connection.disconnected(function() {
	isRunning = false;
	setTimeout(function() {
		if(!isRunning) {
			connection.start().done(function() {
				isRunning = true;
			});
		}
	}, 5000);
});

var Passport;
Passport = {
	Start: function() {
		contosoChatHubProxy.invoke('Start').done(function(s) {

		});
	},
}

//显示提示
contosoChatHubProxy.on('ViewTextEvent', function(s) {
	$('.textarea').empty();
	$('.textarea').html(s);
});

//重新开始
contosoChatHubProxy.on('RestButtonEvent', function() {
	$('.light').css('display', 'none');
	$('.textarea').empty();
	$('.textarea').html('<button class="startbtn btn btn-warning"  onclick="Passport.Start()">点击开始扫描护照</button>')
});
//开始识别
contosoChatHubProxy.on('StartEvent', function() {
	$('.light').css('display', 'block');
});
//结束识别
contosoChatHubProxy.on('EndEvent', function() {
	$('.light').css('display', 'none');
});



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