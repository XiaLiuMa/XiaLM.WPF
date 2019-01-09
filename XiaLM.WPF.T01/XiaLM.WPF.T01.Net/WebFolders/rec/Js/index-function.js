//模块计数器
var i = 0;
var weather_interval;
var isRunning = false;
var model_arr_length = 0;
var setting;
var myself;
var openstatus = false;
var Volume_data;
var io;
var base;
var Dialogue;
var wifi;
var nowcity;
var menulistarr = [];
var HideViewText = false;
var page = 0;
var maxpage = 0;
var infolist = [];
//基础信息
var base_power = 0;

//function right() {
//	$('#content_1').optiscroll('scrollTo', 'right', false, 1000);
//}
//function left1() {
//	$('#content_1').optiscroll('scrollTo', 'left', false, 1000);
//}
var isRunning = false;
var connection = $.hubConnection("http://127.0.0.1:15689");
var contosoChatHubProxy = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('SettingHub');
var contosoChatHubProxy_info = connection.createHubProxy('InfoHub');
var contosoChatHubProxy_DialogueHub = connection.createHubProxy('DialogueHub');
var contosoChatHubProxy_wifi = connection.createHubProxy('NetworkHub');
var MessageViewHub = connection.createHubProxy('MessageViewHub');
connection.start().done(function() {
	isRunning = true;
	io.GetMenuList();
	base.InfoInit();
	base.GetRobotName();
	GetWifiList();
	mouseslide();
	virtualkeyboard();
	Dialogue.GetHideViewTextState();
	$('.systemforbid').css('display', 'none')
})
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

Dialogue = {
	StarRecord: function() {
		contosoChatHubProxy_DialogueHub.invoke('StarRecord');
	},
	GetHideViewTextState: function() {
		contosoChatHubProxy_DialogueHub.invoke('GetHideViewTextState').done(function(b) {
			HideViewText = b;
			Hidebyaftertap()
		})
	},
	Call: function() {
		contosoChatHubProxy_DialogueHub.invoke('Call');
	}
};

setting = {
	GetInformant: function() {
		contosoChatHubProxy.invoke('GetInformant').done(function(e) {
			if(e == 'en') {
				$('.SF_en').css('display', 'block');
			} else {
				$('.SF_cn').css('display', 'block');
			}
		})
	},
	Volumeup: function() {
		try {
			contosoChatHubProxy2.invoke('SetVolume', true).done(function(s) {
				clearTimeout(i);
				$('#Volumevalue').css('display', 'block');
				$('#volumenum').html(parseInt(s));
				i = setTimeout("$('#Volumevalue').css('display','none')", 3000);
			});
		} catch(e) {
			console.log(e.message);
		}
	},
	Volumedown: function() {
		try {
			contosoChatHubProxy2.invoke('SetVolume', false).done(function(s) {
				clearTimeout(i)
				$('#Volumevalue').css('display', 'block');
				$('#volumenum').html(parseInt(s));
				i = setTimeout("$('#Volumevalue').css('display','none')", 3000);
			});
		} catch(e) {
			console.log(e.message);
		}
	},
	SetCity: function() {
		contosoChatHubProxy_info.invoke('SetCity', nowcity);
		$('.cityselect,.citybg').css('display', 'none');
	},
	Robotinfo: function() {
		$('.robotinfo-area').css('display', 'block')
		$('.robotinfo').animate({
			width: "600px"
		}, 150);
	},
	hiddenrobotinfo: function() {
		$('.Robotinfo-num').css('display', 'none')
		$('.robotinfo').animate({
			width: "0px"
		}, 150, function() {
			$('.robotinfo-area').css('display', 'none')
		});
	},
	killrobotinfo: function() {
		$('.Robotinfo-num').css('display', 'none');
		$('.robotinfo-area').css('display', 'none');
		$('.robotinfo').css('width', '0px')
	},
	GetMessages: function() {
		MessageViewHub.invoke('GetMessages').done(function(e) {
			infolist = e;
			var str = '';
			for(var i = 0; i < e.length; i++) {
				str += '<div class="robotinfo-unit" onclick="setting.ClickMessage(' + i + ')">'
				str += '<div class="robotinfo-dec"><img src="rec/img/tip.png">' + e[i].Desc + '</div>'
				if(e[i].Image == null) {
					str += '</div>'
				} else {
					str += '<div class="robotinfo-image"><img src="' + e[i].Image + '" width="200px"></div></div>'
				}
			}
			$('.robotinfo-list').html(str);
			setting.Robotinfo();
		})
	},
	ClickMessage: function(num) {
		MessageViewHub.invoke('ClickMessage', infolist[num]).done(function(e) {
			if(e > 0) {
				$('.Robotinfo-num').css('display', 'block')
			} else {
				$('.Robotinfo-num').css('display', 'none')
			}
		})
	},
	RelieveMalfunction: function() {
		$('#B-background').css('display', 'none')
		contosoChatHubProxy.invoke('RelieveMalfunction')
	},

};

