function exitHandler(options, exitCode) {
    var hrstart = options.hrstart, 
        hrend = process.hrtime(hrstart),
        total = options.total;
    var one_second = 10 ** 9
    var seconds = hrend[0] + hrend[1]/one_second
    avg = total / seconds
    console.log(`Total ${total} calls done in ${seconds} seconds, average ${avg} calls / second `)
    console.log(exitCode);
    //if (options.exit) process.exit();
}

var Client = require('node-rest-client').Client;
  
var myArgs = process.argv.slice(2);
var url = myArgs[0];
var total = parseInt(myArgs[1]);

var options_auth = { user: "folaris", password: "folaris" };
var client = new Client(options_auth);

var args = {
    data: { cmd: "Get-Date" },
    headers: { "Content-Type": "application/json" }
};

var hrstart = process.hrtime()
for (var i=0; i<total; i ++){
  client.post(url, args, function (data, response) {
    //console.log(data); console.log(response);
    });
}

process.on('exit', exitHandler.bind(null,{total:total, hrstart: hrstart}));