var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('VFaceHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('IndexViewHub'); //创建连接器
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	SelectAllFaceList();
    parent.onload('VFace');
});

function SelectAllFaceList() {
	$('.binit').empty()
	$('.winit').empty()
	contosoChatHubProxy.invoke('SelectAllFaceList')
}

function DeleteFaces(t, s) {
	var a = {
		type: t,
		serialnumber: s
	}
	var b = [];
	b.push(a);
	contosoChatHubProxy.invoke('SelectWhiteFaces', b)
}

function ClearWhiteFaces() {
	contosoChatHubProxy.invoke('ClearWhiteFaces')
}

function ClearBlackFaces() {
	contosoChatHubProxy.invoke('ClearBlackFaces')
}

function ClearAllFaces() {
	contosoChatHubProxy.invoke('ClearAllFaces')
}

function wupload() {
	var type = 'white'
	var serialnumber = $('.wserialnumber').val();
	var name = $('.wname').val();
	var sex = $('.wsex').val();
	var idnumber = $('.widnumber').val();
	var filename = document.getElementById("headflie1").files[0].name;
	var imagebytes = $('#avatorVal1').val();    
	console.log(filename)
	var a = {
		type: type,
		serialnumber: parseInt(serialnumber),
		name: name,
		sex: sex,
		idnumber: idnumber,
		filename: filename,
		imagebytes: imagebytes
	}
	var b = [];
	b.push(a);
	contosoChatHubProxy.invoke('UploadFaceList', b)
}

function bupload() {
	var type = 'black'
	var serialnumber = $('.bserialnumber').val();
	var name = $('.bname').val();
	var sex = $('.bsex').val();
	var idnumber = $('.bidnumber').val();
	var filename = document.getElementById("headflie2").files[0].name;
	var imagebytes = $('#avatorVal2').val();    
	console.log(imagebytes)
	var a = {
		type: type,
		serialnumber: parseInt(serialnumber),
		name: name,
		sex: sex,
		idnumber: idnumber,
		filename: filename,
		imagebytes: imagebytes
	}
	var b = [];
	b.push(a);
	contosoChatHubProxy.invoke('UploadFaceList', b)
}

//头像
$('#headflie1').change(function() {
	if(this.files.length != 0) {
		var file = this.files[0],
			reader = new FileReader();
		if(!reader) {
			this.value = '';
			return;
		};
		console.log(file.size,file.type)
		if(!/image/g.test(file.type)) {
			toast("请上传图片文件!")
			$('#headflie1').val('')
			$('#avatorVal1').val('')
			$('#img1').attr('src', '')
			return
		}
		reader.onload = function(e) {
			this.value = '';
			var image = new Image();
			$('#img1').attr('src', e.target.result)
			$('#img1').fadeIn()
			image.src = e.target.result
			image.onload = function() {
				$('#avatorVal1').val(e.target.result)
			}
		};
		reader.readAsDataURL(file);
	};
})
$('#headflie2').change(function() {
	if(this.files.length != 0) {
		var file = this.files[0],
			reader = new FileReader();
		if(!reader) {
			this.value = '';
			return;
		};
		console.log(file.size,file.type)
		if(!/image/g.test(file.type)) {
			toast("请上传图片文件!")
			$('#headflie2').val('')
			$('#avatorVal2').val('')
			$('#img2').attr('src', '')
			return
		}
		reader.onload = function(e) {
			this.value = '';
			var image = new Image();
			$('#img2').attr('src', e.target.result)
			$('#img2').fadeIn()
			image.src = e.target.result
			image.onload = function() {
				$('#avatorVal2').val(e.target.result)
			}
		};
		reader.readAsDataURL(file);
	};
})

contosoChatHubProxy.on("FeedBackSelectResult", function(arr) {
	var str = ''
	str = '<div class="listinfo">' +
		'<a>类型：<span>' + arr.type + '</span></a><br />' +
		'<a>名单数量：<span>' + arr.toal + '</span></a><br />' +
		'<a>当前第几条记录：<span>' + arr.index + '</span></a><br />' +
		'<a>姓名：<span>' + arr.name + '</span></a><br />' +
		'<a>性别：<span>' + arr.sex + '</span></a><br />' +
		'<a>人员编号：<span>' + arr.serialnumber + '</span></a><br />' +
		'<a>身份证号：<span>' + arr.idnumber + '</span></a><br />' +
		'<a>文件名：<span>' + arr.filename + '</span></a><br />' +
		'<button >删除</button>' +
		'</div>'
	if(arr.type == "black") {
		$('.binit').html(str)
	} else {
		$('.winit').html(str)
	}
});

contosoChatHubProxy.on("FeedBackDeleteResult", function(info) {
	if(info == 101) {
		info = '删除成功'
	} else {
		info = '删除失败'
	}
	$('.fankuixingxi').prepend('<br>');
	$('.fankuixingxi').prepend(info);
});

contosoChatHubProxy.on("FeedBackUploadResult", function(info) {
	if(info == 101) {
		info = '上传成功'
	} else {
		info = '上传失败'
	}
	$('.fankuixingxi').prepend('<br>');
	$('.fankuixingxi').prepend(info);
});

contosoChatHubProxy.on("FeedBackAlarmResult", function(info) {
	for(var i in info) {
		str = '<p>机器人编号:' + info[i].robotnumber + '</p>'
		str += '<p>报警人员编号:' + info[i].serialnumber + '</p>'
		str += '<p>二进制文件流:' + info[i].imagebytes + '</p>'
		$('.fankuixingxi').prepend('<br>');
		$('.fankuixingxi').prepend(str);
	}
});

contosoChatHubProxy.on("FeedBackIdentifyState", function(info) {
	if(info == 101) {
		info = '启动识别成功'
	} else if(info == 102) {
		info = '启动识别失败'
	} else if(info == 103) {
		info = '关闭识别成功'
	} else if(info == 104) {
		info = '关闭识别失败'
	} else if(info == 105) {
		info = '重新识别成功'
	} else if(info == 106) {
		info = '重新识别失败'
	}
	$('.fankuixingxi').prepend('<br>');
	$('.fankuixingxi').prepend(info);
});