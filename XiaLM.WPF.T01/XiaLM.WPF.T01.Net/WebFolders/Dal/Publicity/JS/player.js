var media = document.getElementById('media');
media.volume = 0.5;
var state=1;
media.addEventListener("ended", function() {
	console.log("结束");
	state=3;
	loop();
	get.SetPlayState();
})
media.addEventListener("pause", function() {
	state=2;
	console.log("暂停");
	get.SetPlayState();
})
media.addEventListener("play", function() {
	state=1;
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
	if(data.length>0){
		for(var i = 0; i < data.length; i++) {
			if(firsttime === true) {
				try {
					if(i == 0) {
						$("#media").attr("src", data[0].Path);
						now_obj = data[0];
						$("#media").attr("autoplay", "true");
						get.SetPlayState();
					}
				} catch(err) {
					console.log("没有得到有效的路径。" + err)
				}
			}
		}
	}else{
		video.Changelist(1);
		click_style(1);
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
	if(media.paused) {
		media.play();
	} else {
		media.pause();
	}
}

function loop() {
	if(loop_status == 'simple') {
		$('.simple').css('display', 'block');
		$('.complex,.random').css('display', 'none');
		media.currentTime = 0;
		media.play();
	} else if(loop_status == 'complex') {
		$('.complex').css('display', 'block');
		$('.simple,.random').css('display', 'none');
		if(index_num < v.local_list.length - 1) {
			index_num++;
			$("#media").attr("src", v.local_list[index_num].Path);
			now_obj=v.local_list[index_num];
			media.play();
		} else {
			index_num = 0;
			$("#media").attr("src", v.local_list[0].Path);
			now_obj=v.local_list[0]
			media.play();
		}
	} else if(loop_status == 'random') {
		var free_num = parseInt(Math.random() * v.local_list.length);
		$("#media").attr("src", v.local_list[free_num].Path);
		now_obj=v.local_list[free_num];
		media.play();
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

function deletes() {
	var data = [];
	var obj = document.getElementsByName("switch-delete");
	var i;
	if(obj.length > 0) {
		for(k in obj) {
			if(obj[k].checked) {
				i = obj[k].value
				data.push(v.local_list[i])
			}
		}
		if(data.length > 0) {
			console.log(data[0].Session)
			return data;
		} else {
			console.log('没有选择删除对象')
		}
	}
}

function downloads() {
	var data = [];
	var obj = document.getElementsByName("switch-download");
	var i;
	if(obj.length > 0) {
		for(k in obj) {
			if(obj[k].checked) {
				i = obj[k].value
				data.push(v.server_list[i])
			}
		}
		if(data.length > 0) {
			console.log(data[0].SessionId)
			return data;
		} else {
			console.log('没有选择下载对象')
		}
	}
}

function go_download() {
	if($('.getdownload').css('display') == 'none') {
		$('.download').css('display', 'inline-block');
		$('.getdownload').css('display', 'block');
		$('.change_list_mode').css('display', 'none');
	} else {
		$('.download').css('display', 'none');
		$('.getdownload').css('display', 'none');
		$('.change_list_mode').css('display', 'block');
	}
}

function download() {
	$('.download').css('display', 'none');
	$('.getdownload').css('display', 'none');
	download_arr = [];
	download_json = [];
	obj = document.getElementsByName("switch-download");
	for(k in obj) {
		if(obj[k].checked)
			download_arr.push(obj[k].value);
	}
	upload_download_count = 0;
	upload_download_count = download_arr.length;
	$("#totle").html(upload_download_count);
	$("#complete").html(upload_download_count);
	for(var i = 0; i < download_arr.length; i++) {
		var a = {
			Id: list_Server[download_arr[i]].Id,
			Genre: list_Server[download_arr[i]].Genre,
			Name: list_Server[download_arr[i]].Name,
			Desc: list_Server[download_arr[i]].Description,
			Path: list_Server[download_arr[i]].Path,
			PathMark: list_Server[download_arr[i]].PathMark
		}
		download_json.push(a);
	}
	get.DownloadFile();
}

function click_style(e) {
	$(".menuicon").css("background", "none");
	if(e === 0) {
		$('.local').css("background", "#666");
	} else {
		$('.server').css("background", "#666");
	}

}

//收集当前的信息
function StatusInfo() {
	StatusInfo_package = {
		IsPlaying: (media.paused) ? IsPlaying = false : IsPlaying = true,
		FileOnlyFlag: now_address,
		CycleMode: loop_status,
		ChangeVolume: 50,
		IsFullScreen: fullscreen_status,
		IsBusy: busy,
		IsScreenLock: Permissions_Lock
	}
	get.GetPromotionalStatusInfo();
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