//myself = {
//	open: function() {
//		contosoChatHubProxy.invoke('SelfIntroduction').done(function(s) {
//			$('.about_me').html(s);
//		});
//	},
//	//弹出自我介绍
//	aboutme: function() {
//		var data = localStorage.getItem("myself");
//		var datastatus = localStorage.getItem("myselfstatus");
//		if(datastatus === "true") {
//			localStorage.clear();
//			if(openstatus == false) {
//				$('.mine_info_BG').animate({
//					height: 'show'
//				});
//				$('.about_me').html(data);
//				openstatus == true;
//			};
//		} else {
//			console.log("myselfstatus为false");
//		}
//	}
//};

io = {
	GetMenuList: function() {
		contosoChatHubProxy.invoke('GetMenuList').done(function(s) {
			var str = '';
			sliceArr(s, 10);
			page = 0;
			pushlist();
		});
	},
	Jump: function(Url, Name) {
		if(isRunning = true) {
			contosoChatHubProxy.invoke('Jump', Name);
		} else {
			console.log("断开连接");
		}
	}
};

base = {
	GetRobotName: function() {
		contosoChatHubProxy_info.invoke('GetRobotName').done(function(s) {
			$('.robotname').text(s)
		})
	},
	InfoInit: function() {
		contosoChatHubProxy_info.invoke('InfoInit');
	},
	GetInfo: function() {
		contosoChatHubProxy_info.invoke('GetInfo').done(function(s) {
			base.power(s.Power);
			base.wifistatus(s.WifiInfo.SignalQuality, s.WifiInfo.Usable);
		})
	},
	power: function(data) {
		$('.power_num').text(data);
		if(data > 75) {
			$(".power_img").attr("src", "rec/img/p4.png");
		} else if(data <= 75 && data > 50) {
			$(".power_img").attr("src", "rec/img/p3.png");
		} else if(data <= 50 && data > 25) {
			$(".power_img").attr("src", "rec/img/p2.png");
		} else if(data <= 25 && data > -1) {
			$(".power_img").attr("src", "rec/img/p1.png");
		} else {
			$(".power_img").attr("src", "rec/img/p0.png");
		}
	},
	wifistatus: function(wnum, wab) {
		if(wab === true) {
			if(wnum >= 70 && wnum <= 100) {
				$('#wifiicon').attr("src", "rec/img/w3.png");
			} else if(wnum >= 40 && wnum < 70) {
				$('#wifiicon').attr("src", "rec/img/w2.png");
			} else if(wnum < 40 && wnum > 0) {
				$('#wifiicon').attr("src", "rec/img/w1.png");
			} else {
				$('#wifiicon').attr("src", "");
			}
		} else {
			if(wnum >= 70 && wnum <= 100) {
				$('#wifiicon').attr("src", "rec/img/uw3.png");
			} else if(wnum >= 40 && wnum < 70) {
				$('#wifiicon').attr("src", "rec/img/uw2.png");
			} else if(wnum < 40 && wnum > 0) {
				$('#wifiicon').attr("src", "rec/img/uw1.png");
			} else {
				$('#wifiicon').attr("src", "");
			}
		}
	}
};

function onload(name) {
	contosoChatHubProxy.invoke('OnLoad', name);
}

