var on = false;
var faq = {};
var isRunning = false;
var que_index;
var ans_index;
var hotlist;
var status_index;
var HideViewText = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy_DialogueHub = connection.createHubProxy('DialogueHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	faq.init();
	faq.GetProblemTypes();
	faq.GetHideViewTextState();
	//获取配置信息
	parent.onload('Dialogue');
});

faq = {
	IsFirst: true,
	StarRecord: function() {
		contosoChatHubProxy_DialogueHub.invoke('StarRecord');

	},
	init: function() {
		try {
			que_index = parent.que;
			ans_index = parent.ans;
			status_index = parent.status;
			$('.font-pageBG,.font-page').css('display', 'none');
			$('.wenda').empty();
			$('.info_title').empty();
			$('.xiangsi').empty();
			$('.info_title').append(que_index);
			$('.wenda').append('<br /><br />');
			//处理当前问题
			if(ans_index.UrlType == 100) {
				$('.wenda').append(ans_index.Text);
			} else if(ans_index.UrlType == 101) {
				window.location.href = ans_index.Url;
			} else if(ans_index.UrlType == 102) {
				str = '<img src="' + ans_index.Url + '" style="max-height: 600px;max-width: 800px;" />'
				showb();
				$('.font-page').append(str);
			} else if(ans_index.UrlType == 103) {
				str = '<audio src="' + ans_index.Url + '" autoplay="autoplay"></audio>'
				showb();
				$('.font-page').append(str);
			} else if(ans_index.UrlType == 105) {
				showb();
				$('.font-page').append(ans_index.PlayText);
				$('.font-page').append('<br /><br />');
				var fjson = JSON.parse(ans_index.Text);
				for(var i = 0; i < fjson.length; i++) {
					str = '<div class="hangban"><div class="hangban_in">'
					str += '<div class="hleft">'
					str += '<div class="l1"><div class="l11">' + fjson[i].DepartTime + '</div><div class="l12"><img src="images/left.png" width="150px"></div><div class="l13">' + fjson[i].ArriveTime + '</div></div>'
					str += '<div class="l2"><div class="l21 orange">' + fjson[i].DepartPort + '</div><div class="l22">&nbsp;</div><div class="l23 orange">' + fjson[i].ArrivePort + '</div></div>'
					str += '<div class="l3"><div class="l31">' + fjson[i].Airline + '</div><div class="l33">' + fjson[i].Flight + '</div></div></div>'
					str += '<div class="hright orange">￥' + fjson[i].Price + '</div></div></div>'
					$('.font-page').append(str);
				}
			} else {
				str = '没有找到答案，请重新问我。'
				$('.wenda').append(str);
			}
			//处理相似问题
			var aarr = ans_index.SimilarProblems;
			var b;
			var strr;
			if(aarr.length > 0) {
				$('.area').removeClass('a1big');
				$('.wenda').css('height','180px');
				$('.area2').css('display', 'block');
				for(var i = 0; i < aarr.length; i++) {
					strr = '<div onclick="fontshow(' + i + ')"><p class="qq" style="color:#E8B038">' + parseInt(i + 1) + '.' + aarr[i].Question + '</p>';
					if(aarr[i].Answer.length > 10) {
						b = aarr[i].Answer;
						b = b.slice(0, 7) + "... >>点击查看"
					} else {
						b = aarr[i].Answer;
					}
					strr += '<p class="aa">' + b + '</p></div>';
					$('.xiangsi').append(strr);
				}
			} else {
				$('.area').addClass('a1big');
				$('.wenda').css('height','540px');
				$('.area2').css('display', 'none');
			}
		} catch(e) {
			console.log(e.message)
		}
	},
	GetHotProblems: function(e) {
		$('.btn-change2').attr('onclick', 'faq.GetHotProblems(\"' + e + '\")')
		contosoChatHubProxy_DialogueHub.invoke('GetHotProblems', e).done(function(h) {
			$('.xiangsi2').empty();
			hotlist = h;
			if(h.length > 0) {
				for(var i = 0; i < h.length; i++) {
					if(i%2==0){
						$('.xiangsi2').append('<div onclick="fontshowhot(' + i + ')"><p class="qq">' + parseInt(i + 1) + '.' + h[i].Question + '</p></div>')
					}else{
					    $('.xiangsi2').append('<div onclick="fontshowhot(' + i + ')"><p class="qq" style="color: #e8b038;">' + parseInt(i + 1) + '.' + h[i].Question + '</p></div>')
					}
				}
			}
		})
	},
	GetSimilarProblems: function() {
		contosoChatHubProxy_DialogueHub.invoke('GetSimilarProblems', que_index).done(function(e) {
			$('.xiangsi').empty();
			for(var i = 0; i < e.length; i++) {
				strr = '<div onclick="fontshow(' + i + ')"><p class="qq" style="color:#E8B038">' + parseInt(i + 1) + '.' + e[i].Question + '</p>';
				if(e[i].Answer.length > 10) {
					b = e[i].Answer;
					b = b.slice(0, 7) + "... >>点击查看"
				} else {
					b = e[i].Answer;
				}
				strr += '<p class="aa">' + b + '</p></div>';
				$('.xiangsi').append(strr);
			}
		})
	},
	GetProblemTypes: function() {
		contosoChatHubProxy_DialogueHub.invoke('GetProblemTypes').done(function(e) {
			var str = "";
			$(".hotlist").empty();
			for(var i = 0; i < e.length; i++) {
				if(faq.IsFirst) {
					faq.GetHotProblems(e[0]);
					faq.IsFirst = false;
				}
				
				str = "<button onclick='faq.GetHotProblems(\"" + e[i] + "\")'>" + e[i] + "</button>";
				$(".hotlist").append(str);
			}
		})
	},
	GetHideViewTextState:function() {
		contosoChatHubProxy_DialogueHub.invoke('GetHideViewTextState').done(function(b) {
			HideViewText = b;
			Hidebyaftertap()
		})
	},
	Call:function(){
		contosoChatHubProxy_DialogueHub.invoke('Call');
	}
};

