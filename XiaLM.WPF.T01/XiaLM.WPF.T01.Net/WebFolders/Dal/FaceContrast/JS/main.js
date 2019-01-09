var language;
var on = false;
var FaceContrast = {};
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('FaceContrastHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy_info = connection.createHubProxy('InfoHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	FaceContrast.GetStartState();
	parent.onload('FaceContrast');
});

FaceContrast = {
	StartFaceContrast: function() {
		$('.swichforbid').css('display','block');
		contosoChatHubProxy.invoke('StartFaceContrast').done(function() {})
	},
	StopFaceContrast: function() {
		$('.swichforbid').css('display','block');
		contosoChatHubProxy.invoke('StopFaceContrast').done(function() {})
	},
	GetStartState:function(){
		contosoChatHubProxy.invoke('GetStartState').done(function(bo) {
			if(bo){
				$('.swich').html('<img src="images/ON.png" onclick="FaceContrast.StopFaceContrast()">')
			}else{
				$('.swich').html('<img src="images/OFF.png" onclick="FaceContrast.StartFaceContrast()">')
			}
		})
	}
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

contosoChatHubProxy.on("FeedBackFaceContrastState", function(e) {
	$('.swichforbid').css('display','none');
	if(e=="101"){
		$('.swich').html('<img src="images/ON.png" onclick="FaceContrast.StopFaceContrast()">')
		tip('启动成功');
	}else if(e=="102"){
		$('.swich').html('<img src="images/OFF.png" onclick="FaceContrast.StartFaceContrast()">')
		tip('启动失败');
	}else if(e=="103"){
		$('.swich').html('<img src="images/OFF.png" onclick="FaceContrast.StartFaceContrast()">')
		tip('关闭成功');
	}else if(e=="104"){
		$('.swich').html('<img src="images/ON.png" onclick="FaceContrast.StopFaceContrast()">')
		tip('关闭失败');
	}else{
		tip('启动成功');
	}
});

contosoChatHubProxy.on("FeedBackFaceContrastAlarm", function(obj) {
	if(obj.success) {
		var info = obj.response;
		$('.p_name').html(info.passName);
		$('.p_sex').html(info.sex);
		$('.p_age').html(info.age+'岁');
		$('.p_lv').html('等级'+info.level);
		$('.passnum').val(info.cardId);
		$('.passtime').val(info.passTime);
		$('.iotimes').val(info.executeControl);
		$('#similarity').html(parseInt(info.score));
		$('.imgleft').attr('src', info.cardPhotoUrl);
		$('.imgright').attr('src', info.capturePhotoUrl);
		//togglefontpage(false);
	}
});

contosoChatHubProxy.on("FeedBackFaceContrastSnap", function(imgurl) {
	$('.imgmiddle').attr('src','data:image/jpg;base64,'+imgurl)
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
function tip(e){
	$(".tip").css("display", "block");
	$(".tip").html(e)
	setTimeout('$(".tip").css("display","none");', 3000);
}


//function togglefontpage(bo) {
//	if(bo) {
//		$(".pagefont").animate({
//			opacity: 1
//		}, 500);
//		$('.back').css('display','none')
//	} else {
//		$(".pagefont").animate({
//			opacity: 0
//		}, 500);
//		$('.back').css('display','block')
//	}
//}
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

