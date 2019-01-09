var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('BaggageInquiryHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('IndexViewHub'); //创建连接器
var timer;
var forbidstate=true;
connection.start().done(function() {
	//连接成功
	isRunning = true;
	//获取配置信息
	parent.onload('BaggageInquiry');
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

contosoChatHubProxy.on("OcrResultEvent", function(e) {
	$('.fontwindow').css('display','none')
	jsonobj=$.parseJSON(e)
	$('.guest_name').text(jsonobj.data.lk.pname)
	$('.guest_nationality').text(jsonobj.data.lk.pnationality)
	$('.guest_num').text(jsonobj.data.lk.pcertNo)
	var str='';
	$('.tbody').empty();
	for(var i=0;i<jsonobj.data.wp.length;i++){
		str='';
		str+='<tr><td>'+jsonobj.data.wp[i].gname+'</td><td>'+jsonobj.data.wp[i].gquantity+'</td><td>'+jsonobj.data.wp[i].gweight+'</td>'
		str+='<td>'+jsonobj.data.wp[i].goperation+'</td><td>'+jsonobj.data.wp[i].gdeclTime+'</td></tr>'
		$('.tbody').append(str)
	}
	forbidstate=false;
});


contosoChatHubProxy.on("LoadingEvent", function(e) {
	if(e){
		$(".forbid").css('display', 'block');
	}else{
		$(".forbid").css('display', 'none');
	}
});

contosoChatHubProxy.on("MessageEvent", function(e) {
	tip(e)
});

contosoChatHubProxy.on("MessageEvent2", function(e) {
	$('.fontwindow').css('display','none');
	$('.guest_name').text(e.Name);
	$('.guest_nationality').text(e.Nationality);
	$('.guest_num').text(e.Id);
	forbidstate=false;
});

contosoChatHubProxy2.on("PageBackEvent", function(pageEvent) {
	changepage(pageEvent.Url, pageEvent.MenuName, pageEvent.DescName);
	console.log(pageEvent.DescName);
});

$("#back_index").click(function() {
	if(isRunning = true) {
		contosoChatHubProxy2.invoke('BackHome').done(function(s) {
			if(s == true) {
				window.location.href = "http://localhost:15689/index.html"
			}
		});
	} else {
		console.log("断开连接");
	}
});

function failed() {
	$("#fail").css("display", "block");
	setTimeout('$("#fail").css("display","none");', 10000);
}

function tip(e) {
	$(".tip").css("display", "block");
	$(".tip").html(e)
	clearTimeout(timer)
	timer=setTimeout('$(".tip").css("display","none");', 3000);
}

//隐藏图片
function ScreenSleep() {
	$('.fontwindow').css('display', 'block');
	forbidstate=true;
}

var a = setTimeout("ScreenSleep()", 300000);
$("body").click(function() {	
	if(!forbidstate){
		clearTimeout(a);
		$('.fontwindow').css('display', 'none')
		a = setTimeout("ScreenSleep()", 300000);
	}	
})


//function togglefontpage(bo) {
//	if(bo) {
//		$(".pagefont").animate({
//			opacity: 1
//		}, 500);
//		$('.back').css('display','none')
//	} else {
//		$(".pagefont").animate({
//			opacity: 0
//		}, 500);
//		$('.back').css('display','block')
//	}
//}
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