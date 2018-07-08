module.exports = function(app, connection, sha256, logger) {

    /* Constant */
    const query_map_sql = "SELECT * FROM Map WHERE MapId = ?;";
    const submit_map_sql = "INSERT INTO Map (GName, CName, Creator, JsonFile) VALUES (?, ?, ?, ?);";

    app.get('/map', function(req, res) {
    	logger.info("/map GET: Map query requested");

    	var MapId = parseInt(req.query.MapId, 10);

    	if (isNaN(MapId)) {
            logger.warn("External Error: Invalid MapId");
    		res.status(400).end("Error: Invalid MapId");
    	} else {
            queryMap(MapId, function(success, mapInfo) {
                if(!success) {
                    logger.error("Internal Error: " + mapInfo);
                    res.status(500).end("Error: Internal Error");
                } else {
                    logger.info("Map query done");
                    res.status(200).end(JSON.stringify(mapInfo));
                }
            });
    	}
    });

    app.post('/map', function(req, res) {
    	logger.info("/map POST: Map submission received");

        var UserId = parseInt(req.body.UserId, 10);
        var MapHash = String(req.body.MapHash);
    	var MapFile = String(req.body.MapFile);

    	validateMap(MapHash, MapFile, function(valid) {
            if(!valid) {
                logger.warn("External Error: Invalid MapFile");
                res.status(400).end("Error: Invalid MapFile");
            } else {
                submitMap(UserId, MapFile, function(success, result) {
                    if(!success) {
                        logger.error("Internal Error: " + result);
                        res.status(500).end("Error: Internal Error");
                    } else {
                        logger.info("Map File successfully submitted");
                        res.status(201).end("Submitted");
                    }
                });
            }
        });
    });

    /* Function */
    var submitMap = function(UserId, MapFile, callback) {
        logger.debug("submitMap(" + UserId + ", " + MapFile + ") called");
        var J = JSON.parse(MapFile);
    	connection.query(submit_map_sql, [J.GName, J.CName, UserId, MapFile], function(err, rows, fields) {
    		if (err) {
    			logger.error("query error: " + submit_map_sql);
    			logger.error(err);

                callback(false, err);
    		}

            logger.debug("submitMap(" + UserId + ", " + MapFile + ") returns: " + rows);
            callback(true, rows);
    	});
    };

    var queryMap = function(MapId, callback) {
        logger.debug("queryMap(" + MapId + ") called");
        connection.query(query_map_sql, [MapId], function(err, rows, fields) {
            if (err) {
    			logger.error("query error: " + query_map_sql);
    			logger.error(err);

    			callback(false, err);
    		}

            var mapInfo = [];
            for (var i = 0; i < rows.length; i++) {
                mapInfo.push(rows[i]);
            }

            logger.debug("queryMap(" + MapId + ") returns: " + mapInfo);
            callback(true, mapInfo);
        });
    };

    var validateMap = function(MapHash, MapFile, callback) {
        logger.debug("validateMap(" + MapHash + ", " + MapFile + ") called");

        var J = JSON.parse(MapFile);
        var hashVal = sha256(MapFile);

        if(!J.hasOwnProperty("GName") || !J.hasOwnProperty("CName") ||! J.hasOwnProperty("OuterVertices")) {
            logger.warn("Missing Json Property(GName, CName, OuterVertices)=" + !J.hasOwnProperty("GName") + " " + !J.hasOwnProperty("CName")  + " " + ! J.hasOwnProperty("OuterVertices"));
            callback(false);
        }
        if(hashVal !== MapHash) {
            logger.warn("Hash does not match: expected=" + MapHash + ", actual=" + hashVal);
            callback(false);
        }

        logger.debug("validateMap(" + MapHash + ", " + MapFile + ") returns: true");
        callback(true);
    };
}
