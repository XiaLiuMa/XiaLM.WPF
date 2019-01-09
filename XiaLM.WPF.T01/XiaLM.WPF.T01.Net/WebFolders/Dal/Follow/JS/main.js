var language;
var wb=true;
var on = false;
var Follow = {};
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('FollowHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy_info = connection.createHubProxy('InfoHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	Follow.GetFollowStatu();
	parent.onload('Follow');

});

Follow = {
	StartFollow: function() {
		contosoChatHubProxy.invoke('StartFollow',wb);
		$('.swichforbid').css('display','block');
	},
	StopFollow: function() {
		contosoChatHubProxy.invoke('StopFollow');
		$('.swichforbid').css('display','block');
	},
	RestartFollow: function() {
		contosoChatHubProxy.invoke('RestartFollow');
	},
	GetFollowStatu: function() {
		contosoChatHubProxy.invoke('GetFollowStatu');
	},
	changewb: function(bo) {
		if(bo){
			wb=true;
			$('#swichimg3').attr('src','images/white.png')
			$('#swichimg3').attr('onclick','Follow.changewb(false)')
		}else{
			wb=false;
			$('#swichimg3').attr('src','images/black.png')
			$('#swichimg3').attr('onclick','Follow.changewb(true)')
		}
	},
}
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

//跟随状态事件
contosoChatHubProxy.on("FeedBackFollowState", function(state) {
	$('.swichforbid').css('display','none');
	switch(state) {
		case 101:
			on1();
			tip('启动成功');
			break;
		case 102:
			tip('启动失败');
			break;
		case 103:
			off1();
			tip('关闭成功');
			break;
		case 104:
			tip('关闭失败');
			break;
		case 105:
			tip('白名单跟随成功');
			break;
		case 106:
			tip('白名单跟随失败');
			break;
		case 107:
			tip('黑名单跟随成功');
			break;
		case 108:
			tip('黑名单跟随失败');
			break;
		default:
			tip1('错误');
			break;
	}
});

contosoChatHubProxy.on("LowPowerEvent", function() {
	$("#Z-background").css("display", "block");
});

contosoChatHubProxy2.on("PageBackEvent", function(pageEvent) {
	changepage(pageEvent.Url, pageEvent.MenuName, pageEvent.DescName);
	console.log(pageEvent.DescName);
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

function failed() {
	$("#fail").css("display", "block");
	setTimeout('$("#fail").css("display","none");', 10000);
}

function tip(s) {
	$(".tip").css("display", "block")
	$(".tip").text(s);
	setTimeout('$(".tip").css("display","none")', 1500)
}

function on1() {
	$('#swichimg').attr('src', 'images/ON.png')
	$('.imgshow').attr('src', 'images/BGG.gif')
	$("#swichimg").attr("onclick", "Follow.StopFollow()");
	$("#swich2").css("display", "block");
}

function off1() {
	$('#swichimg').attr('src', 'images/OFF.png')
	$('.imgshow').attr('src', 'images/BGS.png')
	$("#swichimg").attr("onclick", "Follow.StartFollow()");
	$("#swich2").css("display", "none");
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