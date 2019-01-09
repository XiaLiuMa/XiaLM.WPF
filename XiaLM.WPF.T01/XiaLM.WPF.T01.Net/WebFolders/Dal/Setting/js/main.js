var isRunning = false;
var connection = $.hubConnection("http://localhost:15689");
var contosoChatHubProxy = connection.createHubProxy('SettingHub'); //创建连接器
var contosoChatHubProxy2 = connection.createHubProxy('ChargeHub'); //创建连接器
var contosoChatHubProxy3 = connection.createHubProxy('IndexViewHub'); //创建连接器
var set;
var Id, IsChat, PhizStyle, ServerIP;
var ServerPort, AutoCharg, LowPower, ExternalName, BeginParams;
var Pattern, ForwardTerminal, AftTerminal, SystemVolume, Speed;
var Pitch, VoiceName, StartCruise, CruiseTime, Pas, ManualTelephone, IsCruiseInterrupt, RouseDuration;
var IsRockArm, RockArmInterval, IsSelfIntroduction, RockArmStr,IsRadarCharg,CameraAlarmBackground,CanTransfer;
var voicer = [];
var gold = 0;

$(function() {
	connection.start().done(function() {
		//连接成功
		isRunning = true;
		//得到配置信息
		getInfo();
		set.GetConfigInfo();
		setTimeout('set.GetVersionInfo()', 3000);
		parent.onload('Setting');
	});
})

