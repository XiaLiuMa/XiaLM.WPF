var media1 = document.getElementById('media');
var audio1 = document.getElementById('audio');
media1.volume = 0.5;
audio1.volume = 0.5;
var state = 1;
media1.addEventListener("ended", function() {
	console.log("结束");
	state = 3;
	loop();
	get.SetPlayState();
})
media1.addEventListener("pause", function() {
	state = 2;
	console.log("暂停");
	get.SetPlayState();
})
media1.addEventListener("play", function() {
	state = 1;
	console.log("播放");
	get.SetPlayState();
})
audio1.addEventListener("ended", function() {
	console.log("结束");
	state = 3;
	loop();
	get.SetPlayState();
})
audio1.addEventListener("pause", function() {
	state = 2;
	console.log("暂停");
	get.SetPlayState();
})
audio1.addEventListener("play", function() {
	state = 1;
	console.log("播放");
	get.SetPlayState();
})
var IsPlaying;
var FileOnlyFlag;
var CycleMode;
var ChangeVolume;
var IsFullScreen;

var loop_status = 'complex'; //complex多 simple单 random随机
var list_patharr = [];
var list_patharr_SERVER = [];

var index_num = 0;
var fullscreen_status = false;
var list_status = false;

var now_address;
var StatusInfo_package;
var firsttime = true;
var upload_download_count = 0;
var busy = false;
//获得本地列表
function GetLocalFileList(data) {
	if(data.length > 0) {
		for(var i = 0; i < data.length; i++) {
			if(firsttime === true) {
				try {
					if(i == 0) {
						if(data[i].FileType == 0) {
							$("#media").attr("src", data[0].Url);
						} else if(data[i].FileType == 1 || data[i].FileType == 2) {
							$("#audio").attr("src", data[0].Url);
						} else {
							tip('文件格式错误:' + data[0].FileType + '(local)')
						}
						now_obj = data[0];
						//playflie();
						firsttime = false;
					}
				} catch(err) {
					console.log("没有得到有效的路径。" + err)
				}
			}
		}
	} else {
		//video.Changelist(1);
	}
}

function launchFullscreen() {
	fullscreen_status = true;
	$("#videoarea").css("width", "100vw")
	$("#videoarea").css("top", "0")
	$("#videoarea").css("left", "0")
	$("#videoarea").css("height", "100vh")
	$("#launchFullscreen").css("display", "none")
	$("#exitFullscreen").css("display", "block")
	/*var _div = window.parent.document.getElementById('page_02_child_head');
	_div.style.cssText = "display:none";
	var _ifream = window.parent.document.getElementById('iframe');
	_ifream.style.cssText = "height:100vh";*/
}
//退出全屏
function exitFullscreen() {
	fullscreen_status = false;
	if(list_status == false) {
		$("#videoarea").css("width", "92%")
		$("#videoarea").css("top", "140px")
		$("#videoarea").css("left", "128px")
		$("#videoarea").css("height", "900px")
		$("#launchFullscreen").css("display", "block")
		$("#exitFullscreen").css("display", "none")
	} else {
		$("#videoarea").css("width", "71.2%")
		$("#videoarea").css("top", "140px")
		$("#videoarea").css("left", "528px")
		$("#videoarea").css("height", "900px")
		$("#launchFullscreen").css("display", "block")
		$("#exitFullscreen").css("display", "none")
	}
}

//关闭所有选择框
function close_ckeck() {
	$('.delete').css('display', 'none');
	$('.getdelete').css('display', 'none');
	$('.download').css('display', 'none');
	$('.getdownload').css('display', 'none');
}

function list_style() {
	if(list_status == false) {
		close_ckeck();
		list_status = true;
		$('#big-menulist').animate({
			width: 'show'
		});
		//$('#big-menulist').toggle();
		$("#videoarea").css("width", "71.2%")
		$("#videoarea").css("top", "140px")
		$("#videoarea").css("left", "528px")
		$("#videoarea").css("height", "900px")
	} else {
		list_status = false;
		$('#big-menulist').toggle();
		//$('#big-menulist').toggle();
		$("#videoarea").css("width", "92%")
		$("#videoarea").css("top", "140px")
		$("#videoarea").css("left", "128px")
		$("#videoarea").css("height", "900px")
	}
}

function stopstart() {
	if(media1.paused) {
		//playflie();
	} else {
		//media.pause();
	}
}

function loop() {
	if(loop_status == 'simple') {
		$('.simple').css('display', 'block');
		$('.complex,.random').css('display', 'none');
		document.getElementById('media').currentTime = 0;
		if(now_obj.PlayState == 3) {
			get.Upload(now_obj)
		} else {
			playflie();
		}
	} else if(loop_status == 'complex') {
		$('.complex').css('display', 'block');
		$('.simple,.random').css('display', 'none');
		if(index_num < v.local_list.length - 1) {
			index_num++;
			now_obj = v.local_list[index_num];
			changeurl();
			if(now_obj.PlayState == 3) {
				get.Upload(now_obj)
			} else {
				playflie()
			}
		} else {
			index_num = 0;
			now_obj = v.local_list[0]
			changeurl();
			if(now_obj.PlayState == 3) {
				get.Upload(now_obj)
			} else {
				playflie()
			}
		}
	} else if(loop_status == 'random') {
		var free_num = parseInt(Math.random() * v.local_list.length);
		now_obj = v.local_list[free_num];
		changeurl();
		if(now_obj.PlayState == 3) {
			get.Upload(now_obj)
		} else {
			playflie()
		}
	} else {
		console.log('loop_status状态错误');
	}
}

