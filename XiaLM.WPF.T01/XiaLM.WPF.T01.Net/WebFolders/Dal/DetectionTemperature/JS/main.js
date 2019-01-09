var Temperature = {};
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('DetectionTemperatureHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy_info = connection.createHubProxy('InfoHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	Temperature.GetStrBaseInfo();
	parent.onload('DetectionTemperature');
});

Temperature = {
	CalibrationCoordinates: function() {
		var temperature = parseFloat($('.HTWD').val());
		var distance = parseFloat($('.HTJL').val());
		contosoChatHubProxy.invoke('CalibrationCoordinates', temperature, distance).done(function(s) {

		})
	},
	SetThresholdValue: function() {
		var temperature = parseFloat($('.ZDWD').val());
		contosoChatHubProxy.invoke('SetThresholdValue', temperature).done(function(s) {

		})
	},
	GetStrBaseInfo: function(obj) {
		contosoChatHubProxy.invoke('GetStrBaseInfo').done(function(obj) {
			$('.HTWD').val(obj.temperature)
			$('.HTJL').val(obj.distance)
			$('.ZDWD').val(obj.thresholdValue)
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

contosoChatHubProxy.on("FeedBackInfraredDetectionAlarm", function(obj) {
	$('.XZB').text(obj.xpoint);
	$('.YZB').text(obj.ypoint);
	$('.WD').text(obj.temperature);
	$('.rightimg').attr('src', 'data:image/jpg;base64,'+obj.imgBytes)
	$('.leftimg').attr('src', 'data:image/jpg;base64,'+obj.captureBytes)
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

virtualkeyboard();
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