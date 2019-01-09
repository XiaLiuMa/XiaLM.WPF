var isRunning = false;
var card;
//扫描仪状态
var issucceed = false;
var statusNum = -1;
var printstatusNum = -1;

var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('ExitAndEntryCardPrintHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	isRunning = true;
	parent.onload('ExitAndEntry');
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

card = {
	StartScanner: function() {
		contosoChatHubProxy.invoke('StartScanner');
	},
	PrintExitAndEntryCard: function(obj) {
		contosoChatHubProxy.invoke('PrintExitAndEntryCard',obj);
	}
}

contosoChatHubProxy.on("ScannerStatusChange", function(state) {
	statusNum = state.statusNum;
	tip(state.msgText);
});
contosoChatHubProxy.on("ScannerScanTimeout", function() {
	tip('扫描超时');
});

contosoChatHubProxy.on("QrCodeFillData", function(d) {
	if(d.Category == "ForeignerEntryCard") {
		page2();
		page2dataget(d.Body);
	} else if(d.Category == "ForeignerExitCard") {
		page3();
		page3dataget(d.Body);
	} else if(d.Category == "TemporaryEntryCard") {
		page4();
		page4dataget(d.Body);
	} else if(d.Category == "TemporaryExitCard") {
		page5();
		page5dataget(d.Body);
	} else {
		tip('卡片类型错误')
	}
});

contosoChatHubProxy.on("PassportFillData", function(d) {
	$('#ln1,#ln2,#ln3,#ln4').val(d.ln);
	$('#fn1,#fn2,#fn3,#fn4').val(d.fn);
	$('#na1,#na2,#na3,#na4').val(d.na);
	$('#id1,#id2,#id3,#id4').val(d.id);
	$('#sex1,#sex2,#sex3,#sex4').val(d.sex);
	$('#bd1,#bd2,#bd3,#bd4').val(d.bd);
});

contosoChatHubProxy.on("PrintStatusChange", function(status) {
	//var isSucceed=status.isSucceed;
	printstatusNum=status.statusNum;
	tip(state.msgText);
});

var rows_index = 0;
var rows;
var xlist = [];
var tablelist = [];

function page1() {
	$('.sbdpage').css('display', 'none')
	$('#page_1').css('display', 'block')
}
function page2() {
	$('.sbdpage').css('display', 'none')
	$('#page_2').css('display', 'block')
}

function page2dataget(d) {
	$('#ln1').val(d.ln)
	$('#fn1').val(d.fn)
	$('#na1').val(d.na)
	$('#id1').val(d.id)
	$('#inc1').val(d.inc)
	$('#sex1').find("option[value=" + d.sex + "]").attr("selected", "selected");
	$('#bd1').val(d.bd)
	$('#vn1').val(d.vn)
	$('#povl1').val(d.povl)
	$('#fight1').val(d.fight)
	$('#pv1').find("option[value=" + d.pv + "]").attr("selected", "selected");
}

function page2dataset() {
	var obj = {
		ln: $('#ln1').val(),
		fn: $('#fn1').val(),
		na: $('#na1').val(),
		id: $('#id1').val(),
		inc: $('#inc1').val(),
		sex: $('#sex1').val(),
		bd: $('#bd1').val(),
		vn: $('#vn1').val(),
		povl: $('#povl1').val(),
		fight: $('#fight1').val(),
		pv: $('#pv1').val()
	}
	var obj2={
		Category:'ForeignerEntryCard',
		Body:obj
	}
	card.PrintExitAndEntryCard(obj2);
}

function page3() {
	$('.sbdpage').css('display', 'none')
	$('#page_3').css('display', 'block')
}

function page3dataget(d) {
	$('#ln2').val(d.ln)
	$('#fn2').val(d.fn)
	$('#na2').val(d.na)
	$('#id2').val(d.id)
	$('#bd2').val(d.bd)
	$('#fight2').val(d.fight)
	$('#sex2').find("option[value=" + d.sex + "]").attr("selected", "selected");
}

function page3dataset() {
	var obj = {
		ln: $('#ln2').val(),
		fn: $('#fn2').val(),
		na: $('#na2').val(),
		id: $('#id2').val(),
		sex: $('#sex2').val(),
		bd: $('#bd2').val(),
		fight: $('#fight2').val()
	}
	var obj2={
		Category:'ForeignerExitCard',
		Body:obj
	}
	card.PrintExitAndEntryCard(obj2);
}

function page4() {
	$('.sbdpage').css('display', 'none')
	$('#page_4').css('display', 'block')
}

function page4dataget(d) {
	$('#name3').val(d.name)
	$('#sex3').find("option[value=" + d.sex + "]").attr("selected", "selected");
	$('#ann3').val(d.ann)
	$('#bd3').val(d.bd)
	$('#na3').val(d.na)
	$('#pb3').val(d.pb)
	$('#annation3').val(d.annation)
	$('#id3').val(d.id)
	$('#pels3').find("option[value=" + d.pels + "]").attr("selected", "selected");
	$('#pttc3').val(d.pttc)
	$('#fight3').val(d.fight)
	$('#ytn3').val(d.ytn)
	$('#hbcb3').find("option[value=" + d.hbcb + "]").attr("selected", "selected");
	$('#email3').val(d.email)
	$('#inc3').val(d.inc)
	$('#idof3').val(d.idof)
	$('#ipod3').val(d.ipod)
	$('#ifight3').val(d.ifight)
}

function page4dataset() {
	var obj = {
		name: $('#name3').val(),
		sex: $('#sex3').val(),
		ann: $('#ann3').val(),
		bd: $('#bd3').val(),
		na: $('#na3').val(),
		pb: $('#pb3').val(),
		annation: $('#annation3').val(),
		id: $('#id3').val(),
		pels: $('#pels3').val(),
		pttc: $('#pttc3').val(),
		fight: $('#fight3').val(),
		ytn: $('#ytn3').val(),
		hbcb: $('#hbcb3').val(),
		email: $('#email3').val(),
		inc: $('#inc3').val(),
		idof: $('#idof3').val(),
		ipod: $('#ipod3').val(),
		ifight: $('#ifight3').val()
	}
	var obj2={
		Category:'TemporaryEntryCard',
		Body:obj
	}
	card.PrintExitAndEntryCard(obj2);
}

function page5() {
	$('.sbdpage').css('display', 'none')
	$('#page_5').css('display', 'block')
}

function page5dataget(d) {
	$('#name4').val(d.name)
	$('#ann4').val(d.ann)
	$('#id4').val(d.id)
	$('#bd4').val(d.bd)
	$('#sex4').find("option[value=" + d.sex + "]").attr("selected", "selected");
	$('#fight4').val(d.fight)
	$('#na4').val(d.na)
}

function page5dataset() {
	var obj = {
		name: $('#name4').val(),
		ann: $('#ann4').val(),
		id: $('#id4').val(),
		bd: $('#bd4').val(),
		sex: $('#sex4').val(),
		fight: $('#fight4').val(),
		na: $('#na4').val()
	}
	var obj2={
		Category:'TemporaryExitCard',
		Body:obj
	}
	card.PrintExitAndEntryCard(obj2);
}

function tip(s) {
	$("#tip").css("display", "block")
	$("#tip").text(s);
	setTimeout('$("#tip").css("display","none")', 1500)
}

Array.prototype.removeByValue = function(val) {
	for(var i = 0; i < this.length; i++) {
		if(this[i] == val) {
			this.splice(i, 1);
			break;
		}
	}
}

virtualkeyboard();
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