set = {
	BackupMapInfo: function() {
		contosoChatHubProxy.invoke('BackupMapInfo');
	},
	BackupHotProblems: function() {
		contosoChatHubProxy.invoke('BackupHotProblems');
	},
	GetPhizStyles: function() {
		contosoChatHubProxy.invoke('GetPhizStyles').done(function(e) {
			var str = '';
			for(var i = 0; i < e.length; i++) {
				str += '<option class="bg_select" value="' + e[i].Id + '">' + e[i].Name + '</option>';
			}
			$('.BQFG').html(str);
			gold++
			set.goldcoller();
		})
	},
	GetLanguages: function() {
		contosoChatHubProxy.invoke('GetLanguages').done(function(e) {
			var str = '';
			for(var i = 0; i < e.length; i++) {
				str += '<option class="bg_select" value="' + e[i] + '">' + e[i] + '</option>';
			}
			$('.YZ').html(str);
			gold++
			set.goldcoller();
		})
	},
	GetVoiceNames: function() {
		contosoChatHubProxy.invoke('GetVoiceNames').done(function(e) {
			var str = '';
			voicer = e;
			for(var i = 0; i < e.length; i++) {
				if(BeginParams == e[i].Language) {
					str += '<option class="bg_select" value="' + e[i].Param + '">' + e[i].Name + '(' + e[i].Language + ')</option>';
				}
			}
			$('.SYZL').html(str);
			gold++
			set.goldcoller();
		})
	},
	GetConfigInfo: function() {
		contosoChatHubProxy.invoke('GetConfigInfo').done(function(e) {
			gold = 0;
			Id = e.Id;
			IsChat = e.IsChat;
			PhizStyle = e.PhizStyle;
			ServerIP = e.ServerIP;
			ServerPort = e.ServerPort
			AutoCharg = e.AutoCharg;
			LowPower = e.LowPower;
			ExternalName = e.ExternalName;
			BeginParams = e.BeginParams;
			Pattern = e.Pattern;
			ForwardTerminal = e.ForwardTerminal;
			AftTerminal = e.AftTerminal;
			SystemVolume = e.SystemVolume;
			Speed = e.Speed;
			Pitch = e.Pitch;
			VoiceName = e.VoiceName;
			StartCruise = e.StartCruise;
			CruiseTime = e.CruiseTime;
			Pas = e.Password;
			ManualTelephone = e.ManualTelephone;
			IsCruiseInterrupt = e.IsCruiseInterrupt;
			RouseDuration = e.RouseDuration;
			IsRockArm = e.IsRockArm;
			CameraAlarmBackground = e.CameraAlarmBackground;
			CanTransfer=e.CanTransfer;
			RockArmInterval = e.RockArmInterval;
			IsSelfIntroduction = e.IsSelfIntroduction;
			RockArmStr = e.RockArmStr;
			IsRadarCharg=e.IsRadarCharg;
			set.GetPhizStyles();
			set.GetLanguages();
			set.GetVoiceNames();
			gold++
			set.goldcoller();

			setInfo(e);
		})
	},
	goldcoller: function() {
		if(gold > 3) {
			set.Updateinfo();
		}
	},
	Updateinfo: function() {
		set.GetByID(Id);
		var IsChatbool = IsChat ? IsChatbool = "1" : IsChatbool = "";
		var IsSelfIntroductionbool = IsSelfIntroduction ? IsSelfIntroductionbool = "1" : IsSelfIntroductionbool = "";
		var AutoChargbool = AutoCharg ? AutoChargbool = "1" : AutoChargbool = "";
		var StartCruisebool = StartCruise ? StartCruisebool = "1" : StartCruisebool = "";
		var IsCruiseInterruptbool = IsCruiseInterrupt ? IsCruiseInterruptbool = "1" : IsCruiseInterruptbool = "";
		var IsRockArmbool = IsRockArm ? IsRockArmbool = "1" : IsRockArmbool = "";
		var CameraAlarmBackgroundbool = CameraAlarmBackground ? CameraAlarmBackgroundbool = "1" : CameraAlarmBackgroundbool = "";
		var CanTransferbool = CanTransfer ? CanTransferbool = "1" : CanTransferbool = "";
		var IsRadarChargbool= IsRadarCharg ? IsRadarChargbool = "1" : IsRadarChargbool = "";
		$("#YS").html('<input type="range" min="0" max="100" value=' + Speed + ' class="rangestyle" onchange="Speed=$(this).val();set.SetConfigInfo();">');
		$("#YL").html('<input type="range" min="0" max="100" value=' + SystemVolume + ' class="rangestyle"  onchange="SystemVolume=$(this).val();set.SetConfigInfo();">')
		$("#YG").html('<input type="range" min="0" max="100" value=' + Pitch + ' class="rangestyle" onchange="Pitch=$(this).val();set.SetConfigInfo();">')
		XLhtml();
		$(".XL").find("option[value=" + IsChatbool + "]").attr("selected", "selected");
		$(".BQFG").find("option[value=" + PhizStyle + "]").attr("selected", "selected");
		$(".YZ").find("option[value=" + BeginParams + "]").attr("selected", "selected");
		$(".SYZL").find("option[value=" + VoiceName + "]").attr("selected", "selected");
		YYMShtml();
		$(".YYMS").find("option[value=" + Pattern + "]").attr("selected", "selected");
		$(".textnet5").val(Pas);
		$(".textnet1").val(ForwardTerminal);
		$(".textnet2").val(AftTerminal);
		$(".textnet3").val(ServerIP);
		$(".textnet4").val(ServerPort);
		$(".textnet6").val(ManualTelephone);
		$(".ZDCD").find("option[value=" + AutoChargbool + "]").attr("selected", "selected");
		$(".DDLJJ").find("option[value=" + LowPower + "]").attr("selected", "selected");
		$(".QYXH").find("option[value=" + StartCruisebool + "]").attr("selected", "selected");
		$(".ZDTLSC").find("option[value=" + CruiseTime + "]").attr("selected", "selected");
		$(".YYJT").find("option[value=" + IsCruiseInterruptbool + "]").attr("selected", "selected");
		$(".HHSC").find("option[value=" + RouseDuration + "]").attr("selected", "selected");
		$(".SFBB").find("option[value=" + IsRockArmbool + "]").attr("selected", "selected");
		$(".QDTC").find("option[value=" + CameraAlarmBackgroundbool + "]").attr("selected", "selected");
		$(".KQTS").find("option[value=" + CanTransferbool + "]").attr("selected", "selected");
		ZZJShtml();
		$(".ZZJS").find("option[value=" + IsSelfIntroductionbool + "]").attr("selected", "selected");
		$(".YBBDX").find("option[value=" + RockArmStr + "]").attr("selected", "selected");
		$(".JJLD").find("option[value=" + IsRadarChargbool + "]").attr("selected", "selected");
		$(".textnet7").val(RockArmInterval);
		if(Pattern == '103') {
			$('.HHSChidden').css('display', 'block')
		}
		if(AutoCharg) {
			$('.JJLDChidden').css('display', 'block')
		}
	},
	SetConfigInfo: function(s) {
		var info = {
			Id: Id,
			IsChat: Boolean(IsChat),
			PhizStyle: PhizStyle,
			ServerIP: $('.textnet3').val(),
			ServerPort: $('.textnet4').val(),
			AutoCharg: Boolean(AutoCharg),
			LowPower: LowPower,
			xternalName: ExternalName,
			BeginParams: BeginParams,
			Pattern: Pattern,
			ForwardTerminal: $('.textnet1').val(),
			AftTerminal: $('.textnet2').val(),
			SystemVolume: SystemVolume,
			Speed: Speed,
			Pitch: Pitch,
			VoiceName: VoiceName,
			StartCruise: Boolean(StartCruise),
			CruiseTime: CruiseTime,
			Password: $('.textnet5').val(),
			ManualTelephone: $('.textnet6').val(),
			IsCruiseInterrupt: Boolean(IsCruiseInterrupt),
			RouseDuration: RouseDuration,
			RockArmStr: RockArmStr,
			IsRockArm: Boolean(IsRockArm),
			CameraAlarmBackground: Boolean(CameraAlarmBackground),
			CanTransfer:Boolean(CanTransfer),
			RockArmInterval: parseFloat(RockArmInterval),
			IsSelfIntroduction: Boolean(IsSelfIntroduction),
			IsRadarCharg: Boolean(IsRadarCharg),
		};
		contosoChatHubProxy.invoke('SetConfigInfo', info).done(function() {
			set.tip("已修改");
			setInfo(info)
		})
	},
	GetByID: function(s) {
		encode_data = s;
		$('#r-encode').empty();
		if(s != "" && s !== undefined && s !== null) {
			var qrcode = new QRCode(document.getElementById("r-encode"), {
				width: 230,
				height: 230
			});
			qrcode.makeCode(s);
		} else {
			$("#r-encode").html("无ID信息");
		}
	},
	Reboot: function() {
		contosoChatHubProxy.invoke('Reboot');
	},
	ShutDown: function() {
		contosoChatHubProxy.invoke('ShutDown');
	},
	Upgrade: function() {
		contosoChatHubProxy.invoke('Upgrade');
	},
	RegisteredRobot: function() {
		contosoChatHubProxy.invoke('RegisteredRobot').done(function(data) {
			if(data == true) {
				set.tip("注册成功");
				$('#zhuce').css('display', 'none');
			} else {
				set.tip("注册失败");
			}
		});
	},
	tip: function(s) {
		$("#tip").css("display", "block")
		$("#tip").text(s);
		setTimeout('$("#tip").css("display","none")', 4000)
	},
	jodo: function(e) {
		$('#jodo').css('display', 'block');
		$('#command').text(e);
		$('.btnss').empty();
		if(e == '重启') {
			$('.btnss').append('<button class="btn btn-danger btn-mine" onclick="set.Reboot(),set.closejodo()">确定</button><button class="btn btn-primary btn-mine" onclick="set.closejodo()">取消</button>')
		} else if(e == '关机') {
			$('.btnss').append('<button class="btn btn-danger btn-mine" onclick="set.ShutDown(),set.closejodo()">确定</button><button class="btn btn-primary btn-mine" onclick="set.closejodo()">取消</button>')
		} else if(e == '升级') {
			$('.btnss').append('<button class="btn btn-danger btn-mine" onclick="set.Upgrade(),set.closejodo()">确定</button><button class="btn btn-primary btn-mine" onclick="set.closejodo()">取消</button>')
		}
	},
	closejodo: function() {
		$('#jodo').css('display', 'none');
	},
	GetVersionInfo: function() {
		contosoChatHubProxy.invoke('GetVersionInfo').done(function(data) {
			$('.CurrentVersion').text(data.CurrentVersion);
			$('.NewestVersion').text(data.NewestVersion);
			if(data.IsUpdate === false) {
				$('.goup').css('display', 'none');
			} else {
				$('.goup').css('display', 'block');
			}
		});
	},
	changevoicer: function() {
		var boolswitch = true;
		var str;
		for(var i = 0; i < voicer.length; i++) {
			if(BeginParams == voicer[i].Language) {
				str += '<option class="bg_select" value="' + voicer[i].Param + '">' + voicer[i].Name + '(' + voicer[i].Language + ')</option>';
				if(boolswitch) {
					boolswitch = false;
					VoiceName = voicer[i].Param;
				}
			}
		}
		$('.SYZL').html(str);
		set.SetConfigInfo();
	},
	HHSChidden: function(e) {
		if(e == '103') {
			$('.HHSChidden').css('display', 'block')
		} else {
			$('.HHSChidden').css('display', 'none')
		}
		Pattern = e;
		set.SetConfigInfo();
	},
	JJLDChidden: function(e) {
		if(e == '1') {
			$('.JJLDChidden').css('display', 'block')
		} else {
			$('.JJLDChidden').css('display', 'none')
		}
		AutoCharg = e;
		//set.SetConfigInfo();
	}
}

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
//注册事件

