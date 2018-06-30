module.exports = function(app, connection) {

    /* Constant */
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

}
