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
