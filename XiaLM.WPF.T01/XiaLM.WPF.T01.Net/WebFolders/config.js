//--------------------------------------
//海关》H   边检》B   盛视》S
//--------------------------------------
var config="S"
var config_data=""


if(config=="H"){
	$('.PBimg').attr('src','rec/img/PB.png');
	$('.logoimg').attr('src','img/l1.png');
	$('.logo').css('left','887px');
	$('.cname').text('中国海关服务电话');
	config_data='中国海关服务电话'
}else if(config=="B"){
	$('.PBimg').attr('src','rec/img/PB2.png');
	$('.logoimg').attr('src','img/l2.png');
	$('.logo').css('left','887px');
	$('.cname').text('中国边检服务电话');
	config_data='中国边检服务电话'
}else if(config=="S"){
	$('.PBimg').attr('src','rec/img/PB3.png');
	$('.logoimg').attr('src','img/l3.png');
	$('.logo').css('left','840px');
	$('.cname').text('人工服务电话');
	config_data='人工服务电话'
}else{
	
}
