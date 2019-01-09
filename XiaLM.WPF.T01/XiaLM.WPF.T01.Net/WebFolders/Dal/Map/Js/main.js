var isRunning = false;
var media = document.getElementById('mediamap');
media.volume = 0.5;
media.addEventListener("ended", function() {
	console.log("播放结束");
	$('.video').css('display','none');
})
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('MapHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	isRunning = true;
	initmap();
	parent.onload('Map');
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

function initmap(){
	contosoChatHubProxy.invoke('GetMapInfo').done(function(data) {
		if(data==""){
			$(".iframe_map").css("display", "block")
			$(".iframe_map").attr("src", 'error.html');
		}else{
			$(".iframe_map").css("display", "block")
			$(".iframe_map").attr("src", data);
		}
	});
}

contosoChatHubProxy.on("PlayMp4Event", function(u) {
	$('.video').css('display','block');
	$('#mediamap').attr('src',u);
	media.play();
});


function stopvideo(){
	$('.video').css('display','none');
	media.pause();
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