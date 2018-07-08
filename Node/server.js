/**************************************************************/
/* Variables */
var new_bold_line_string = '============================================';
var new_line_string = '----------------------------------------------';

var server_ip_addr = '127.0.0.1';
var server_port = '8080';

var express = require('express');
var app = express();

// BODY PARSER
var bodyParser = require('body-parser')
app.use(bodyParser.json()); // to support JSON-encoded bodies
app.use(bodyParser.urlencoded({ // to support URL-encoded bodies
	extended: true
}));

// MYSQL
var mysql = require('mysql');
var connection = mysql.createConnection({
	host: 'localhost',
	user: 'groot',
	password: '1234ABcd@',
	database: 'Fill'
});
connection.connect(function(err) {
	if(err) throw err;
	console.log('Database connected');
});

// Utlities
var sha256 = require('sha256');
const winston = require('winston');
const tsFormat = () => (new Date()).toLocaleTimeString();
const logger = winston.createLogger({
  transports: [
    new (winston.transports.Console)({
      timestamp: tsFormat
    })
  ]
});

// ASYNC
// Include the async package
// Make sure you add "async" to your package.json
async = require("async");

// Array to hold async tasks
var asyncTasks = [];

// Custom functions
var map = require('./src/map')(app, connection, sha256, logger);
var score = require('./src/score')(app, connection);
var playresult = require('./src/playresult')(app, connection);
var user = require('./src/user')(app, connection);

/**************************************************************/
// respond with "hello world" when a GET request is made to the homepage
app.get('/', function(req, res) {
	res.send('This is server for Project Fill');
});

app.listen(server_port, function() {
	console.log(new_line_string);
	console.log('Server running at ' + server_ip_addr + ':' + server_port + '/');
	console.log(new_line_string);
});
