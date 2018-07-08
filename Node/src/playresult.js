module.exports = function(app, connection) {

    /* Constant */
    const submit_insert = 'INSERT INTO PlayResult (MapId, UserId, Submission, NumOfGuards, GuardLocation, GameHash, Score) VALUES (?, ?, ?, ?, ?, ?, ?)'
    const submit_query_submission = 'SELECT MAX(Submission) AS Max FROM PlayResult WHERE MapId = ? AND UserId = ?;'

    app.post('/submit', function(req, res) {
    	console.log("Submit received");

    	var MapId = parseInt(req.body.MapId, 10);
    	var UserId = parseInt(req.body.UserId, 10);
    	var NumOfGuards = parseInt(req.body.NumOfGuards, 10);
    	var GuardLocation = String(req.body.GuardLocation);
    	var GameHash = String(req.body.GameHash);
    	var Score = parseFloat(req.body.Score);

    	if (isNaN(MapId) || isNaN(UserId) || isNaN(NumOfGuards) || isNaN(Score))
    		res.end("Error: invalid field");
    	else {
            submitGI(MapId, UserId, NumOfGuards, GuardLocation, GameHash, Score, function() {
    			res.end("Submitted");
    		});
    	}
    });

    var submitGI = function(MapId, UserId, NumOfGuards, GuardLocation, GameHash, Score, callback) {
    	connection.query(submit_query_submission, [MapId, UserId], function(err, rows, fields) {
    		if (err) {
    			console.log("query error: " + top_score_query);
    			console.log(err);

    			res.end("Error:" + err);
    			throw err;
    		}

    		var Submission = parseInt(rows[0].Max);

    		connection.query(submit_insert, [MapId, UserId, Submission + 1, NumOfGuards, GuardLocation, GameHash, Score], function(err, rows, fields) {
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

}
