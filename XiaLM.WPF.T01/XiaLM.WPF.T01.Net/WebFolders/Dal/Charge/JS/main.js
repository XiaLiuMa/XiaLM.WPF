var language;
var on = false;
var Charge = {};
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('ChargeHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy_info = connection.createHubProxy('InfoHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	Charge.IsCharge();
	parent.onload('Charge');
});

Charge = {
	GetInformant: function() {
		contosoChatHubProxy2.invoke('GetInformant').done(function(e) {
			language = e;
			if(e == 'en') {
				$('.SF_en').css('display', 'block');
			} else {
				$('.SF_cn').css('display', 'block');
			}
		})
	},
	Charge: function() {
		//充电
		contosoChatHubProxy.invoke('StartCharge').done(function(s) {
			if(s == true) {
				$("#swichimg").attr("onclick", "Charge.StopCharge()");
				$("#swichimg").attr("src", "images/ON.png");
				$('.charging').css('display','block');
			} else {
				$("#swichimg").attr("onclick", "Charge.Charge()");
				$("#swichimg").attr("src", "images/OFF.png");
				$('.charging').css('display','none');
				failed();
				$("#fail").html("<p>返回值:false</p>")
			}
		});
	},
	StopCharge: function() {
		//停止充电
		contosoChatHubProxy.invoke('StopCharge').done(function(s) {
			if(s == true) {
				$("#swichimg").attr("onclick", "Charge.Charge()");
				$("#swichimg").attr("src", "images/OFF.png");
				$('.charging').css('display','none');
			} else {
				$("#swichimg").attr("onclick", "Charge.StopCharge()");
				$("#swichimg").attr("src", "images/ON.png");
				$('.charging').css('display','block');
				failed();
				$("#fail").html("<p>返回值：false</p>")
			}
		});
	},
	IsCharge: function() {
		contosoChatHubProxy.invoke('IsCharge').done(function(s) {
			console.log(s)
			if(s == true) {
				$("#swichimg").attr("onclick", "Charge.StopCharge()");
				$("#swichimg").attr("src", "images/ON.png");
				$('.charging').css('display','block');
			} else {
				$("#swichimg").attr("onclick", "Charge.Charge()");
				$("#swichimg").attr("src", "images/OFF.png");
			}
		});
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


//充电状态事件
contosoChatHubProxy.on("ChargeStatus", function(pageEvent) {
	if(pageEvent == 1001) {
		on = true;
		$('#test').prepend("充电中1001<br />");
	} else if(pageEvent == 1002) {
		on = false;
		$('.wrapper').css("display", "none");
		$('#test').prepend("充电结束1002<br />");
	} else if(pageEvent == 1003) {
		on = false;
		$('.wrapper').css("display", "none");
		$('#test').prepend("充电错误1003<br />");
	} else if(pageEvent == 1005) {
		failed();
		$("#fail").html("<p>返回值为1005,电量低无法充电</p>");
		$('#test').prepend("电量低无法充电1005<br />");
	} else {
	}
	/// <summary>
	/// 充电中
	/// </summary>
	//Charging=1001,
	/// <summary>
	/// 充电结束
	/// </summary>
	//ChargeEnd=1002,
	/// <summary>
	/// 充电错误
	/// </summary>
	//ChargeError=1003,
	/// <summary>
	/// 寻找充电桩
	/// </summary>
	//FindChargingPile=1004

});

contosoChatHubProxy.on("LowPowerEvent", function() {
	$("#Z-background").css("display", "block");
});

contosoChatHubProxy.on("ChargeEvent", function(b) {
	if(b===true){
		$("#swichimg").attr("src", "images/ON.png");
	}else{
		$("#swichimg").attr("src", "images/OFF.png");
	}
});

contosoChatHubProxy_info.on("PushInfo", function(base_info) {
    $('#ele_num').html(base_info.Power);
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




/*function status() {
	if(on == false) {
		if(language == 'en') {
			$(".swich").attr("src", "images/OFF.png");
		} else {
			$(".swich").attr("src", "images/关.png");
		}
	} else {
		if(language == 'en') {
			$(".swich").attr("src", "images/NO.png");
		} else {
			$(".swich").attr("src", "images/开.png");
		}
	}
}
setInterval("status()", 500);*/

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
