/**************************************************************/
/* Variables */
var new_bold_line_string = '============================================';
var new_line_string = '----------------------------------------------';

var server_ip_addr = '127.0.0.1';
var server_port = '8080';
const MAX_SCORES = 3;

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
	user: 'root',
	password: '1234',
	database: 'Fill'
});
connection.connect();
console.log('Database connected');

// ASYNC
// Include the async package
// Make sure you add "async" to your package.json
async = require("async");

// Array to hold async tasks
var asyncTasks = [];


/**************************************************************/


// respond with "hello world" when a GET request is made to the homepage
app.get('/', function(req, res) {
	res.send('hello world');
});

// POST method route
const top_score_query = 'SELECT * FROM GameInfo WHERE GameId = ? ORDER BY NumOfGuards, Score DESC;'
const bot_score_query = 'SELECT * FROM GameInfo WHERE GameId = ? ORDER BY NumOfGuards, Score ASC;'

app.post('/top_scores', function(req, res) {

	console.log("GameId:" + req.body.GameId);
	var numberGameId = parseInt(req.body.GameId, 10);

	console.log(numberGameId);

	if (numberGameId == NaN) {
		res.end("Error: GameId is invalid!");
	} else {

		var ret_scores = '"Scores": {\n';
		var ret_guards = '"Guards": {\n'

		doQueries(numberGameId, ret_scores, ret_guards, function(err, ret_guards, ret_scores) {
			if (err) {
				console.log("Query failed: " + err);
				return;
			}

			ret_guards = ret_guards.substring(0, ret_guards.length - 2) + "}";
			ret_scores = ret_scores.substring(0, ret_scores.length - 2) + "}";

			res.end('{\n' + ret_guards + ",\n" + ret_scores + "\n}");
		});
	}
});

var doQueries = function(numberGameId, ret_scores, ret_guards, callback) {
	connection.query(top_score_query, [numberGameId], function(err, rows, fields) {
		// connection.query(top_score_query, [1], function(err, rows, fields) {
		if (err) {
			console.log("query error: " + top_score_query);
			console.log(err);

			res.end("Error:" + err);
			throw err;
		}

		console.log("row length:" + rows.length);

		for (var i = 0; i < MAX_SCORES; i++) {
			var gNum = (rows[i] != null) ? (rows[i].NumOfGuards) : 0;
			var sc = (rows[i] != null) ? (rows[i].Score) : 0;
			ret_guards += '"g_high' + (i + 1) + '": ' + gNum + ',\n';
			ret_scores += '"high' + (i + 1) + '": ' + sc + ',\n';
		}

		// DEBUG
		console.log('Print result of top_score_query for GameId=' + numberGameId);

		connection.query(bot_score_query, [numberGameId], function(err, rows, fields) {
			if (err) {
				console.log("query error: " + top_score_query);
				console.log(err);

				res.end("Error:" + err);
				throw err;
			}

			for (var i = 0; i < MAX_SCORES; i++) {
				var gNum = (rows[i] != null) ? (rows[i].NumOfGuards) : 0;
				var sc = (rows[i] != null) ? (rows[i].Score) : 0;
				ret_guards += '"g_low' + (i + 1) + '": ' + gNum + ',\n';
				ret_scores += '"low' + (i + 1) + '": ' + sc + ',\n';
			}

			// DEBUG
			console.log('Print result of bot_score_query for GameId=' + numberGameId);

			callback(null, ret_guards, ret_scores);
		});
	});
}

app.listen(server_port, function() {
	console.log(new_line_string);
	console.log('Server running at ' + server_ip_addr + ':' + server_port + '/');
	console.log(new_line_string);
});