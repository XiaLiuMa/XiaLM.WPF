var on = false;
var Transit = {};
var isRunning = false;
var arr = [];
var unit;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('TransitHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	Transit.GoIndexStep();
	parent.onload('Transit');
});

Transit = {
	GoIndexStep: function() {
		contosoChatHubProxy.invoke('GoIndexStep').done(function(e) {
			$('.sbdpage').css('display','none');
			$('#page_1').css('display','block');
			$('.mliststart').empty();
			$('.TSY').empty();
			//$('.TSY').html(e.TSY);
			arr = e.Qpoints;
			for(var i = 0; i < e.Qpoints.length; i++) {
				var ftext = $('.ftext');
				ftext[i].innerText = arr[i].Text;
			}
			LoadedTrigger(e.TSY,true);
		})
	},
	GoPreviousStep: function() {
		contosoChatHubProxy.invoke('GoPreviousStep').done(function(e) {
			$('.mliststart').empty();
			$('.TSY').html(e.TSY);
			arr = e.Qpoints;
			for(var i = 0; i < e.Qpoints.length; i++) {
				if(e.Qpoints[i].IsEnd) {
					str = '<div class="mlist" onclick="golast('+i+')">'
					str += '<p><span>' + arr[i].Text + '</span></p>'
					str += '</div>'
					$('.mliststart').append(str);
				} else {
					str = '<div class="mlist" onclick="clicked(' + i + ')">'
					str += '<p><span>' + arr[i].Text + '</span></p>'
					str += '</div>'
					$('.mliststart').append(str);
				}
			}
			if(e.Qpoints[0].Y > 2) {
				$('.pre').css('display', 'block')
			}
			if(e.Qpoints[0].Y < 3) {
				$('.pre').css('display', 'none')
			}
			if(e.IsIndex) {
				$('.sbdpage').css('display','none');
			    $('#page_1').css('display','block');
			}
			LoadedTrigger(e.TSY,true);
		})
	},
	GoNextStep: function() {
		contosoChatHubProxy.invoke('GoNextStep', unit).done(function(e) {
			$('.sbdpage').css('display','none');
			$('#page_2').css('display','block');
			$('.mliststart').empty();
			$('.TSY').html(e.TSY);
			arr = e.Qpoints;
			for(var i = 0; i < e.Qpoints.length; i++) {
				if(e.Qpoints[i].IsEnd) {
					str = '<div class="mlist" onclick="golast('+i+')">'
					str += '<p><span>' + arr[i].Text + '</span></p>'
					str += '</div>'
					$('.mliststart').append(str);
				} else {
					str = '<div class="mlist" onclick="clicked(' + i + ')">'
					str += '<p><span>' + arr[i].Text + '</span></p>'
					str += '</div>'
					$('.mliststart').append(str);
				}
			}
			if(e.Qpoints[0].Y > 2) {
				$('.pre').css('display', 'block');
			}
			if(e.Qpoints[0].Y < 3) {
				$('.pre').css('display', 'none');
			}
			LoadedTrigger(e.TSY,true);
		})
	},
	ShowResult: function() {
		contosoChatHubProxy.invoke('ShowResult', unit).done(function(e) {
			$('.sbdpage').css('display','none');
			$('#page_3').css('display','block');
			$('.titleimg').attr('src','');
				$('.tittext').text('');
			if(e.IsEligible){
				$('.titleimg').attr('src','images/T.png');
				$('.tittext').text('您好，以下是办理此业务的相关信息。');
				var imgstr='tableimg/'+e.Txt;
				$('.othisimg').css('display','block')
				$('.othisimg').attr('src',imgstr);
				LoadedTrigger('您好，以下是办理此业务的相关信息。',false);
			}else{
				$('.titleimg').attr('src','images/F.png');
				$('.tittext').text('对不起，您不符合办理条件。');
				$('.othisimg').css('display','none');
				LoadedTrigger('对不起，您不符合办理条件。',false);
			}
		})
	},
	StarRecording: function() {
	    contosoChatHubProxy.invoke('StarRecording');
	}
}

contosoChatHubProxy.on("NoticeSpecifiedNode", function(noticeType, qpoint) {
	unit = qpoint;
	if(noticeType === "IndexStep") {
		Transit.GoIndexStep();
	} else if(noticeType === "PreviousStep") {
		Transit.GoPreviousStep();
	} else if(noticeType === "NextStep") {
		if(qpoint.IsEnd){
			Transit.ShowResult();
		}else{
			Transit.GoNextStep();
		}
	} else {
		console.log('错误的noticeType')
	}
});

contosoChatHubProxy.on("NoticeWarningMessage", function(s) {
     $('.warming').html(s);
});
contosoChatHubProxy.on("NoticeViewEnable", function(s) {
     if(s){
     	$('#forbid').css('display','none');
     }else{
     	$('#forbid').css('display','block');
     }
});

function bb() {
	$('.sbdpage').css('display', 'none');
	$('#page_1').css('display', 'block');
	Transit.GoIndexStep();
}

function n() {
	if(unit.IsEnd === true) {
		Transit.ShowResult();
	} else {
		Transit.GoNextStep();
	}
}
function b() {
	Transit.GoPreviousStep();
}
function clicked(e) {
	unit = arr[e];
	Transit.GoNextStep();
}
function golast(e){
	unit = arr[e];
	Transit.ShowResult();
}
function LoadedTrigger(tsy,boo){
	$('.warming').html('')
	console.log(tsy,boo)
	contosoChatHubProxy.invoke('LoadedTrigger',tsy,boo);
}

//开始录音
contosoChatHubProxy.on("NoticeRecordState", function(b) {
	if(b){
		$('.speak_img').addClass('anime1');
	}else{
		$('.speak_img').removeClass('anime1');
	}
});

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