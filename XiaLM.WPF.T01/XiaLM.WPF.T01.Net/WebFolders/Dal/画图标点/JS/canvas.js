$(function() {　　
	showimage();

});

function showimage() {
	$("#canvas").css("background", "url(images/11.jpg)");
	$("#canvas").css("background-repeat", "no-repeat");
	$("#canvas").css("background-size", "contain");
}

var bot; //画布div
var x, y, X1, Y1; //坐标
var flag = 0;
var time; //定时器ID
var color = 0; //记住所选颜色
var lineW = 5; //画笔粗细
var canvas; //创建画布
var context; //获取上下文
var isMouseDown = false; //记录鼠标是否按下

window.onload = function() {
	//创建画布
	canvas = document.getElementById("canvas");
	canvas.width = 1280;
	canvas.height = 960;
	//获取上下文
	context = canvas.getContext("2d");
	bot = document.getElementById("painting");
	canvas.onmousedown = function(e) {
		mouseDownAction(event)
	}
	canvas.onmousemove = function(e) {
		console.log(isMouseDown)
		if(isMouseDown) {
			x = e.clientX - 100;
			y = e.clientY - 180;
			drowline(x, y, X1, Y1);
			flag++;
			console.log(x + '-' + y + '-' + X1 + '-' + Y1)
		}
	}
	$('#painting').on('mouseup', function(event) {
		mouseUpAction(event)
	})

}

/**
 *选中画笔颜色
 */
function pen_click(num) {
	var chk = document.getElementsByTagName("input");
	for(var i = 0; i < chk.length; i++) {
		if(i == num) {
			chk[i].checked = true;
			color = i;
		} else {
			chk[i].checked = "";
		}
	}
}

/**
 * 画笔粗细
 */
function line_wid(num) {
	lineW = num;
}

/**
 *鼠标按下
 */
function mouseDownAction(e) {
	isMouseDown = true;
	//记录下鼠标点击的时候的位置
	x = e.clientX - 100;
	y = e.clientY - 180;
	console.log(x + '-' + y)
}

/**
 *鼠标移动
 */
function mouseMoveAction(e) {

}

/**
 *鼠标弹起来
 */
function mouseUpAction(e) {
	isMouseDown = false;
	flag = 0;
}

/**
 * 绘制
 */
function drowline(num1, num2, num3, num4) {
	//开启新的路径
	if(flag)
		context.beginPath();
	//移动画笔的初始位置
	context.moveTo(num1, num2);
	context.lineWidth = lineW;
	if(color == 0) {
		context.strokeStyle = "red";
	} else if(color == 1) {
		context.strokeStyle = "green";
	} else if(color == 2) {
		context.strokeStyle = "blue";
	}
	//移动画笔的结束位置
	context.lineTo(num3, num4);
	//开始绘制
	context.stroke();

	if(flag != 0) {
		X = X1;
		Y = Y1;
	}
}

function clear_pic() { //清除画布
	context.clearRect(0, 0, 500, 500);
}

function clear_save() { //清除保存
	var div = document.getElementsByClassName("div");
	for(var i = 0; i < div.length; i++) {
		div[i].innerHTML = "";
	}
}