module.exports = function(app, connection) {

    /* Constant */
    const checkUser_Email = 'SELECT * FROM User WHERE Email=?';
    const checkUser = 'SELECT * FROM User WHERE DeviceId=?;';
    const updateUser = 'UPDATE User SET Email=?, Pwd=?, UserName=? WHERE DeviceId=?;';
    const updateUser_Email = 'UPDATE User SET DeviceId=?, Pwd=?, UserName=? WHERE Email=?;';
    const registerUser = 'INSERT INTO User (DeviceId, Email, Pwd, UserName) WHERE (?, ?, ?, ?);';

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

    // app.post('/user/exists?UserId=?', function(req, res) {
    //     var userId = parseInt(req.query.UserId, 10);
    //     console.log("UserId:" + req.query.UserId);
    //
    //     if(isNaN(userId) || userId <= 0) {
    //         res.status(404).end("Error: UserId is invalid!");
    //     } else {
    //
    //         res.status(200).end("")
    //     }
    // });

    app.post('/user', function(req, res) {
        var DeviceId = parseInt(req.body.DeviceId, 10);
        var Email = req.body.Email;
        var Pwd = req.body.Pwd;
        var UserName = req.body.UserName;

        if (isNaN(DevideId)) {
            res.end("Error: invalid field");
        } else {
            submitUser(DeviceId, Email, Pwd, UserName, function() {
                res.end("Submitted");
            });
        }
    });

    var submitUser = function(DeviceId, Email, Pwd, UserName) {
        connection.query(registerUser, [DeviceId, Email, Pwd, UserName], function(err, rows, fields) {
            if(err) {
                console.log("query error: " + registerUser);
                console.log(err);

                res.end("Error:" + err);
                throw err;
            }

            callback();
        });
    }

    app.patch('/user', function(req, res) {
        var DeviceId = parseInt(req.body.DeviceId, 10);
        var Email = req.body.Email;
        var Pwd = req.body.Pwd;
        var UserName = req.body.UserName;

        connection.query(submitUser_Email, [Email], function(err, rows, fields) {
            if(rows[0] == null) {
                console.log("query failed: " + submitUser_Email + " with email=" + Email);
                console.log(err);

                res.end("No user by that email");
                throw err;
            } else {
                connection.query(updateUser_Email, [DeviceId, Pwd, UserName, Email], function(err, rows, fields) {
                    if (err) {
                        console.log("query error: " + updateUser_Email);
                        console.log(err);

                        res.end("Error:" + err);
                        throw err;
                    } else {
                        console.log("Update Successful!");

                        res.end("Update Successful!");
                    }
                });
            }
        });
    });
}
