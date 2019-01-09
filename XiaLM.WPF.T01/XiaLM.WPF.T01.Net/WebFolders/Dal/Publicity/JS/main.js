var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('PublicityHub'); //创建连接器
var contosoChatHubProxy_setting = connection.createHubProxy('SettingHub'); //创建连接器
var get;
var video;
var del_list_id, del_list_type, del_list_name;
var first_file;

var lastpath;
var list;
var list_Server;
var i; //音量计时器
var show; //分所计时器
var now_obj;
var cc; //判断下载的首个文件
var ctrl_type = false; //f为上传，t为下载
var Permissions_Lock = false; //权限锁

$(function() {
	connection.start().done(function() {
		//连接成功
		isRunning = true;
		//得到配置信息
		get.GetVideInfos();
		get.GetLocalVideoList();
		get.GetDownloadingList();
		parent.onload('Publicity');
	});
})

Vue.filter('codeFormat', function(name) {
	title=name.split(".");
	return title[0];
});
var v = new Vue({
	el: '#app',
	data: {
		local_list: [],
		server_list: [],
		download_list: [],
		show_dl: false,
		show_list: true,
	},
	methods: {
		play: function(u) {
			$("#media").attr("src", u.Path);
			$("#media").attr("autoplay", "true");
			now_obj = u;
			get.SetPlayState();
		}
	}
	
})
setting = {
	Volumeup: function() {
		contosoChatHubProxy_setting.invoke('SetVolume', true).done(function(s) {
			clearTimeout(i)
			$('#Volumevalue').css('display', 'block');
			$('#volumenum').html(parseInt(s));
			i = setTimeout("$('#Volumevalue').css('display','none')", 3000);
		});
	},
	Volumedown: function() {
		contosoChatHubProxy_setting.invoke('SetVolume', false).done(function(s) {
			clearTimeout(i)
			$('#Volumevalue').css('display', 'block');
			$('#volumenum').html(parseInt(s));
			i = setTimeout("$('#Volumevalue').css('display','none')", 3000);
		});
	}
}

