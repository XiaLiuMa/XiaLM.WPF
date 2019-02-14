var Follow = {};
var connection = $.hubConnection("http://localhost:5000");
var homeHub = connection.createHubProxy('homeHub'); //创建连接器

connection.start().done(function() {

});

Follow = {
    StartFollow: function() {
        contosoChatHubProxy.invoke('StartFollow').done(function() {})
    },
    SetThresholdValue: function () {
        var temperature = parseFloat($('.ZDWD').val());
        contosoChatHubProxy.invoke('SetThresholdValue', temperature).done(function (s) {

        })
    }
}

homeHub.on("FeedBackFaceContrastAlarm", function(obj) {
	
});

