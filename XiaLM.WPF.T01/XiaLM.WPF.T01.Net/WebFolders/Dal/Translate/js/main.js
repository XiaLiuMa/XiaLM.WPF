var Translate={}
var areaheight = 0;
var languagestatus = 'CN_EN';
var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('TranslateHub'); //创建连接器
var timer;

connection.start().done(function() {
	//连接成功
	isRunning = true;
	Translate.SwitchTranslateType();
	parent.onload('Translate');
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

Translate={
	StartRecording:function(){
		contosoChatHubProxy.invoke('StartRecording').done(function(list) {
			$(".t1").empty();
			$(".t2").empty();
			$('#speak_img').addClass('anime1');
		})
	},
	SwitchTranslateType:function(){
		contosoChatHubProxy.invoke('SwitchTranslateType',languagestatus).done(function(list) {
			
		})
	}
}

//开始录音
contosoChatHubProxy.on("RecoedEvent", function(s) {
	if(s==0){
		console.log("开始录音"); //开始录音
			$(".before_Translation").empty();
			$(".after_Translation").empty();
			$('#speak_img').addClass('anime1');
	}else{
		$('#speak_img').removeClass('anime1');
	}
});
//注册事件
contosoChatHubProxy.on('ReturnRecordAfteStr', //接收答案
	function(data) {
		$(".t2").html(data);
	});
contosoChatHubProxy.on('ReturnRecordBeforeStr', //接收提问
	function(data) {
		$(".t1").html(data);
	});
contosoChatHubProxy.on('RecodEnd', //结束录音
	function(data) {
		$('#speak_img').removeClass('anime1');
	});
	contosoChatHubProxy.on('SoundDetectionEvent', //声音检测
	function(data) {
		if(data===true){
			$('.stext').text('点击开始翻译');
		}else{
			$('.stext').text('正在识别..');
		}
	});



//切换语种
function change() {
	var cn = '中文(cn) <img src="images/CN.png" width="46px"/>'
	var en = '&nbsp;&nbsp;英文(en)<img src="images/EN.png">'
	if($(".l1").hasClass('active')) {
		$(".l1").removeClass('active');
		$(".l1").html(en)
		$(".l2").html(cn)
		languagestatus = 'EN_CN';
	} else {
		$(".l1").addClass('active');
		$(".l1").html(cn)
		$(".l2").html(en)
		languagestatus = 'CN_EN';
	}
	Translate.SwitchTranslateType();
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