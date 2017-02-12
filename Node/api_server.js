/*jslint node: true*/
"use strict";
/**************************************************************/
/* Variables */
var new_bold_line_string = '============================================';
var new_line_string = '----------------------------------------------';
//var jar_path = '/Users/yunjoon_soh/Personal_Projects/whereToMeet/Java_workspace/MiddlePoint_API/jar_test.jar';
var jar_path = './jar_test.jar'
var server_ip_addr = '127.0.0.1';
//var server_ip_addr = '192.168.0.53';
var http = require("http");

var server = http.createServer(function (request, res) {
   // Send the HTTP header 
   // HTTP Status: 200 : OK
   // Content Type: text/plain
   res.writeHead(200, {'Content-Type': 'text/plain'});
   
   console.log('Request Received' + new_bold_line_string);
   console.log(request.url);
   console.log(new_line_string);

}).listen(8080, server_ip_addr);


var url = require('url');

var qs = require('querystring');

/**************************************************************/
/* Functions */
/**************************************************************/
console.log('Hello, World! Server running with Node.js');


server.on('request', function (request, res) {
   // the same kind of magic happens here!
   var url_parts = url.parse(request.url, true);
   var query = url.parse(request.url, true).query;

   if(url_parts.pathname === '/top_scores'){
		   res.end('{	"Scores": {		"low1": 123,		"low2": 234,		"low3": 345,		"high1": 987,		"high2": 876,		"high3": 765	},	"Guards": {		"g_low1": 3,		"g_low2": 3,		"g_low3": 3,		"g_high1": 5,		"g_high2": 4,		"g_high3": 4	}}');
   }

   else if(url_parts.pathname === '/submit'){
   }
   
//   if (url_parts.pathname === '/middlePoint') {
  //    var refined_query = JSON.stringify(query).replace(/"/g, "'");
//
  //    console.log("Following query was received: " + refined_query);
//
  //    var exec = require('child_process').exec;
    //  var cmd = 'java -jar ' + jar_path + ' "' + refined_query + '"';
///
   //   console.log("Try running the following cmd call: " + cmd);
//
  //    exec(cmd, function (error, stdout, stderr) {
    //     console.log(new_line_string + ' stdout:');
      //   console.log(stdout);
        // console.log(new_line_string + ' stderr:');
      //   console.log(stderr);
         
       //  if (stdout.toLowerCase().indexOf("error") > -1) {
        //    res.writeHead(404, {"Content-Type": "text/plain"});
       //     res.end('[{"Query Result Type":"Error"}]');
       //  } else {
        //    res.end(stdout);
        // }
    //  });
   else {
		console.log('\'' + url_parts.pathname + '\' did not match any API...');
		res.end('Error: Did not match any API.');
   }
});

// Console will print the message
console.log(new_line_string);
console.log('Server running at ' + server_ip_addr + ':8080/');
console.log(new_line_string);

//server.on('/addStation', function (req, res) {
//   console.log("addStation called");
//   console.log(req.stationName);
//
//   res.end("req.stationName = " + parts.stationName);
//});
