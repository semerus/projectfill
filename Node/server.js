/**************************************************************/
/* Variables */
var new_bold_line_string = '============================================';
var new_line_string = '----------------------------------------------';

var server_ip_addr = '127.0.0.1';
var server_port = '8080';
const MAX_SCORES = 10;

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


// ASYNC
// Include the async package
// Make sure you add "async" to your package.json
async = require("async");

// Array to hold async tasks
var asyncTasks = [];

// Custom functions
var map = require('./src/map')(app, connection);

/**************************************************************/
// respond with "hello world" when a GET request is made to the homepage
app.get('/', function(req, res) {
	res.send('This is server for Project Fill');
});

// POST method route
const top_score_query = 'SELECT * FROM GameInfo WHERE GameId = ? ORDER BY NumOfGuards, Score DESC;'
const bot_score_query = 'SELECT * FROM GameInfo WHERE GameId = ? ORDER BY NumOfGuards, Score ASC;'

app.get('/all_scores', function(req, res) {

	var numberGameId = parseInt(req.query.GameId, 10);
	console.log("GameId:" + req.query.GameId);

	var numberN = parseInt(req.query.N, 10);
	console.log("N:" + req.query.N);

	if(isNaN(numberN) || numberN <= 0) {
		numberN = 3; // by default
	}

	if (isNaN(numberGameId)) {
		res.status(404).end("Error: GameId is invalid!");
	} else {

		var ret_scores = '"Scores": {\n';
		var ret_guards = '"Guards": {\n'

		doQueries(numberGameId, ret_scores, ret_guards, numberN, function(err, ret_guards, ret_scores) {
			if (err) {
				console.log("Query failed: " + err);
				res.status(500).end("Error: DB query error");
				return;
			}

			console.log(ret_guards)

			ret_guards = ret_guards.substring(0, ret_guards.length - 2) + "}";
			ret_scores = ret_scores.substring(0, ret_scores.length - 2) + "}";

			res.status(200).json('{\n' + ret_guards + ",\n" + ret_scores + "\n}");
		});
	}
});

const submit_insert = 'INSERT INTO GameInfo (GameId, UserId, Submission, NumOfGuards, GuardLocation, GameHash, Score) VALUES (?, ?, ?, ?, ?, ?, ?)'
const submit_query_submission = 'SELECT MAX(Submission) AS Max FROM GameInfo WHERE GameId = ? AND UserId = ?;'

app.post('/submit', function(req, res) {

	console.log("Submit received");

	var GameId = parseInt(req.body.GameId, 10);
	var UserId = parseInt(req.body.UserId, 10);
	var NumOfGuards = parseInt(req.body.NumOfGuards, 10);
	var GuardLocation = String(req.body.GuardLocation);
	var GameHash = String(req.body.GameHash);
	var Score = parseFloat(req.body.Score);

	if (isNaN(GameId) || isNaN(UserId) || isNaN(NumOfGuards) || isNaN(Score))
		res.end("Error: invalid field");
	else {
		submitGI(GameId, UserId, NumOfGuards, GuardLocation, GameHash, Score, function() {
			res.end("Submitted");
		});
	}
});

var doQueries = function(numberGameId, ret_scores, ret_guards, max_scores, callback) {
	connection.query(top_score_query, [numberGameId], function(err, rows, fields) {
		// connection.query(top_score_query, [1], function(err, rows, fields) {
		if (err) {
			console.log("query error: " + top_score_query);
			console.log(err);

			res.end("Error:" + err);
			throw err;
		}

		console.log("row length:" + rows.length);

		size = (max_scores > MAX_SCORES) ? MAX_SCORES : max_scores;

		for (var i = 0; i < size; i++) {
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

			for (var i = 0; i < size; i++) {
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

var submitGI = function(GameId, UserId, NumOfGuards, GuardLocation, GameHash, Score, callback) {
	connection.query(submit_query_submission, [GameId, UserId], function(err, rows, fields) {
		if (err) {
			console.log("query error: " + top_score_query);
			console.log(err);

			res.end("Error:" + err);
			throw err;
		}

		var Submission = parseInt(rows[0].Max);

		connection.query(submit_insert, [GameId, UserId, Submission + 1, NumOfGuards, GuardLocation, GameHash, Score], function(err, rows, fields) {
			if (err) {
				console.log("query error: " + top_score_query);
				console.log(err);

				res.end("Error:" + err);
				throw err;
			}

			callback();
		});
	});
}

const checkUser = 'SELECT * FROM User WHERE DeviceId = ?'
const updateUser = 'UPDATE User SET Email=?, Pwd=?, UserName=? WHERE DeviceId=?'
const registerUser = 'INSERT INTO User (DeviceId, Email, Pwd, UserName) WHERE (?, ?, ?, ?)'

var user = function(DeviceId, Email, Pwd, UserName) {
	connection.query(checkUser, [DeviceId], function(err, rows, fields) {
		console.log("CheckUser returned" + rows);
		// if the checkUser returns rows not null, then you have to update
		if (rows[0] != null) {
			connection.query(updateUser, [Email, Pwd, UserName, DeviceId], function(err, rows, fields) {
				if (err) {
					console.log("query error: " + top_score_query);
					console.log(err);

					res.end("Error:" + err);
					throw err;
				}

				console.log()
			});
		} else {
			connection.query(registerUser, [DeviceId, Email, Pwd, UserName], function(err, rows, fields) {
				if (err) {
					console.log("query error: " + top_score_query);
					console.log(err);

					res.end("Error:" + err);
					throw err;
				}
			});
		}
	});

}

app.listen(server_port, function() {
	console.log(new_line_string);
	console.log('Server running at ' + server_ip_addr + ':' + server_port + '/');
	console.log(new_line_string);
});
