var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('Publicity2Hub'); //创建连接器
var contosoChatHubProxyv1 = connection.createHubProxy('PublicityHub'); //创建连接器
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
var MediaVolume = 0;
var deleteopen = false;

$(function() {
	connection.start().done(function() {
		//连接成功
		isRunning = true;
		get.GetAllPlayerItems();
		get.GetMediaVolume();
		get.GetPlayerItems();
		//得到配置信息

		parent.onload('Publicity2');
	});
})

Vue.filter('codeFormat', function(name) {
	return name;
});
var v = new Vue({
	el: '#app',
	data: {
		local_list: [],
		all_list: [],
		plan_list: [],
		show_dl: false,
		show_list: true,
		active: true,
	},
	methods: {
		play: function(u) {
			now_obj = u;
			if(u.FileType == 0) {
				$("#media").attr("src", u.Url);
			} else if(u.FileType == 1 || u.FileType == 2) {
				$("#audio").attr("src", u.Url);
			} else {
				tip('文件格式错误：' + u.FileType + 'v')
			}
			//$("#media").attr("autoplay", "true");
			playflie();
			get.SetPlayState();
		},
		deleteitem: function(item) {
			contosoChatHubProxy.invoke('Delete', item)
		},
	}
})
setting = {
	MediaVolumeUp: function() {
		contosoChatHubProxy.invoke('MediaVolumeUp');
	},
	MediaVolumeDown: function() {
		contosoChatHubProxy.invoke('MediaVolumeDown');
	}
}