get = {
	//本地列表
	GetLocalVideoList: function() {
		contosoChatHubProxy.invoke('GetLocalVideoList').done(function(data) {
			$('.videoarea_black').css('display', 'none');
			v.local_list = [];
			if(data.length > 0) {
				for(var i = 0; i < data.length; i++) {
					v.local_list.push(data[i]);
				}
				GetLocalFileList(data);
			} else {
				$('.videoarea_black').css('display', 'block');
				media.pause();
			}
		});
	},
	//下载中的列表
	GetDownloadingList: function() {
		contosoChatHubProxy.invoke('GetDownloadingList').done(function(data) {
			v.download_list = data;
		});
	},
	//服务器列表
	GetVideInfos: function() {
		contosoChatHubProxy.invoke('GetVideInfos').done(function(data) {
			v.server_list = data;
		});
	},
	GetInformant: function() {
		contosoChatHubProxy_setting.invoke('GetInformant').done(function(e) {
			if(e == 'en') {
				$('.SF_en').css('display', 'block');
			} else {
				$('.SF_cn').css('display', 'block');
			}
		})
	},
	SetFullState: function(e) {
		contosoChatHubProxy.invoke('SetFullState', e);
	},
	//文件删除
	deletelist: function() {
		var dellist = deletes();
		$('.tip').css('display','block')
		$('.delete,.oklist').css('display', 'none');
		$("#media").attr("src",'');
		contosoChatHubProxy.invoke('Delete', dellist).done(function(e) {
			$('.tip').css('display','none')
 			get.GetLocalVideoList();
		});
	},
	//文件下载
	downloadlist: function() {
		var dllist = downloads();
		contosoChatHubProxy.invoke('Download', dllist).done(function() {
			get.GetDownloadingList();
		});
	},

	//改变音量
	InsertVolumeExcursion: function(e) {
		console.log(e)
		contosoChatHubProxy_setting.invoke('InsertVolumeExcursion', e).done(function() {});
	},
	SetPlayState: function() {
		var e = now_obj;
		var Media = document.getElementById('media');
		if(state === 3) {
			e.PlayState = 1003;
		} else if(state === 2) {
			e.PlayState = 1002;
		} else if(state === 1) {
			e.PlayState = 1001;
		} else {
			console.log('当前播放状态错误')
		}
		console.log("当前状态" + e.PlayState + "名称" + e.Name);
		for(var i = 0; i < v.local_list.length; i++) {
			if(v.local_list[i].Path === e.Path) {
				index_num = i;
			}
		}
		contosoChatHubProxy.invoke('SetPlayState', e)
	},
	//文件导入选择
	RetrieveDiskFiles: function(Path, IsFile) {
		console.log('1')
		$('.pushin').css('display','block')
		$(".file-list").empty();
		if(IsFile == false) {
			$("#lu").html(Path);
			contosoChatHubProxy.invoke('RetrieveDiskFiles', Path, ["*.mp4"]).done(function(data) {
				var str;
				var Name;
				var Type;
				var Path1;
				str = '';
				if(data != ""){
					UpperPath = data[0].UpperPath;
				} else {
					UpperPath = before_path;
				}
				if(Path != "") {
					str += '<div class="file-list-block" onclick="get.RetrieveDiskFiles(\'' + UpperPath + '\',false)">'
					str += '<p class="file-list-name"><img src="images/21.png" width="50px">上一步(' + UpperPath + ')</p>'
					str += '</div>'
					$(".file-list").append(str);
					str = '';
				}
				before_path = Path;
				for(var i = 0; i < data.length; i++) {
					Name = data[i].Name;
					IsFile = data[i].IsFile;
					Path1 = data[i].Path;
					if(data[i].IsFile == false) {
						str += '<div class="file-list-block" onclick="get.RetrieveDiskFiles(\'' + Path1 + '\',' + IsFile + ')">'
						str += '<p class="file-list-name"><img src="images/inf.png" width="50px">' + Name + '</p>'
						str += '</div>'
						$(".file-list").append(str);
						str = '';
					} else{
						str += '<div class="file-list-block" onclick="get.RetrieveDiskFiles(\'' + Path1 + '\',' + IsFile + ')">'
						str += '<p class="file-list-name"><img src="images/22.png" width="50px">' + Name + '</p>'
						str += '</div>'
						$(".file-list").append(str);
						str = '';
					}
				}
			});
		} else if(IsFile == true) {
			str = '';
			get.ImportFiles(Path);
			str += '<div class="file-list-block" onclick="get.RetrieveDiskFiles(\'' + before_path + '\',false)">'
			str += '<p class="file-list-name"><img src="images/21.png" width="50px">继续上传(' + before_path + ')</p>'
			str += '</div>'
			$(".file-list").append(str);
		} else {
			str = '';
			$(".file-list").append("获取文件格式错误,3秒后重新加载。");
			setTimeout("get.RetrieveDiskFiles('',false)", 3000);
			setTimeout('$(".file-list").append("")', 3000);
		}
	},
	//文件导入
	ImportFiles: function(Path) {
		$(".uploadinfo").html("正在上传文件，您可以继续选择文件上传。");
		setTimeout('$(".uploadinfo").html("")', 5000);
		contosoChatHubProxy.invoke('ImportFiles', [Path]).done(function(data) {
             get.GetLocalVideoList();
		});
	}
}

