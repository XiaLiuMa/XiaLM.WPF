var draw = function(x, y, width, height, color){			
var drawing=document.getElementById("canvas");
var ctx=drawing.getContext("2d");		
    ctx.beginPath();
    ctx.strokeStyle=color;//轮廓颜色
    ctx.strokeRect(x,y,width,height);//绘制矩形轮廓
    ctx.closePath();
};