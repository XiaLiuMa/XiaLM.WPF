var on = false;
var faq = {};
var isRunning = false;
var que_index;
var ans_index;
var hotlist;
var status_index;
var color=['#e8b038','#32b16c','#7ecef4','#eb6877']
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('IndexViewHub'); //创建连接器
var contosoChatHubProxy_DialogueHub = connection.createHubProxy('EnDialogueHub');
connection.start().done(function() {
	//连接成功
	isRunning = true;
	faq.GetProblemTypes();
	//获取配置信息
	parent.onload('EnDialogue');
});

faq = {
	IsFirst:true,
	StarRecord: function() {
		contosoChatHubProxy_DialogueHub.invoke('StarRecord');

	},
	GetHotProblems: function(e) {
		$('.btn-change2').attr('onclick','faq.GetHotProblems(\"'+e+'\")')
		contosoChatHubProxy_DialogueHub.invoke('GetHotProblems', e).done(function(h) {
			$('.xiangsi2').empty();
			hotlist = h;
			if(h.length > 0) {
				for(var i = 0; i < h.length; i++) {
					$('.xiangsi2').append('<div onclick="fontshowhot(' + i + ')"><p class="qq">' + parseInt(i + 1) + '.' + h[i].Question + '</p></div>')
				}
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
				str = "<button style='background:"+color[i%4]+"' onclick='faq.GetHotProblems(\"" + e[i] + "\")'>" + e[i] + "</button>";
				$(".hotlist").append(str);
			}
		})
	},
	
};

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