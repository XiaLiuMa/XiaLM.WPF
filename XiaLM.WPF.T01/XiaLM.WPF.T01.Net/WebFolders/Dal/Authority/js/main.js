var Authority = {};
var areaheight = 0;
var languagestatus = 'CN_EN';
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('AuthorityHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	isRunning = true;
	Authority.GetViewAuthorities();
	parent.onload('Authority');
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

var Aobj;
Authority = {
	GetViewAuthorities: function() {
		contosoChatHubProxy.invoke('GetViewAuthorities').done(function(s) {
			Aobj = s;
			$('.wlist').empty();
			for(var i = 0; i < s.length; i++) {
				var str = '<div class="wunit" id="' + i + '">';
				str += '<div class="left l1">' + s[i].DescName + '</div>';
				if(s[i].Enable) {
					str += '<div class="left l2"><div class="seed2 left">否</div><div class="slideTwo left"><input type="checkbox" class="scheckbox" id="l' + i + '" onchange="Authority.postobj($(this).parent().parent().parent().attr(\'id\'),true)"/><label for="l' + i + '"></label></div><div class="seed left">是</div></div>';
				} else {
					str += '<div class="left l2"><div class="seed2 left">否</div><div class="slideTwo left"><input type="checkbox" class="scheckbox" id="l' + i + '" checked="checked" onchange="Authority.postobj($(this).parent().parent().parent().attr(\'id\'),true)"/><label for="l' + i + '"></label></div><div class="seed left">是</div></div>';
				}
				if(s[i].IsHide) {
					str += '<div class="left l3"><div class="seed2 left">隐藏</div><div class="slideTwo left"><input type="checkbox" class="hcheckbox" id="b' + i + '" onchange="Authority.postobj($(this).parent().parent().parent().attr(\'id\'),false)"/><label for="b' + i + '"></label></div><div class="seed left">显示</div></div>';
				} else {
					str += '<div class="left l3"><div class="seed2 left">隐藏</div><div class="slideTwo left"><input type="checkbox" class="hcheckbox" id="b' + i + '" checked="checked" onchange="Authority.postobj($(this).parent().parent().parent().attr(\'id\'),false)"/><label for="b' + i + '"></label></div><div class="seed left">显示</div></div>';
				}
				$('.wlist').append(str);
			}

			$('.list_node_x', window.parent.document).empty();
			parent.io.GetMenuList();
			$('.forbid').css('display', 'none')
		})
	},
	postobj: function(i, s) {
		var nowobj = Aobj[i];
		$('.forbid').css('display', 'block')
		console.log(nowobj.DescName)
		if(s === true) {
			if(nowobj.Enable) {
				nowobj.Enable = false;
			} else {
				nowobj.Enable = true;
			}
		} else {
			if(nowobj.IsHide) {
				nowobj.IsHide = false;
			} else {
				nowobj.IsHide = true;
			}
		}
		var buildarr = [];
		buildarr.push(nowobj)
		contosoChatHubProxy.invoke('SetViewAuthoritie', buildarr).done(function(s) {
			Authority.GetViewAuthorities();
		})
	}
};

//contosoChatHubProxy.on("PushConnectMessage", function(data) {
//	$(".tip").css("display", "block");
//	$(".tip").html(data)
//	setTimeout('$(".tip").css("display","none")', 3000);
//});

contosoChatHubProxy.on("PushConnectState", function(data) {
	parent.page = 0;
	parent.BackHome();
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

function changemode(bo) {
	var pushall = [];
	if(bo) {
		$('.forbid').css('display', 'block')
		$('.changemode').removeClass('activebtn');
		$('.all').addClass('activebtn');
		for(var i = 0; i < Aobj.length; i++) {
			Aobj[i].IsHide = false;
			pushall.push(Aobj[i]);
		}
		contosoChatHubProxy.invoke('SetViewAuthoritie', pushall).done(function(s) {
			Authority.GetViewAuthorities();
		})
	} else {
		$('.forbid').css('display', 'block')
		$('.changemode').removeClass('activebtn');
		$('.some').addClass('activebtn');
		for(var i = 0; i < Aobj.length; i++) {
			Aobj[i].IsHide = true;
			pushall.push(Aobj[i]);
		}
		contosoChatHubProxy.invoke('SetViewAuthoritie', pushall).done(function(s) {
			pushall = [];
			for(var i = 0; i < Aobj.length; i++) {
				if(Aobj[i].DescName == '翻译' || Aobj[i].DescName == '地图服务' || Aobj[i].DescName == '宣传互动' || Aobj[i].DescName == '咨询服务') {
					Aobj[i].IsHide = false;
					pushall.push(Aobj[i]);
				}
			}
			contosoChatHubProxy.invoke('SetViewAuthoritie', pushall).done(function(s) {
				Authority.GetViewAuthorities();
			})
		})
	}
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