contosoChatHubProxy.on("RefreshSettingPage", function(config) {
	//self.location.reload(true);
});

contosoChatHubProxy.on("NoticeBackupState", function(state) {
	if(state == 0) {
		set.tip("正在备份");
	} else if(state == 1) {
		set.tip("备份失败");
	} else if(state == 2) {
		set.tip("备份成功");
	} else {
		console.log('error')
	}
});

contosoChatHubProxy.on("NoticeBackupMapInfoState", function(b) {
	if(b) {
		set.tip("备份成功");
	} else {
		set.tip("备份失败");
	}
});

virtualkeyboard();

function reflash() {
	ForwardTerminal = $('.textnet1').val();
	AftTerminal = $('.textnet2').val();
	ServerIP = $('.textnet3').val();
	ServerPort = $('.textnet4').val();
	Pas = $('.textnet5').val();
	ManualTelephone = $('.textnet6').val();
	RockArmInterval = $('.textnet7').val();
	set.SetConfigInfo();
}

function setInfo(obj) {
	var obj=JSON.stringify(obj)
	localStorage.setItem("setinfo", obj);
}

function getInfo() {
	var objstr = localStorage.getItem("setinfo");
	if(objstr===null){
		return;
	}
	var e=JSON.parse(objstr)
	gold = 0;
	Id = e.Id;
	IsChat = e.IsChat;
	PhizStyle = e.PhizStyle;
	ServerIP = e.ServerIP;
	ServerPort = e.ServerPort
	AutoCharg = e.AutoCharg;
	LowPower = e.LowPower;
	ExternalName = e.ExternalName;
	BeginParams = e.BeginParams;
	Pattern = e.Pattern;
	ForwardTerminal = e.ForwardTerminal;
	AftTerminal = e.AftTerminal;
	SystemVolume = e.SystemVolume;
	Speed = e.Speed;
	Pitch = e.Pitch;
	VoiceName = e.VoiceName;
	StartCruise = e.StartCruise;
	CruiseTime = e.CruiseTime;
	Pas = e.Password;
	ManualTelephone = e.ManualTelephone;
	IsCruiseInterrupt = e.IsCruiseInterrupt;
	RouseDuration = e.RouseDuration;
	IsRockArm = e.IsRockArm;
	CameraAlarmBackground=e.CameraAlarmBackground;
	CanTransfer=e.CanTransfer;
	RockArmInterval = e.RockArmInterval;
	IsSelfIntroduction = e.IsSelfIntroduction;
	RockArmStr = e.RockArmStr;
	IsRadarCharg=e.IsRadarCharg;
	set.GetPhizStyles();
	set.GetLanguages();
	set.GetVoiceNames();
	gold++
	set.goldcoller();
}

function XLhtml(){
	str='<option class="bg_select" value="1">是</option><option class="bg_select" value="">否</option>'
	$('.XL').html(str)
}
function YYMShtml(){
	str='<option class="bg_select" value="101">命令模式</option>'+
	'<option class="bg_select" value="102">连续语音模式</option>'+
	'<option class="bg_select" value="103">唤醒连续</option>'
	$('.YYMS').html(str)
}
function ZZJShtml(){
	str='<option class="bg_select" value="1">是</option><option class="bg_select" value="">否</option>'
	$('.ZZJS').html(str)
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