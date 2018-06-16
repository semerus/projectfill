module.exports = function(app, connection) {

    /* Constant */
    const submit_map_query = "INSERT INTO Game (GName, CName, NumOfVertices, NumOfHoles, LineColorRGB, LineColorA, BGColorRGB, BGColoarA, GuardBasicColorRGB, GuardBasicColorA, GuardSelectedColorRGB, GuardSelectedColorA, VGColorRGB, VGColorA) WHERE (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? );";
    const query_map_query = "SELECT * FROM Game WHERE GameId = ?;";

    app.get('/map', function(req, res) {
    	console.log("Map query requested");

    	var GameId = parseInt(req.query.GameId, 10);

    	if (isNaN(GameId)) {
    		res.end("Error: invalid field");
    	} else {
            queryMap(GameId, function(mapInfo) {
                console.log("Map query done");
                res.end(JSON.stringify(mapInfo));
            });
    	}
    });

    app.post('/map', function(req, res) {
    	console.log("Map submission received");

    	var GameFile = String(req.body.GameFile);
    	var JsonObj = JSON.parse(GameFile);

    	res.end("Submitted");
    });

    /* Function */
    var submitMap = function(UserId, GameHash, FilePath, callback) {
    	connection.query(submit_map_query, [UserId, GameHash, FilePath], function(err, rows, fields) {
    		if (err) {
    			console.log("query error: " + top_score_query);
    			console.g(err);

    			res.end("Error:" + err);
    			throw err;
    		}

    		var Submission = parseInt(rows[0].Max);
    	});
    }

    var queryMap = function(GameId, callback) {

        connection.query(query_map_query, [GameId], function(err, rows, fields) {
            if (err) {
    			console.log("query error: " + top_score_query);
    			console.log(err);

    			res.end("Error:" + err);
    			throw err;
    		}

            var mapInfo = [];
            for (var i = 0; i < rows.length; i++) {
                mapInfo.push(rows[i]);
            }
            callback(mapInfo);
        });
    }
}
