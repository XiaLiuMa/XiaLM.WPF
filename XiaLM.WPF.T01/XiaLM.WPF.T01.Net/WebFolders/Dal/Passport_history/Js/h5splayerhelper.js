/** 
 * Check on iOS platform
*/
function H5siOS() {

  var iDevices = [
    'iPad Simulator',
    'iPhone Simulator',
    'iPod Simulator',
    'iPad',
    'iPhone',
    'iPod'
  ];

  if (!!navigator.platform) {
    while (iDevices.length) {
      if (navigator.platform === iDevices.pop()){ return true; }
    }
  }

  return false;
}

/** 
 *=================H5Player Create
 *
 */
 
function H5sPlayerCreate(conf) {
	var player;

	if (H5siOS())
	{
		player = new H5sPlayerHls(conf);
	}else
	{
		player = new H5sPlayerWS(conf);
	}
	return player;
}

 
/** 
 *=================H5Player Delete
 *
 */
function H5sPlayerDelete(player) {
	delete player;
	
	return true;
}

function GetURLParameter(sParam)
{
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) 
    {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0] == sParam) 
        {
            return sParameterName[1];
        }
    }
}


//my function
try{
	if(H5siOS() === true) {
		$('#h5sVideo1').prop("controls", true);
	}
	var conf1 = {
		videoid: 'h5sVideo1',
		protocol: window.location.protocol, //http: or https:
		host: '127.0.0.1:8089/', //localhost:8080
		rootpath: '', // '/'
		token: 'token1',
		hlsver: 'v1', //v1 is for ts, v2 is for fmp4
		session: 'c1782caf-b670-42d8-ba90-2244d0b0ee83' //session got from login
	};
	var v1 = H5sPlayerCreate(conf1);
	$('#h5sVideo1').get(0).play();
	if(v1 != null) {
		v1.disconnect();
		H5sPlayerDelete(v1);
		v1 = null;
	}
	v1 = H5sPlayerCreate(conf1);
	console.log(v1);
	v1.connect();
} catch(e) {
	console.log(e.message)
}
 

