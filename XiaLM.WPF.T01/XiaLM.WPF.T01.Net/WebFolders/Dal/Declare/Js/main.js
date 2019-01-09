var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('DeclareHub'); //创建连接器

connection.start().done(function() {
	//连接成功
	isRunning = true;
	addrow();
	parent.onload('Declare');
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

var rows_index = 0;
var rows;
var xlist=[];
var tablelist=[];

function addrow() {
	rows_index = rows_index + 1;
	rows=$('#inner_table').find("tr").length;
	if(rows<9){
		var str;
	str += '<tr id="row' + rows_index + '">'
	str += '<td class="row_input"><input class="row_input_unit variety"  style="font-size:25px" maxlength="20" type="text"></td>'
	str += '<td class="row_input"><input class="row_input_unit type" type="text" style="font-size:25px" maxlength="20"></td>'
	str += '<td class="row_input"><input class="row_input_unit number" type="text"  style="font-size:25px" maxlength="20"></td>'
	str += '<td class="row_input"><input class="row_input_unit money" type="text"   style="font-size:25px" maxlength="20"></td>'
	str += '<td class="row_input"><input class="row_input_unit comment" type="text" style="font-size:25px" maxlength="20" id=col_add' + rows_index + ' onclick="isaddrow(' + rows_index + ',this.value)"></td>'
	str += '<td class="row_input"><button class="btn btn-danger btn-lg" onclick="delrow(' + rows_index + ')"><font style="color:#fff">删除当前行</font></button></td>'
	str += '</tr>'
	$("#inner_table").append(str);
	    now=true;
	}else{
		now=false;
	}
	return now;
}


function isaddrow(e, _this) {
	if(_this != null || _this != "") {
		now=addrow();
		console.log(now)
		if(now!=false){
			$("#col_add" + e).removeAttr("onclick");
		}
	}
}

function delrow(e) {
	console.log(e)
	console.log($("#inner_table").find("tr").length);
	var inner_table_length = $("#inner_table").find("tr").length
	if(inner_table_length != 2) {
		$("#row" + e).remove();
	} else {
	}
}

function changeselect(dom,val){
	if($(dom).hasClass('active')){
		$(dom).removeClass('active');
		xlist.removeByValue(val);
	}else{
		$(dom).addClass('active');
		xlist.push(val);
	}
}

function changeselect(dom,val){
	if($(dom).hasClass('active')){
		$(dom).removeClass('active');
		xlist.removeByValue(val);
	}else{
		$(dom).addClass('active');
		xlist.push(val);
	}
}

function nextstap(io){
	$('.sbdpage').css('display','none');
	if(io=='入境'){
		$('#page_3').css('display','block');
	}else if(io=='出境'){
		$('#page_32').css('display','block');
	}else{
		tip('没有选择出入境')
		//$('#page_3').css('display','block');
	}
}

function gettablelist(){
	for(var i = 1; i < rows_index; i++) {
		var row="#row"+i;
		$tr = $(row);
		var middle={
			pm:$tr.find(".variety").val(),
			xh:$tr.find(".type").val(),
			sl:$tr.find(".number").val(),
			je:$tr.find(".money").val(),
			pz:$tr.find(".comment").val()
		}
		tablelist.push(middle);
	}
}

function postlist(){
	gettablelist();
	xliststr="";
	for(var i=0;i<xlist.length;i++){
		xliststr+=xlist[i];
	}
	var post={
		xm:$('#name').val(),
        xb:$('#sex').val(),
        csrq:$('#birthday_date').val(),    //出生日期
        gj:$('#country').val(),  //国籍
        hzh:$('#passport').val(), //护照(号)
        crj:$('#io').val(), //出/入境
        lzg:$('#cfrom').val(), //来自国
        hbh:$('#hangban').val(),//航班
        crrq:$('#io_date').val(),   //出入日期
        xdwmx:xliststr,   //携带物明细
        wplb:tablelist    //物品列表
	}
	contosoChatHubProxy.invoke('PrintDeclaration', post).done(function(s) {});
}

contosoChatHubProxy.on("DeclareStatusChange", function(s) {
	$('.msginfo').text(s.msgText)
	if(s.statusNum==1000){$('.status').text('扫描成功')}
	else if(s.statusNum==1001){$('.status').text('扫描开始')}
	else if(s.statusNum==1002){$('.status').text('扫描结束')}
	else if(s.statusNum==1003){$('.status').text('扫描异常')}
	else if(s.statusNum==1111){$('.status').text('扫描失败')}
	else if(s.statusNum==2000){$('.status').text('打印成功')}
	else if(s.statusNum==2001){$('.status').text('开始打印')}
	else if(s.statusNum==2002){$('.status').text('打印结束')}
	else if(s.statusNum==2003){$('.status').text('打印异常')}
	else if(s.statusNum==2222){$('.status').text('打印失败')}
	else{$('.status').text('状态码错误')}
});
contosoChatHubProxy.on("ScannerScanTimeout", function(s) {
	$('.msginfo').text('')
	$('.status').text('扫描超时');
	setTimeout('parent.BackHome();',4000);
});

Array.prototype.removeByValue = function(val) {
	for(var i = 0; i < this.length; i++) {
		if(this[i] == val) {
			this.splice(i, 1);
			break;
		}
	}
}

virtualkeyboard();


//是否启用护照
function SetCanBrushPassport(bo){
	contosoChatHubProxy.invoke('SetCanBrushPassport', bo)
}

//刷护照后的事件
contosoChatHubProxy.on("PassportFillData", function(s) {
	var sex=''
	$('#name').val(s.ln+s.fn);
	$('#sex').find("option").removeAttr("selected");
	$('#sex').val(s.sex);
	$('#sex').find("option[value='"+s.sex+"']").attr("selected",true);
	$('#birthday_date').val(s.bd);
	$('#country').val(s.na);
	$('#passport').val(s.id);
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