get = {
	//本地列表
	GetPlayerItems: function() {
		hiddendeleteinco();
		contosoChatHubProxy.invoke('GetPlayerItems').done(function(data) {
			$('.videoarea_black').css('display', 'none');
			v.local_list = [];
			if(data.length > 0) {
				for(var i = 0; i < data.length; i++) {
					v.local_list.push(data[i]);
				}
				GetLocalFileList(data);
			} else {
				$('.videoarea_black').css('display', 'block');
				document.getElementById('media').pause();
			}
		});
	},
	//服务器列表
	GetAllPlayerItems: function() {
		contosoChatHubProxy.invoke('GetAllPlayerItems').done(function(data) {
			v.all_list = data;
		});
	},
	//计划列表
	GetPlayPlanList: function() {
		hiddendeleteinco();
		contosoChatHubProxy.invoke('GetPlayPlanList').done(function(data) {
			v.plan_list = data;
			var str = '';
			for(var i = 0; i < data.length; i++) {
				str += '<ul class="list_dl_ul">'
				str += '<div class="relative planmenu">' +
					'<div class="menu_left" onclick="get.PlayPlan(\'' + data[i].Guid + '\')" >' + data[i].Name + '</div>' +
					'<div class="menu_right" onclick="get.DeletePlan(\'' + data[i].Guid + '\')"><img src="images/delunit.png"></div></div>'
				for(var j = 0; j < data[i].PlayerItems.length; j++) {
					str += '<li>' + data[i].PlayerItems[j].Name + '</li>'
				}
				str += '</ul>'
			}
			$('.list_dl_list').html(str);
			//			<ul class="list_dl_ul" v-for="(plangroup,pindex) in plan_list">
			//				<div class="relative planmenu">
			//					<div class="menu_left" onclick="get.PlayPlan(plangroup.Guid)" >{{plangroup.Name}}</div>
			//					<div class="menu_right" onclick="get.DeletePlan(plangroup.Guid)">删除</div>
			//				</div>
			//				<li v-for="(planitem,itemindex) in plan_list.PlayerItems">{{planitem.Name}}</li>
			//			</ul>				

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
		console.log(1)
		var e = now_obj;
		if(state === 3) {
			e.PlayState = 0;
			$('.startplay').css('display', 'none')
			$('.stopplay').css('display', 'none')
		} else if(state === 2) {
			e.PlayState = 1;
			$('.startplay').css('display', 'block')
			$('.stopplay').css('display', 'none')
			$('.audioui_txt').text(now_obj.Name + '已暂停')
		} else if(state === 1) {
			e.PlayState = 2;
			$('.stopplay').css('display', 'block')
			$('.startplay').css('display', 'none')
			$('.audioui_txt').text(now_obj.Name + '播放中')
//			for(var i = 0; i < v.local_list.length; i++) {
//				v.local_list[i].PlayState = 0;
//				if(v.local_list[i].Url === e.Url) {
//					v.local_list[i].PlayState = 2;
//				}
//			}
		} else {
			console.log('当前播放状态错误')
		}
		console.log("当前状态" + e.PlayState + "名称" + e.Name);
		for(var i = 0; i < v.local_list.length; i++) {
			if(v.local_list[i].Url === e.Url) {
				index_num = i;
			}
		}
		contosoChatHubProxy.invoke('SetPlayState', e)
	},
	//文件导入选择
	RetrieveDiskFiles: function(Path, IsFile) {
		console.log('1')
		$('.pushin').css('display', 'block')
		$(".file-list").empty();
		if(IsFile == false) {
			$("#lu").html(Path);
			contosoChatHubProxyv1.invoke('RetrieveDiskFiles', Path, ["*.mp4", "*.mp3", "*.wav", "*.txt"]).done(function(data) {
				var str;
				var Name;
				var Type;
				var Path1;
				str = '';
				if(data != "") {
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
					} else {
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
			get.ImportFile(Path);
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
	ImportFile: function(Path) {
		//$(".uploadinfo").html("正在上传文件，您可以继续选择文件上传。");
		contosoChatHubProxy.invoke('ImportFile', Path).done(function(bo) {
			//$(".uploadinfo").html("");
			if(bo) {
				tip('文件导入成功')
			} else {
				tip('文件导入失败')
			}
		});
	},
	//获得当前计划名
	GetPlanName: function() {
		contosoChatHubProxy.invoke('GetPlanName').done(function(data) {

		});
	},
	//播放计划
	PlayPlan: function(e) {
		contosoChatHubProxy.invoke('PlayPlan', e).done(function(bo) {
			if(bo) {
				tip('计划设置成功');
			} else {
				tip('计划设置失败');
			}
			$('.smallwindowsty').css('display', 'none')
		});
	},
	//添加计划
	AddPlan: function() {
		var addlist = addplanlist();
		var addobj = {
			PlayFileGuids: addlist,
			Name: $('.planlistname').val(),
			IsSelected: false
		}
		contosoChatHubProxy.invoke('AddPlan', addobj).done(function(bo) {
			if(bo) {
				tip('计划添加成功');
			} else {
				tip('计划添加失败');
			}
			get.GetPlayPlanList();
			$('.smallwindowsty').css('display', 'none')
		});
	},
	//删除计划
	DeletePlan: function(item) {
		contosoChatHubProxy.invoke('DeletePlan', item).done(function(bo) {
			if(bo) {
				tip('删除计划成功')
			} else {
				tip('删除计划失败')
			}
			get.GetPlayPlanList();
		});
	},
	//保存添加文本
	SaveImportText: function() {
		var saveobj = {
			Language: $('.languageselecter').val(), //语言 Mandarin 普通话 English 英语 Cantonese粤语
			Name: $('.textname').val(),
			Text: $('.textinfo').val()
		}
		contosoChatHubProxy.invoke('SaveImportText', saveobj).done(function(bo) {
			if(bo) {
				tip('保存文本成功');
			} else {
				tip('保存文本失败');
			}
			$('.textwindowsty').css('display', 'none');
		});
	},
	//打开添加计划列表
	openaddlist: function() {
		try {
			get.GetAllPlayerItems();
			$('.addlist').css('display', 'block');
		} catch(e) {
			tip('无法获取计划列表')
		}
	},
	GetMediaVolume: function() {
		try {
			contosoChatHubProxy.invoke('GetMediaVolume').done(function(Volume) {
				MediaVolume = Volume / 100;
				document.getElementById('media').volume = MediaVolume;
				document.getElementById('audio').volume = MediaVolume;
			});
		} catch(e) {
			tip('无法获取音量')
		}
	},
	Upload: function(obj) {
		try {
			contosoChatHubProxy.invoke('Upload', obj).done(function(bo) {
				if(bo) {
					tip('正在连接请稍候..')
					tip('开始播放视频');
					v.play(obj)
				} else {
					tip('播放视频失败：upload-fail')
				}
			});
		} catch(e) {
			tip('无法获取音量')
		}
	},
	RestUpdata: function(bo) {
		contosoChatHubProxy.invoke('RestUpdata', bo)
		$('.fileexitswindowsty').css('display', 'none');
	}
}

video = {
	//切换列表
	Changelist: function(e) {
		if(e) {
			$('#list_dl').css('display', 'none');
		} else {
			$('#list_dl').css('display', 'block')
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

//设置播放状态
contosoChatHubProxy.on('PlayEvent', function(o) {
	if(o.Guid === now_obj.Guid) {
		if(!media1) {
			document.getElementById('media').attr("src", o.Url);
		}
		if(o.PlayState == 1) {
			stopplay();
			now_obj.PlayState = 1;
			state = 2;
		} else if(o.PlayState == 2) {
			startplay();
			now_obj.PlayState = 2;
			state = 1;
		} else if(o.PlayState == 0) {
			if(o.FileType == 0) {
				document.getElementById('media').currentTime = 0;
				document.getElementById('media').pause();
			} else {
				document.getElementById('audio').currentTime = 0;
				document.getElementById('audio').pause();
			}
			now_obj.PlayState = 0;
			state = 3;
		} else {
			console.log('o.PlayState未知')
		}
		get.SetPlayState();
	} else {
		now_obj = o;
		if(o.FileType == 0){
			$("#media").attr("src", o.Url);
		}else{
			$("#audio").attr("src", o.Url);
		}
		
		if(now_obj.PlayState == 3) {
			get.Upload(now_obj)
		} else {
			startplay();
		}
		now_obj.PlayState = 2;
		state = 1;
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
//刷新列表
contosoChatHubProxy.on('FlushEvent', function(bo) {
	if(bo) {
		firsttime = true;
	}
	get.GetPlayerItems();
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
//内存不足事件
contosoChatHubProxy.on('HardDiskQuotaEvent', function() {
	tip('内存不足')
});
//出现导入文本框选择器
contosoChatHubProxy.on('ImportTextDialogueEvent', function(name, info) {
	$('.textwindowsty').css('display', 'block');
	$('.textname').val(name);
	$('.textinfo').val(info);
	$('.pushin').css('display', 'none');
});

//文件已存在对话框
contosoChatHubProxy.on('FileExitsDilaugeEvent', function(name) {
	$('.fileexitswindowsty').css('display', 'block');
	$('.refilename').text(name);
});

//静音
contosoChatHubProxyv1.on("EilenceEvent", function(b) {
	if(b) {
		media1.volume = 0;
		audio1.volume = 0;
	} else {
		media1.volume = 0.5;
		audio1.volume = 0.5;
	}
});

//回馈音量
contosoChatHubProxy.on("FeedbackVolumeEvent", function(vo) {
	clearTimeout(i)
	$('#Volumevalue').css('display', 'block');
	$('#volumenum').html(parseInt(vo));
	MediaVolume = vo / 100;
	document.getElementById('media').volume = MediaVolume;
	document.getElementById('audio').volume = MediaVolume;
	i = setTimeout("$('#Volumevalue').css('display','none')", 3000);
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

//键盘
virtualkeyboard();