//无法播放播放下一个
function next() {
	if(document.getElementById("media").networkState == 3) {
		//		$("#media").attr("src", list_patharr[index_num + 1]);
		//		media.play();
		//		next()
	}
	console.log(document.getElementById("media").networkState)
}

function go_delete() {
	if($('.getdelete').css('display') == 'none') {
		$('.delete').css('display', 'inline-block');
		$('.getdelete').css('display', 'block');
		$('.change_list_mode').css('display', 'none');
	} else {
		$('.delete').css('display', 'none');
		$('.getdelete').css('display', 'none');
		$('.change_list_mode').css('display', 'block');
	}
}

function addplanlist() {
	var adddata = [];
	var obj = document.getElementsByName("switch-add");
	var i;
	if(obj.length > 0) {
		for(k in obj) {
			if(obj[k].checked) {
				i = obj[k].value
				adddata.push(v.all_list[i].Guid)
			}
		}
		if(adddata.length > 0) {
			console.log(adddata[0])
			return adddata;
		} else {
			console.log('没有选择添加对象')
		}
	}
}

//循环方式//complex多 simple单 random随机
function loop_change(e) {
	if(e == "simple") {
		loop_status = "simple";
		$('.simple').css('display', 'block');
		$('.complex,.random').css('display', 'none');
	} else if(e == "complex") {
		loop_status = "complex";
		$('.complex').css('display', 'block');
		$('.simple,.random').css('display', 'none');
	} else if(e == "random") {
		loop_status = "random";
		$('.random').css('display', 'block');
		$('.simple,.complex').css('display', 'none');
	} else {
		console.log("循环模式错误");
	}
}



function deletevideo() {
	if(!deleteopen) {
		deleteopen=true;
		$('.menu_left').animate({
			width: "220px"
		}, 200);
		$('.menu_right').animate({
			width: "70px"
		}, 200);
	} else {
		deleteopen=false;
		$('.menu_left').animate({
			width: "290px"
		}, 200);
		$('.menu_right').animate({
			width: "0px"
		}, 200);
	}
}
function hiddendeleteinco(){
	deleteopen=false;
		$('.menu_left').css('width','290px')
		$('.menu_right').css('width','0px')
}

//tip
function tip(e) {
	$('.tip').html(e);
	$('.tip').css('display', 'block');
	setTimeout("$('.tip').css('display','none');", 2000)
}

//判断文件种类
function playflie() {
	if(now_obj.FileType == 0) {
		//处理第一个视频
		if(now_obj.PlayState == 3) {
			get.Upload(now_obj)
		} else {
			document.getElementById('audio').pause();
			$('.audioui_black').css('display', 'none');
			$('.audioui_txt').text('')
			$("#media").attr("autoplay", "true");
			get.SetPlayState();
		}
	} else if(now_obj.FileType == 1 || now_obj.FileType == 2) {
		//处理第一个音频
		if(now_obj.PlayState == 3) {
			get.Upload(now_obj)
		} else {
			document.getElementById('media').pause();
			$('.audioui_black').css('display', 'block');
			$('.audioui_txt').text('播放音频中')
			$("#audio").attr("autoplay", "true");
			get.SetPlayState();
		}
	} else {
		tip('文件格式错误:' + now_obj.FileType + '媒体播放')
	}
}
//改变URL
function changeurl() {
	if(now_obj.FileType == 0) {
		$("#media").attr("src", v.local_list[index_num].Url);
	} else if(now_obj.FileType == 1 || now_obj.FileType == 2) {
		$("#audio").attr("src", v.local_list[index_num].Url);
	} else {
		tip('文件格式错误:' + now_obj.FileType + 'URL')
	}
}

//播放/暂停
function startplay() {
	if(now_obj.FileType == 0) {
		document.getElementById('audio').pause();
		$('.audioui_black').css('display', 'none');
		$('.audioui_txt').text('')
		document.getElementById('media').play();
		//			state = 1;
		//			get.SetPlayState();
	} else if(now_obj.FileType == 1 || now_obj.FileType == 2) {
		document.getElementById('media').pause();
		$('.audioui_black').css('display', 'block');
		$('.audioui_txt').text('播放音频中')
		document.getElementById('audio').play();
		//			state = 1;
		//			get.SetPlayState();
	} else {
		tip('文件格式错误:' + now_obj.FileType + '开始播放')
	}
}

function stopplay() {
	document.getElementById('media').pause();
	document.getElementById('audio').pause();
	//	state = 2;
	//	get.SetPlayState();
}