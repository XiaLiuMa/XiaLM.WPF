var phone;
var phone_status;
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('ManualServiceHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	isRunning = true;
	parent.onload('ManualService');
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

phone = {
		//拨打电话
		Call: function() {
			contosoChatHubProxy.invoke('Call');
		},
		//挂断电话
		HungUp: function() {
			contosoChatHubProxy.invoke('HungUp');
		},
		de:function() {
			$('.status').text(config_data);
			$(".on").css("display","block");
			$(".off").css("display","none");
			$('.info').text('点击拨号');
		}
}
//注册事件
contosoChatHubProxy.on('PhoneStateChange',
	function(data) {
		//接收参数
		phone_status = data;
		if(data == "1005") {
			$('.status').text('对方没有应答...');
			$(".on").css("display","none");
			$(".off").css("display","block");
			$('.info').text('点击挂断');
		} else if(data == "1001") {
			$('.status').text('拨打电话中...');
			$(".on").css("display","none");
			$(".off").css("display","block");
			$('.info').text('点击挂断');
		} else if(data == "1002") {
			$('.status').text('通话中...');
			$(".on").css("display","none");
			$(".off").css("display","block");
			$('.info').text('点击挂断');
		}else if(data == "1003") {
			$('.status').text('结束通话');
			$(".on").css("display","none");
			$(".off").css("display","block");
			$('.info').text('点击挂断');
			setTimeout('phone.de()',1000);
		}else if(data == "1004") {
			$('.status').text('对方已挂断');
			$(".on").css("display","none");
			$(".off").css("display","block");
			$('.info').text('点击挂断');
		}else{
			$('.status').text(config_data);
			$(".on").css("display","block");
			$(".off").css("display","none");
			$('.info').text('点击拨号');
		}
	}
);

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