video = {
	//切换列表
	Changelist: function(e) {
		if(e === 0) {
			if(v.local_list.length > 0) {
				$('#list_dl').css('display', 'none');
				click_style(0);
			}
		} else {
			$('#list_dl').css('display', 'block')
			click_style(1);
		}
	},
	fullscreen: function() {
		if($('#videoarea').hasClass('fulls')) {
			video.ex_full();
		} else {
			video.go_full();
		}
	},
	deletevideo: function() {
		if($(".oklist").css("display") == "none") {
			$('.delete,.oklist').css('display', 'block')
		} else {
			$('.delete,.oklist').css('display', 'none')
		}
	},
	go_full: function() {
		$('#iframe', parent.document).css('top', '0px');
		$('#iframe', parent.document).css('height', '1200px');
		$('.exfull').css('display', 'block');
		$('.MS').css('height', '1200px');
		$('.title').css('display', 'none');
		$('#videoarea').addClass('fulls');
		$('#videoarea').removeClass('nofull');
		get.SetFullState(true);
	},
	ex_full: function() {
		$('#iframe', parent.document).css('top', '50px');
		$('#iframe', parent.document).css('height', '1150px');
		$('.exfull').css('display', 'none');
		$('.MS').css('height', '1070px');
		$('.title').css('display', 'block');
		$('#videoarea').removeClass('fulls');
		$('#videoarea').addClass('nofull');
		get.SetFullState(false);
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
//下载中的视频
contosoChatHubProxy.on('Downloading', function(SessionId) {
	$('.' + SessionId).css('border-bottom', '2px solid greenyellow')
});
//下载中的视频百分比

contosoChatHubProxy.on('ProgressBar', function(s, p) {
	$('.' + s).html(parseInt(p) + '%');
});
//下载完成
contosoChatHubProxy.on('Finished', function(s) {
	v.download_list.removeByValue(s);
	console.log('下载完成' + s);
	get.GetLocalVideoList();
});
//下载失败
contosoChatHubProxy.on('Failed', function(s) {
	var failunit;
	for(var i = 0; i < v.download_list.length; i++) {
		if(v.download_list[i].SessionId === s) {
			failunit = v.download_list[i].VideoName;
		}
	}
	console.log('下载失败' + failunit);
	$('.failed').append(failunit + '下载失败');
	v.download_list.removeByValue(s);
});
contosoChatHubProxy.on('StorageFull', function() {
	v.download_list = [];
	$('.failed').empty();
	console.log('存储已满');
	$('.failed').append('内部储存已满，以中断下载。');
});

//指定播放
contosoChatHubProxy.on('PlayStateEvent', function(o) {
	if(o.Session === now_obj.Session) {
		if(o.PlayState == 1002) {
			media.pause();
			now_obj.PlayState = 1002;
			state=2;
		} else if(o.PlayState == 1001) {
			media.play();
			now_obj.PlayState = 1001;
			state=1;
		} else if(o.PlayState == 1003) {
			media.currentTime = 0;
			media.pause();
			now_obj.PlayState = 1003;
			state=3;
		} else {
			console.log('o.PlayState未知')
		}
		get.SetPlayState();
	} else {
		now_obj = o;
		$("#media").attr("src", o.Path);
		media.play();
		now_obj.PlayState = 1001;
		state=1;
		get.SetPlayState();
	}
});

//音量显示
contosoChatHubProxy.on('Volume', function(e) {
	clearTimeout(i)
	$('#Volume_value').css('display', 'block');
	$('#volume_num').html(e);
	i = setTimeout("$('#Volume_value').css('display','none')", 3000);
});
//发送全屏状态
contosoChatHubProxy.on('RequesFullState', function() {
	if($('#videoarea').hasClass('fulls')) {
		get.SetFullState(true)
	} else {
		get.SetFullState(false)
	}
});
contosoChatHubProxy.on('FullScreenEvent', function(s) {
	if(s === false) {
		video.ex_full();
	} else {
		video.go_full();
	}
});
//状态改变
contosoChatHubProxy.on('RefreshFileList', function(s1, s2) {
	//下载开始播放第一个
	if(ctrl_type == true) {
		$("#media").attr("src", list_patharr[0]);
	}
	ctrl_type = false;
	//禁止触发播放
	firsttime = false;
	//接收参数
	get.BusyInfo();
	list = s1;
	list_Server = s2;
	GetLocalFileList(s1);
	GetServerFileList(s2);
});

//单个宣传片循环播放
contosoChatHubProxy.on('CurrentCycle', function(data) {
	loop_change('simple');
});
//多个宣传片循环播放
contosoChatHubProxy.on('AllCycle', function(data) {
	loop_change('complex');
});
//随机循环播放
contosoChatHubProxy.on('RandomPlay', function(data) {
	loop_change('random');
});
//静音
contosoChatHubProxy.on("EilenceEvent", function(b) {
	if(b){
		media.volume=0;
	}else{
		media.volume=0.5;
	}
});

Array.prototype.removeByValue = function(val) {
	for(var i = 0; i < this.length; i++) {
		if(this[i].SessionId == val) {
			this.splice(i, 1);
			break;
		}
	}
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