function BackHome() {
	$('#iframe').attr('src', 'def.html')
	$('#iframe').css('display', 'none');
	que = '';
	ans = '';
	contosoChatHubProxy.invoke('BackHome').done(function(s) {

	})
}

function format(e) {
	if(e < 10) {
		return '0' + e;
	} else {
		return e;
	}
}

//天气
contosoChatHubProxy_info.on("PushWeather", function(s) {
	try {
		$(".city").html(s.results[0].location.name);
		nowcity = s.results[0].location.name;
		$(".wenduzhi").html(s.results[0].now.temperature);
		$(".tianqi").html(s.results[0].now.text);
		$(".time_data").html(new Date().getFullYear() + '/' + (new Date().getMonth() + 1) + '/' + new Date().getDate() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay()));
		$(".time_font").html(format(new Date().getHours()) + ":" + format(new Date().getMinutes()));
		var code = 'rec/weather/' + s.results[0].now.code + '.png';
		$('#weather_icon').attr('src', code);
	} catch(e) {
		console.log(e.message);
	}
});
//电量值
contosoChatHubProxy_info.on("PushInfo", function(base_info) {
	base.power(base_info.Power);
	base.wifistatus(base_info.WifiInfo.SignalQuality, base_info.WifiInfo.Usable);
});

//录音
contosoChatHubProxy_DialogueHub.on("RecoedEvent", function(s) {
	try {
		if(s == 0) {
			$('#speaktext').text('正在识别...');
			$('#click_speak').addClass('anime1');
			$('.orange').empty();
		} else {
			$('#speaktext').text('请稍候...');
			$('#click_speak').removeClass('anime1');
		}
	} catch(e) {
		console.log(e.message);
	}
});

//SoundDetectionEvent连续模式
contosoChatHubProxy_DialogueHub.on("SoundDetectionEvent", function(t) {
	if(t === true) {
		$('#speaktext').text('正在识别')
	} else {
		Hidebyaftertap();
		$('#click_speak').removeClass('anime1');
	}
});

contosoChatHubProxy.on("RequestEvent", function(t) {
	if(t === true) {
		$('#speaktext').text('正在查询中...')
	} else {
		Hidebyaftertap();
		$('#click_speak').removeClass('anime1');
	}
});

var que = '';
var ans = '';
var status = '';
//获得问题
contosoChatHubProxy_DialogueHub.on("InsetQuestion", function(q) {
	que = q;
	$('.Q').text(q);
	$('.A').empty();
});

//获得答案
contosoChatHubProxy_DialogueHub.on("InsertAnswer", function(a) {
	ans = a;
	if(a.UrlType == 100) {
		if(a.Text.length > 30) {
			if($('#iframe').css("display") == "none") {
				io.Jump('http://127.0.0.1:15689/Dal/Dialogue/Index.html', 'Dialogue');
			}
		} else {
			if(a.SimilarProblems != null) {
				if(a.SimilarProblems.length > 0) {
					if($('#iframe').css("display") == "none") {
						io.Jump('http://127.0.0.1:15689/Dal/Dialogue/Index.html', 'Dialogue');
					}
				} else {
					$('.A').text(a.Text);
				}
			} else {
				$('.A').text(a.Text);
			}
		}
	} else {
		if($('#iframe').css("display") == "none") {
			io.Jump('http://127.0.0.1:15689/Dal/Dialogue/Index.html', 'Dialogue');
		}
	}
});

//打开转接弹框
contosoChatHubProxy_DialogueHub.on("TransferCallEvet", function() {
	$('.citybg,.font-page2').css('display', 'block');
	setTimeout("$('.citybg,.font-page2').css('display','none');", 15000)
});

//页面跳转
contosoChatHubProxy.on("JumpEvent", function(pageEvent) {
	$('#iframe').attr('src', pageEvent.Url);
	$('#iframe').css('display', 'block');
	setting.killrobotinfo();
});

//返回首页事件
contosoChatHubProxy.on("TimeOut", function(pageEvent) {
	BackHome();
});

//电量过低
contosoChatHubProxy.on("LowPowerEvent", function() {
	$("#Z-background").css("display", "block");
});

//权限
contosoChatHubProxy.on("NoPermissions", function(pageEvent) {
	$("#N-background").css("display", "block");
	clearTimeout('NoPermissions()');
	setTimeout('NoPermissions()', 5000);
});

