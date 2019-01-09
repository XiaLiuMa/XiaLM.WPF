var on = false;
var Tanzhengtai = {};
var isRunning = false;
var numval;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('YanZhengTaiHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	parent.onload('YanZhengTai');
});

Tanzhengtai = {
	SetChannelPeopleNum: function() {
		$('.add').css('display','none')
		contosoChatHubProxy.invoke('SetChannelPeopleNum', numval);
	}
}

contosoChatHubProxy.on("ChannelDeviceState", function(s) {
	$('.list').css('display', 'block')
	var strr = ""
	strr += "<p style='color:gold'>通道状态</p><div>";
	for(var i in s){
		strr += '<div style="width: 33%;float: left;font-size: 28px;overflow: hidden;">'+s[i]+'</div>';
	}
	strr += "</div>";
	$('.list').html(strr);
});

contosoChatHubProxy.on("YanZhenTaiDeviceState", function(s) {
	$('.list2').css('display', 'block')
	var strr = ""
	strr += "<p style='color:gold'>验证台状态</p><div>";
	for(var i in s){
		strr += '<div style="width: 33%;float: left;font-size: 28px;overflow: hidden;">'+s[i]+'</div>';
	}
	strr += "</div>";
	$('.list2').html(strr);
});

contosoChatHubProxy.on("ViewText", function(s) {
	$(".textinfo").html(s);
});
contosoChatHubProxy.on("ChannelPeopleNum", function(s) {
	$(".now_num_btn").html(s);
});

function changenum(b, n) {
	numval = parseInt($('.reset').val());
	if(b) {
		if(numval + n < 1000) {
			numval=numval + n
			$('.reset').val(numval);
		} else {
			numval=1000;
			$('.reset').val(1000);
		}
	} else {
		if(numval - n > 0) {
			numval=numval - n
			$('.reset').val(numval);
		} else {
			numval=0;
			$('.reset').val(0);
		}
	}
}

function tip(s) {
	$(".tip").css("display", "block")
	$(".tip").text(s);
	setTimeout('$(".tip").css("display","none")', 1500)
}
//virtualkeyboard();
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