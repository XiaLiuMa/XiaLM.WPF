var Translate = {}
var areaheight = 0;
var languagestatus;
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('Translate2Hub'); //创建连接器
var timer = 15;
var settime;
var obj1;
var obj2;

connection.start().done(function() {
	//连接成功
	isRunning = true;
	Translate.GetLanguageDescs();
	parent.onload('Translate2');
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

Translate = {
	StartRecording: function() {
		contosoChatHubProxy.invoke('Start').done(function(list) {
			$(".t1").empty();
			$(".t2").empty();
			$('#speak_img').addClass('anime1');
			$(".forbid").css('display', 'block');

		})
	},
	GetLanguageDescs: function() {
		contosoChatHubProxy.invoke('GetLanguageDescs').done(function(list) {
			languagestatus = list;
			for(var i = 0; i < list.length; i++) {
				if(list[i].ChDesc == '中文') {
					str1 = '<option selected="selected" value="' + list[i].ChDesc + '">' + list[i].ChDesc + '</option>'
					obj1 = list[i];
				} else {
					str1 = '<option value="' + list[i].ChDesc + '">' + list[i].ChDesc + '</option>'
				}
				$('.select1').append(str1)
				if(list[i].ChDesc == '英语') {
					str2 = '<option selected="selected" onclick=Translate.SetLanguage2("' + list[i].ChDesc + '")>' + list[i].ChDesc + '</option>'
					obj2 = list[i];
				} else {
					str2 = '<option onclick=Translate.SetLanguage2("' + list[i].ChDesc + '")>' + list[i].ChDesc + '</option>'
				}
				$('.select2').append(str2)
			}
		})
	},
	SetLanguage1: function(name) {
		for(var i = 0; i < languagestatus.length; i++) {
			if(languagestatus[i].ChDesc == name) {
				obj1 = languagestatus[i];
			}
		}
		Translate.SetLanguage();
	},
	SetLanguage2: function(name) {
		for(var i = 0; i < languagestatus.length; i++) {
			if(languagestatus[i].ChDesc == name) {
				obj2 = languagestatus[i];
			}
		}
		Translate.SetLanguage();
	},
	SetLanguage: function() {
		contosoChatHubProxy.invoke('SetLanguage', obj1, obj2)
	}
}

//开始录音
contosoChatHubProxy.on("RecoedEvent", function(s) {
	if(s == 0) {
		console.log("开始录音"); //开始录音
		$(".before_Translation").empty();
		$(".after_Translation").empty();
		$('#speak_img').addClass('anime1');
	} else {
		$('#speak_img').removeClass('anime1');
	}
});
//注册事件
contosoChatHubProxy.on('InsertTargetText', //接收答案
	function(data) {
		$(".t2").html(data);
		$('.stext').text('点击继续翻译');
		$('.ctip').text('完成翻译')
	});
contosoChatHubProxy.on('InsertSourceTextEvent', //接收提问
	function(data) {
		$(".t1").html(data);
	});
contosoChatHubProxy.on('RecodEnd', //结束录音
	function(data) {
		$('#speak_img').removeClass('anime1');
	});
contosoChatHubProxy.on('SoundDetectionEvent', //声音检测
	function(data) {
		if(data === true) {
			$('.stext').text('点击开始翻译');	
		} else {
			$('.stext').text('正在识别..');
		}
	});
contosoChatHubProxy.on('WaitingEvent', //进入页面的遮罩
	function() {
		$(".forbid").css('display', 'block');
	});
contosoChatHubProxy.on('WaitIsOverEvent', //退出页面的遮罩
	function() {
		$(".forbid").css('display', 'none');
	});

contosoChatHubProxy.on('CountDownEvent', //15秒倒计时
	function() {
		try {
			clearTimeout(settime);
		} catch(e) {}
		timer = 15;
		daojishi();
		$('.ctip').text('正在侦听中...')
	});

contosoChatHubProxy.on('WaitSpeakEvent', //等待开始说话
	function() {
		$('.ctip').text('正在等待语音输入...')
	});

contosoChatHubProxy.on('NoSpeakEvent', //没有说话
	function() {
		$('.stext').text('没有听见您说话');
		$('.ctip').text('没有听见您说话')
		setTimeout('canceldaojishi()',1000)
	});

contosoChatHubProxy.on('CancelCountdownEvent',
	function() {
		try {
			clearTimeout(settime);
		} catch(e) {}
		$('.timernum').text('-');
		$(".forbid").css('display', 'none');
		$('#speak_img').removeClass('anime1');
		$('.stext').text('点击继续翻译');
	});

function daojishi() {
	if(timer > 0) {
		timer--;
		$('.timernum').text(timer)
		settime = setTimeout('daojishi()', 1000);
	} else {
		canceldaojishi();
	}
}

function canceldaojishi() {
	$('.timernum').text('-')
	timer = 15;
	$(".forbid").css('display', 'none');
	$('#speak_img').removeClass('anime1');
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