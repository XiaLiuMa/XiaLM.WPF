var Translate = {}
var areaheight = 0;
var languagestatus = 'CN_EN';
var isRunning = false;

var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('NetworkHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	wifi.GetWifiList();
	isRunning = true;
	parent.onload('Network');
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

var wifilist = [];
var wifiobj;
var activewifi;
wifi = {
	GetWifiList: function() {
		contosoChatHubProxy.invoke('GetWifiList');
		$('.wpassBG').css('display', 'none');
		$('.wpass').css('display', 'none');
		$('.wpass2').css('display', 'none');
	},
	ConnectToSSID: function(p) {
		contosoChatHubProxy.invoke('ConnectToSSID', wifiobj, p).done(function(s) {
			$('.wpassBG').css('display', 'none');
			$('.wpass').css('display', 'none');
		})
	},
	Quality: function(e) {
		var qstr = '';
		if(e < 40 && e > 0) {
			qstr = '<img src="images/w1.png">'
			return qstr;
		} else if(e >= 40 && e < 70) {
			qstr = '<img src="images/w2.png">'
			return qstr;
		} else if(e >= 70 && e <= 100) {
			qstr = '<img src="images/w3.png">'
			return qstr;
		} else {
			console.log('信号强度强大于100或小于0或未获取');
			return ' ';
		}
	},
	thisobj: function(id) {
		for(var i = 0; i < wifilist.length; i++) {
			if(id === wifilist[i].ssid) {
				wifiobj = wifilist[i];
			}
		}
		try {
			if(wifiobj.isNoKey === true) {
				wifi.ConnectToSSID('');
			} else if(wifiobj.profileName.length > 0) {
				$('.wpassBG').css('display', 'block');
				$('.wpass2').css('display', 'block');
			} else {
				$('.wpassBG').css('display', 'block');
				$('.wpass').css('display', 'block');
				$('.pas').val('');
			}
		} catch(e) {
			console.log(e.message);
		}
	},
	getpass: function() {
		wifi.ConnectToSSID($('.pas').val());
	},
	removewifi: function() {
		contosoChatHubProxy.invoke('RemoveNetwork', wifiobj).done(function(s) {
			$('.wpassBG').css('display', 'none');
			$('.wpass2').css('display', 'none');
		})
	},
	SetDefaultWifi: function() {
		contosoChatHubProxy.invoke('SetDefaultWifi', activewifi).done(function(s) {
			console.log(1)
		})
	}
};

contosoChatHubProxy.on("PushWifiList", function(s) {
	wifilist = s;
	var str = '';
	var qimg = '';
	$('.wlist').empty();
	if(s.length === 0) {
		$('.wlist').html('当前没有搜索到WIFI信号')
	} else {
		for(var i = 0; i < s.length; i++) {
			if(s[i].isConnected == true) {
				activewifi=s[i];
				$('.wnow').html('<div style="overflow:hidden" onclick="$(\'.nowip\').toggle()">'+s[i].ssid+'<span class="nowip">('+s[i].localIp+')</span></div>');
				if(s[i].isDefaultWifi){
					$('.wloc').html('<img src="images/fa.png" style="margin-right:55px" onclick="wifi.SetDefaultWifi()">'+wifi.Quality(s[i].signalQuality));
				}else{
					$('.wloc').html('<img src="images/nfa.png" style="margin-right:55px" onclick="wifi.SetDefaultWifi()">'+wifi.Quality(s[i].signalQuality));
				}
				$('.wifi_tip').html(wifi.Quality(s[i].signalQuality));
			} else {
				if(s[i].canConnect === true) {
					str = '<div class="wunit" onclick="wifi.thisobj(\'' + s[i].ssid + '\')">';
					str += '<div class="unittit left">' + s[i].ssid + '</div>';
					if(s[i].isNoKey === false) {
						str += '<div class="unitlock left"><img src="images/wl.png"></div>';
					} else {
						str += '<div class="unitlock left"><img src="images/uwl.png"></div>';
					}
					str += '<div class="unitlock left">'
					str += wifi.Quality(s[i].signalQuality);
					str += '</div></div>'
					$('.wlist').append(str);
				}
			}
		}
	}
});

contosoChatHubProxy.on("PushConnectMessage", function(data) {
	$(".tip").css("display", "block");
	$(".tip").html(data)
	setTimeout('$(".tip").css("display","none")', 3000);
});

contosoChatHubProxy.on("PushConnectState", function(data) {
	wifi.GetWifiList();
});

contosoChatHubProxy.on("PushRemoveSuccess", function(data) {
	$('.wpassBG').css('display', 'none');
	$('.wpass').css('display', 'none');
	$('.wpass2').css('display', 'none');
	setTimeout('wifi.GetWifiList();', 3000);
});
//控制缩放
document.ontouchstart = function(e) {
	if(e.touches.length > 1) {
		e.preventDefault();
	}
};
document.ontouchmove = function(e) {
	if(e.touches.length > 1) {
		e.preventDefault();
	}
};
document.ontouchend = function(e) {
	if(e.touches.length > 1) {
		e.preventDefault();
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

function changetextpw() {
	if($('.pas').attr('type') == 'text') {
		$('.pas').attr('type', 'password');
	} else {
		$('.pas').attr('type', 'text');
	}
}

function colsepw() {
	$('.wpassBG').css('display', 'none');
	$('.wpass').css('display', 'none');
	$('.wpass2').css('display', 'none');
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