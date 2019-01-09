var language;
var on = false;
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('FollowHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy_info = connection.createHubProxy('InfoHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	//parent.onload('Follow');
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



$("#back_index").click(function() {
	if(isRunning = true) {
		contosoChatHubProxy2.invoke('BackHome').done(function(s) {
			if(s == true) {
				window.location.href = "http://localhost:15689/index.html"
			}
		});
	} else {
		console.log("断开连接");
	}
});


function tip(s) {
		$(".tip").css("display", "block")
		$(".tip").text(s);
		setTimeout('$(".tip").css("display","none")', 1500)
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