function NoPermissions() {
	$("#N-background").css("display", "none");
	is_time == false;
};

contosoChatHubProxy.on("PermissionsLockEvent", function(data) {
	if(data == true) {
		$(".no-touch").css("display", "block");
	} else {
		$(".no-touch").css("display", "none");
	}
});

//屏保
contosoChatHubProxy.on('ScreenWakeUp',
	function(data) {
		clearTimeout(a);
		$('#pingbao').css('display', 'none');
		a = setTimeout("ScreenSleep()", 900000);
	});

function ScreenSleep() {
	$('#pingbao').css('display', 'block');
	contosoChatHubProxy.invoke('ScreenSleep');
}

var a = setTimeout("ScreenSleep()", 900000);
$("body").click(function() {
	clearTimeout(a);
	$('#pingbao').css('display', 'none')
	a = setTimeout("$('#pingbao').css('display','block');", 900000);
})

//滑动事件
$("#content_1").on("swipeleft", function() {
	$('#content_1').data('optiscroll').scrollTo('right', false, 10000);
});

$("#content_1").on("swiperight", function() {
	$('#content_1').data('optiscroll').scrollTo('left', false, 400);
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

//向右滑动
function mouseslide() {
	try {
		var y
		$(".z-index-def").on("touchstart", function(e) {
			startX = e.originalEvent.changedTouches[0].pageX,
				startY = e.originalEvent.changedTouches[0].pageY;
		});
		$(".z-index-def").on("touchend", function(e) {
			moveEndX = e.originalEvent.changedTouches[0].pageX,
				moveEndY = e.originalEvent.changedTouches[0].pageY,
				x = moveEndX - startX,
				y = moveEndY - startY;
			if(x > 150) {
				changepage(true);
			} else if(x < -150) {
				changepage(false);
			}
		});
	} catch(e) {
		console.log("滑动列表出错。" + e.message)
	}
}

function togglewifilist() {
	if($('.wifi,.wifiarea').css('display') == 'none') {
		$('.wifi,.wifiarea').animate({
			height: 'show'
		});
	} else {
		$('.wifi,.wifiarea').animate({
			height: 'hide'
		});
	}
}

function changetextpw() {
	if($('.pas').attr('type') == 'text') {
		$('.pas').attr('type', 'password');
	} else {
		$('.pas').attr('type', 'text');
	}
}

function GetWifiList() {
	contosoChatHubProxy_wifi.invoke('GetWifiList');
}

//权限开
contosoChatHubProxy.on("PermissionValidationViewOpen", function(data) {
	$('#N-background').css('display', 'block')
	$('.Ntitle').html('<p>请扫描二维码验证身份。</p>');
	$('.Nbutton').html('<button class="btn btn-info btn_mine" onclick="contosoChatHubProxy.invoke(\'StopValidation\');">取消验证</button>    <button class="btn btn-dark btn_mine" onclick="gopass()">使用密码</button>');
});
//权限关
contosoChatHubProxy.on("PermissionValidationViewClose", function(data) {
	$('#N-background').css('display', 'none');
	$('.Ntitle').html('');
	$('.Nbutton').html('');
});
//没有权限
contosoChatHubProxy.on("NoPermissionEvent", function(data) {
	tip('没有权限');
});
//验证错误
contosoChatHubProxy.on("PermissionValidationError", function(data) {
	tip('验证错误');
});
//验证成功
contosoChatHubProxy.on("PermissionValidationSuccess", function(data) {
	$('.Ntitle').html('');
	$('.Nbutton').html('');
	$('.Ntitle').html('<p>验证成功</p>');
	setTimeout('$(".Ntitle").html("正在跳转请稍等...")', 500);
});
//验证超时
contosoChatHubProxy.on("PermissionValidationTimeout", function(data) {
	tip('验证超时');
});

//弹出人工解除
contosoChatHubProxy.on("BouncedMalfunction", function(data) {
	$('#B-background').css('display', 'block')
});

//隐藏文本
contosoChatHubProxy_DialogueHub.on("HideViewText", function(b) {
	HideViewText = b;
	if(HideViewText) {
		$('#speaktext').html('请继续对话')
	} else {
		$('#speaktext').html('请点击继续对话')
	}
});

MessageViewHub.on("PushMsgNumEvent", function(e) {
	if(e == 0) {
		$('.Robotinfo-num').css('display', 'none')
	} else {
		$('.Robotinfo-num').css('display', 'block')
		$('.Robotinfo-num').text(e);
	}
});

MessageViewHub.on("PushFlicker", function() {
	$("#info-inco").attr("src", "rec/img/info2.png")
	setTimeout('$("#info-inco").attr("src","rec/img/info.png")', 300)
	setTimeout('$("#info-inco").attr("src","rec/img/info2.png")', 600)
	setTimeout('$("#info-inco").attr("src","rec/img/info.png")', 900)
	setTimeout('$("#info-inco").attr("src","rec/img/info2.png")', 1200)
	setTimeout('$("#info-inco").attr("src","rec/img/info.png")', 1500)
});

function tip(data) {
	$(".tip").css("display", "block");
	$(".tip").html(data)
	setTimeout('$(".tip").css("display","none")', 1500);
}

function gopass() {
	$('#N-background').css('display', 'block')
	$('.Ntitle').html('权限密码：<input class="np" type="text"/>');
	$('.Nbutton').html('<button class="btn btn-info btn_mine" onclick="contosoChatHubProxy.invoke(\'StopValidation\');">取消验证</button>    <button class="btn btn-primary btn_mine" onclick="postpass()">验证密码</button>');
}

function postpass() {
	var p = $('.np').val();
	contosoChatHubProxy.invoke('PermissionValidationPassword', p);
}

//控制缩放
document.ontouchstart = function(e) {
	if(e.touches.length > 1) {
		e.preventDefault();
		e.stopPropagation();
	}
};
document.ontouchmove = function(e) {
	if(e.touches.length > 1) {
		e.preventDefault();
		e.stopPropagation();
	}
};
document.ontouchend = function(e) {
	if(e.touches.length > 1) {
		e.preventDefault();
		e.stopPropagation();
	}
};
document.addEventListener('keydown', function(e) {
	if(e.ctrlKey) {
		e.preventDefault();
	}
})
document.addEventListener("mousewheel", function(e) {
	e.preventDefault(); 
}, false);

function sliceArr(array, size) {
	menulistarr = [];
	var start;
	var end;
	for(var x = 0; x < Math.ceil(array.length / size); x++) {
		start = x * size;
		end = start + size;
		menulistarr.push(array.slice(start, end));
	}
	maxpage = Math.ceil(array.length / size) - 1;
}

function pushlist() {
	$('.list_node_x').removeAttr('onclick')
	$('.list_node_x').empty();
	if(menulistarr[page].length > 0) {
		for(var i = 0; i < menulistarr[page].length; i++) {
			if(menulistarr[page][i].Name == "Dialogue") {
				str = '<img src="' + menulistarr[page][i].Logo + '" width="150px"><p>' + menulistarr[page][i].Desc + '</p>'
				$('#node' + (i + 1)).html(str).attr('onclick', 'io.Jump(\'' + menulistarr[page][i].Url + '\',\'' + menulistarr[page][i].Name + '\')');
			} else {
				str = '<img src="' + menulistarr[page][i].Logo + '" width="150px"><p>' + menulistarr[page][i].Desc + '</p>'
				$('#node' + (i + 1)).html(str).attr('onclick', 'io.Jump(\'' + menulistarr[page][i].Url + '\',\'' + menulistarr[page][i].Name + '\')');
			}
		}
	} else {
		page--;
		pushlist();
	}

}

function changepage(status) {
	if(status === false) {
		if(page >= maxpage) {
			console.log(page + ':' + maxpage);
		} else {
			page++;
			pushlist();
		}
	} else {
		if(page <= 0) {
			console.log(page + ':' + maxpage);
		} else {
			page--;
			pushlist();
		}
	}
}

function Hidebyaftertap() {
	if(HideViewText) {
		$('#speaktext').html('请继续对话')
	} else {
		$('#speaktext').html('请点击继续对话')
	}
}