//录音
contosoChatHubProxy_DialogueHub.on("RecoedEvent", function(s) {
	try {
		if(s == 0) {
			$('#speaktext').text('正在识别...')
			$('#speak_img').addClass('anime1');
		} else {
			$('#speaktext').text('请稍候...');
			$('#speak_img').removeClass('anime1');
		}
	} catch(e) {
		console.log(e.message);
	}
});
//获得问题
contosoChatHubProxy_DialogueHub.on("InsetQuestion", function(q) {
	que_index = q;
	$('.info_title').empty();
	$('.info_title').append(q);
	clearInterval(donghua);
});

//获得答案
contosoChatHubProxy_DialogueHub.on("InsertAnswer", function(a) {
	try {
		$('.wenda').empty();
		$('.xiangsi').empty();
		ans_index = a;
		//处理相似问题
		var aarr = ans_index.SimilarProblems;
		var b;
		var strr;
		if(aarr.length > 0) {
			$('.area').removeClass('a1big');
			$('.wenda').css('height','180px');
			$('.area2').css('display', 'block');
			for(var i = 0; i < aarr.length; i++) {
				strr = '<div onclick="fontshow(' + i + ')"><p class="qq">' + parseInt(i + 1) + '.' + aarr[i].Question + '</p>';
				if(aarr[i].Answer.length > 10) {
					b = aarr[i].Answer;
					b = b.slice(0, 7) + "...... >>点击展开"
				} else {
					b = aarr[i].Answer;
				}
				strr += '<p class="aa">' + b + '</p></div>';
				$('.xiangsi').append(strr);
			}
		} else {
			$('.area').addClass('a1big');
			$('.wenda').css('height','540px');
			$('.area2').css('display', 'none');
		}
		//处理当前问题
		if(a.UrlType == 100) {
			$('.wenda').append(a.Text);
		} else if(a.UrlType == 101) {
			window.location.href = a.Url;
		} else if(a.UrlType == 102) {
			str = '<img src="' + a.Url + '" style="max-height: 600px;max-width: 800px;" />'
			$('.wenda').append(str);
		} else if(a.UrlType == 103) {
			str = '<audio src="' + a.Url + '" autoplay="autoplay"></audio>'
			showb();
			$('.font-page').append(str);
		} else if(a.UrlType == 105) {
			showb();
			$('.font-page').append(a.PlayText);
			$('.font-page').append('<br /><br />');
			var fjson = JSON.parse(a.Text);
			for(var i = 0; i < fjson.length; i++) {
				str = '<div class="hangban"><div class="hangban_in">'
				str += '<div class="hleft">'
				str += '<div class="l1"><div class="l11">' + fjson[i].DepartTime + '</div><div class="l12"><img src="images/left.png" width="150px"></div><div class="l13">' + fjson[i].ArriveTime + '</div></div>'
				str += '<div class="l2"><div class="l21 orange">' + fjson[i].DepartPort + '</div><div class="l22">&nbsp;</div><div class="l23 orange">' + fjson[i].ArrivePort + '</div></div>'
				str += '<div class="l3"><div class="l31">' + fjson[i].Airline + '</div><div class="l33">' + fjson[i].Flight + '</div></div></div>'
				str += '<div class="hright orange">￥' + fjson[i].Price + '</div></div></div>'
				$('.font-page').append(str);
			}
		} else {
			str = '没有找到答案，请重新问我。'
			$('.wenda').append(str);
		}
	} catch(e) {
		console.log(e.message)
	}
});

