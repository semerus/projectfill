module.exports = function(app, connection, sha256) {

    /* Constant */
    const query_map_sql = "SELECT * FROM Map WHERE MapId = ?;";
    const submit_map_sql = "INSERT INTO Map (GName, CName, Creator, JsonFile) WHERE (?, ?, ?, ?);";

    app.get('/map', function(req, res) {
    	console.log("Map query requested");

    	var MapId = parseInt(req.query.MapId, 10);

    	if (isNaN(MapId)) {
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

        var UserId = parseInt(req.body.UserId, 10);
        var MapHash = String(req.body.MapHash);
    	var MapFile = String(req.body.MapFile);

    	validateMap(MapHash, MapFile, function(valid) {
            if(!valid) {
                res.end("Invalid GameFile");
            } else {
                submitMap(UserId, MapFile, function() {
                    res.end("Submitted");
                });
            }
        });
    });

    /* Function */
    var submitMap = function(UserId, MapFile, callback) {
        var J = JSON.parse(MapFile);
    	connection.query(submit_map_sql, [J.GName, J.CName, UserId, MapFile], function(err, rows, fields) {
    		if (err) {
    			console.log("query error: " + top_score_query);
    			console.log(err);

    			res.end("Error:" + err);
    			throw err;
    		}

            callback();
    	});
    };

    var queryMap = function(GameId, callback) {
        connection.query(query_map_sql, [GameId], function(err, rows, fields) {
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
    };

    var validateMap = function(MapHash, MapFile, callback) {
        var J = JSON.parse(MapFile);
        var hashVal = sha256(MapFile);
        if(isNaN(J.name) || isNaN(J.OuterVectices) || hashVal != MapHash) {
            callback(false);
        }
        callback(true);
    };
}
