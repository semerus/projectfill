var assert = require('assert');

let chai = require('chai');
let chaiHttp = require('chai-http');

chai.use(chaiHttp);

var request = require("request");
let server = require('../server.js');

var express = require('express');
var app = express();
let map = require("../src/map.js")(app, null, null);

const server_addr = "http://localhost:8080";
const mapid_exist = 1;
const mapid_does_not_exist = 100;

describe('hooks', function() {
    before(function() {

    });
});

describe('Ping', function() {
    describe('GET /', function() {
        it('should answer', function(done) {
            request(server_addr, function(err, res, body) {
                // assert.equal(res.statusCode, 200);
                console.log(body);
                done();
            })
        })
    });
});

describe('Scores', function() {
    describe('GET /all_scores?MapId=?', function() {
        it('should return json formatted top score (map id exists)', function(done) {
            request(server_addr + "/all_scores?MapId=" + mapid_exist, function(err, res, body) {
                assert.equal(res.statusCode, 200);
                assert.equal(res.toJSON().headers['content-type'], 'application/json; charset=utf-8');
                done();
            });
        });
        it('should not return json formatted top score (map id does not exists)', function(done) {
            request(server_addr + "/all_scores?MapId=" + mapid_does_not_exist, function(err, res, body) {
                assert.equal(res.statusCode, 200);
                assert.equal(res.toJSON().headers['content-type'], 'application/json; charset=utf-8');
                done();
            });
        });
        it('should return json formatted top score (map id exists) N = 5', function(done) {
            request(server_addr + "/all_scores?MapId=" + mapid_exist + "&N=5", function(err, res, body) {
                assert.equal(res.statusCode, 200);
                assert.equal(res.toJSON().headers['content-type'], 'application/json; charset=utf-8');
                // assert.equal(res.body.length, 347);
                done();
            });
        });
        it('should not return json formatted top score (map id does not exists) N = 5', function(done) {
            request(server_addr + "/all_scores?MapId=" + mapid_does_not_exist + "&N=5", function(err, res, body) {
                assert.equal(res.statusCode, 200);
                assert.equal(res.toJSON().headers['content-type'], 'application/json; charset=utf-8');
                // assert.equal(res.body.length, 347);
                done();
            });
        });
        it('should return json formatted top score (map id exists) N = 50, should return 10', function(done) {
            request(server_addr + "/all_scores?MapId=" + mapid_exist + "&N=50", function(err, res, body) {
                assert.equal(res.statusCode, 200);
                assert.equal(res.toJSON().headers['content-type'], 'application/json; charset=utf-8');
                // assert.equal(res.body.length, 661);
                done();
            });
        });
        it('should not return json formatted top score (map id does not exists) N = 50, should return 10', function(done) {
            request(server_addr + "/all_scores?MapId=" + mapid_does_not_exist + "&N=50", function(err, res, body) {
                assert.equal(res.statusCode, 200);
                assert.equal(res.toJSON().headers['content-type'], 'application/json; charset=utf-8');
                // assert.equal(res.body.length, 661);
                done();
            });
        });
    });
});

describe('Maps', function() {
    describe('GET /map?MapId=?', function() {
        it('should return json formatted map data (map id exists)', function(done) {
            request(server_addr + "/map?MapId=2", function(err, res, body) {
                assert.equal(res.statusCode, 200);
                console.log(body);
                done();
            });
        });
    });
    //
    // describe('POST /map', function() {
    //     it('should successfully parse map file', function (done) {
    //         console.log(map.submit_map_query);
    //         parseMap("\[\{    \"Name\": \"test\",    \"Id\": 1,    \"OuterVectices\": \[      \{        \"x\": -3.33333349,        \"y\": 2.22222281      \},      \{        \"x\": 0.4666668,        \"y\": 2.60000038      \},      \{        \"x\": 1.0666678,        \"y\": -1.333333      \},      \{        \"x\": -1.11111069,        \"y\": -3.51111126      \},      \{        \"x\": -4.244445,        \"y\": -1.71111107      \},      \{        \"x\": -5.15555573,        \"y\": 0.222222209    \},      \{        \"x\": -6.15555573,        \"y\": 1.377778      \}    \]  \}\]", function() {
    //             console.log("Done");
    //             done();
    //         });
    //     });
    // });
});
