module.exports = function(app, connection) {

    /* Constant */
    const top_score_query = 'SELECT * FROM PlayResult WHERE MapId = ? ORDER BY NumOfGuards, Score DESC;'
    const bot_score_query = 'SELECT * FROM PlayResult WHERE MapId = ? ORDER BY NumOfGuards, Score ASC;'
    const MAX_SCORES = 10;

    app.get('/all_scores', function(req, res) {

    	var numberMapId = parseInt(req.query.MapId, 10);
    	console.log("MapId:" + req.query.MapId);

    	var numberN = parseInt(req.query.N, 10);
    	console.log("N:" + req.query.N);

    	if(isNaN(numberN) || numberN <= 0) {
    		numberN = 3; // by default
    	}

    	if (isNaN(numberMapId)) {
    		res.status(404).end("Error: MapId is invalid!");
    	} else {

    		var ret_scores = '"Scores": {\n';
    		var ret_guards = '"Guards": {\n'

    		doQueries(numberMapId, ret_scores, ret_guards, numberN, function(err, ret_guards, ret_scores) {
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


    var doQueries = function(numberMapId, ret_scores, ret_guards, max_scores, callback) {
    	connection.query(top_score_query, [numberMapId], function(err, rows, fields) {
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
    		console.log('Print result of top_score_query for MapId=' + numberMapId);

    		connection.query(bot_score_query, [numberMapId], function(err, rows, fields) {
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
    			console.log('Print result of bot_score_query for MapId=' + numberMapId);

    			callback(null, ret_guards, ret_scores);
    		});
    	});
    }

}
