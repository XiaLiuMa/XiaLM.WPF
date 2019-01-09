$(function() {　　
	showimage();

});

function showimage() {
	$("#canvas").css("background", "url(images/11.jpg)");
	$("#canvas").css("background-repeat", "no-repeat");
	$("#canvas").css("background-size", "contain");
}

var layer = 0;
var status = 1;
var coordinate = {};
CanvasExt = {
	line: function(canvasId, penColor, strokeWidth, ctype, startY) {
		var type = ctype;
		var pen = this;
		pen.penColor = penColor;
		pen.penWidth = strokeWidth;

		var canvas = document.getElementById(canvasId);
		var ctx = canvas.getContext("2d");
		ctx.lineWidth = 4;
		ctx.strokeStyle = "red";
		canvas.width = 1280;
		canvas.height = 960;
		//canvas 的矩形框
		var canvasRect = canvas.getBoundingClientRect();
		//矩形框的左上角坐标
		var canvasLeft = canvasRect.left;
		var canvasTop = 130

		var x = 0;
		var y = 0;
		var startY = startY;

		//鼠标点击按下事件，画图准备
		canvas.onclick = function(e) {

			x = 0;
			y = 0;
			width = 0;
			height = 0;
			var color = pen.penColor;
			var penWidth = pen.penWidth;
			var scrollY = 0

			x = e.clientX - canvasLeft;
			y = e.clientY - canvasTop + scrollY + startY;
			console.log(e.clientY + "-" + canvasTop + "-" + scrollY + "+" + startY + "=" + y);

			if(status==2) {
				coordinate.x2 = x
				coordinate.y2 = y
				$('.x1').html(parseInt(coordinate.x1/2))
				$('.y1').html(parseInt(coordinate.y1/2))
				$('.x2').html(parseInt(coordinate.x2/2))
				$('.y2').html(parseInt(coordinate.y2/2))
				ctx.beginPath();
				ctx.moveTo(coordinate.x1, coordinate.y1);
				ctx.lineTo(coordinate.x2, coordinate.y2);
				ctx.strokeStyle = '#fff';
				ctx.lineWidth = 6;
				ctx.stroke();
				ctx.fillStyle = "red";
				ctx.fillRect(coordinate.x1 - 6, coordinate.y1 - 6, 12, 12);
				ctx.fillRect(x - 6, y - 6, 12, 12);
				ctx.stroke();
				status = 1;
				$('.saveline').removeAttr('disabled')
			} else {
				CanvasExt.remove();
				coordinate.x1 = x
				coordinate.y1 = y
				$('.x1').html(parseInt(coordinate.x1/2))
				$('.y1').html(parseInt(coordinate.y1/2))
				ctx.fillStyle = "red";
				ctx.fillRect(x - 6, y - 6, 12, 12);
				ctx.stroke();
				status = 2;
				$('.saveline').attr('disabled','disabled')
			}
		};

		//		canvas.onmousedown = function(e) {
		//			CanvasExt.remove()
		//			//设置画笔颜色和宽度
		//			x = 0;
		//			y = 0;
		//			width = 0;
		//			height = 0;
		//			var color = pen.penColor;
		//			var penWidth = pen.penWidth;
		//			var scrollY = 0
		//
		//			layerIndex++;
		//			layer++;
		//			layerName += layerIndex;
		//			x = e.clientX - canvasLeft;
		//
		//			y = e.clientY - canvasTop + scrollY + startY;
		//			console.log(e.clientY + "-" + canvasTop + "-" + scrollY + "+" + startY + "=" + y);
		//
		//			$("#" + canvasId).drawLayers();
		//			$("#" + canvasId).saveCanvas();
		//
		//		};
		//
		//		canvas.onmouseup = function(e) {
		//
		//			var color = pen.penColor;
		//			var penWidth = pen.penWidth;
		//
		//			canvas.onmousemove = null;
		//
		//			width = width;
		//			height = height;
		//			//$("#xf").html(x+width);
		//			//$("#yf").html(height+y);
		//
		//			$("#" + canvasId).removeLayer(layerName);
		//
		//			$("#" + canvasId).addLayer({
		//				type: 'line',
		//				strokeStyle: color,
		//				strokeWidth: penWidth,
		//				name: layerName,
		//				fromCenter: false,
		//				x1: 200,
		//				y1: 50,
		//				x2: x,
		//				y2: y,
		//			});
		//
		//			$("#" + canvasId).drawLayers();
		//			$("#" + canvasId).saveCanvas();
		//
		//		}
	},
	remove: function() {
		var drawing = document.getElementById("canvas");
		var ctx = drawing.getContext("2d");
		ctx.clearRect(0,0,canvas.width,canvas.height);
		canvas.height=canvas.height;
		//$('#canvas').removeLayers();
	},

};

//drawPen();
function drawPen1() {
	status = 1;
	var color = "red";
	var width = 5;
	var type = 1;
	var startY = 0;
	console.log(startY)
	CanvasExt.line("canvas", color, width, type, startY);

}

function removeit() {
	CanvasExt.remove();
}