//SoundDetectionEvent连续模式
contosoChatHubProxy_DialogueHub.on("SoundDetectionEvent", function(t) {
	if(t === true) {
		$('#speaktext').text('正在识别')
	} else {
		Hidebyaftertap();
	}
});

contosoChatHubProxy_DialogueHub.on("ClearListEvent", function() {
	$('.wenda').empty();
	$('.xiangsi').empty();
	$('.font-pageBG,.font-page').css('display', 'none');
});

contosoChatHubProxy.on("RequestEvent", function(t) {
	if(t === true) {
		$('#speaktext').text('正在查询中...')
	} else {
		Hidebyaftertap();
		$('#click_speak').removeClass('anime1');
	}
});

//隐藏文本
contosoChatHubProxy_DialogueHub.on("HideViewText", function(b) {
	HideViewText = b;
	Hidebyaftertap();
});


function HideViewTextinit() {
	HideViewText = parent.HideViewText;
	Hidebyaftertap();
}
function Hidebyaftertap(){
	if(HideViewText) {
		$('#speaktext').html('请继续对话')
	} else {
		$('#speaktext').html('请点击继续对话')
	}
}

function donghua() {
	$('#click_speak').animate({
		"opacity": 1
	}, 500, function() {
		$('#click_speak').animate({
			"opacity": 0.5
		}, 500);
	});
}

function fontshow(e) {
	try {
		$('.font-pageBG,.font-page').css('display', 'block');
		$('.font-page').empty();
		var Sim = ans_index.SimilarProblems[e];
		$('.font-page').append('<p>' + Sim.Question + '</p>')
		$('.font-page').append('<p style="color:coral">' + Sim.Answer + '</p>')
	} catch(e) {
		console.log(e.message)
	}
}

function fontshowhot(e) {
	try {
		$('.font-pageBG,.font-page').css('display', 'block');
		$('.font-page').empty();
		$('.font-page').append('<p>' + hotlist[e].Question + '</p>')
		$('.font-page').append('<p style="color:coral">' + hotlist[e].Answer + '</p>')
	} catch(e) {
		console.log(e.message)
	}
}

function showb() {
	$('.font-pageBG,.font-page').css('display', 'block');
	$('.font-page').empty();
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