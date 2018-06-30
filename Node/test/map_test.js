var assert = require('assert');

let chai = require('chai');
let chaiHttp = require('chai-http');

chai.use(chaiHttp);

var request = require("request");
let server = require('../server.js');

var express = require('express');
var app = express();
let map = require("../src/map.js")(app, null, null);

var sha256 = require("sha256")

const server_addr = "http://localhost:8080";
const mapid_exist = 1;
const mapid_does_not_exist = 100;
const valid_map = "{\"GName\" : \"test_Mecca\", \"CName\" : \"Random\",\"OuterVertices\" : [{\"x\": 4.0,\"y\": 5.0},{\"x\": -1.0,\"y\": 7.0},{\"x\": -3.0,\"y\": 3.0},{\"x\": -1.0,\"y\": -1.0},{\"x\": 3.0,\"y\": 1.0},{\"x\": 4.0,\"y\": 5.0}],\"holes\" : [{ \"innerVertices\" : [{\"x\": 0.0,\"y\": 3.0},{\"x\": -1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 5.0},{\"x\": 1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 3.0}]}],\"lineColor\" : {\"r\": 0.392,\"g\": 0.047, \"b\": 0.012, \"a\": 1.0},\"backgroundColor\" : {\"r\": 0.706,\"g\": 0.765, \"b\": 1.000, \"a\": 1.0},\"guardBasicColor\" : {\"r\": 0.492,\"g\": 0.147, \"b\": 0.112, \"a\": 1.0},\"guardSelectedColor\" : {\"r\": 0.592,\"g\": 0.247, \"b\": 0.212, \"a\": 1.0},\"vgColor\" : {\"r\": 0.692,\"g\": 0.347, \"b\": 0.312, \"a\": 0.2}}"
const invalid_map_no_GName = "{\"CName\" : \"Random\",\"OuterVertices\" : [{\"x\": 4.0,\"y\": 5.0},{\"x\": -1.0,\"y\": 7.0},{\"x\": -3.0,\"y\": 3.0},{\"x\": -1.0,\"y\": -1.0},{\"x\": 3.0,\"y\": 1.0},{\"x\": 4.0,\"y\": 5.0}],\"holes\" : [{ \"innerVertices\" : [{\"x\": 0.0,\"y\": 3.0},{\"x\": -1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 5.0},{\"x\": 1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 3.0}]}],\"lineColor\" : {\"r\": 0.392,\"g\": 0.047, \"b\": 0.012, \"a\": 1.0},\"backgroundColor\" : {\"r\": 0.706,\"g\": 0.765, \"b\": 1.000, \"a\": 1.0},\"guardBasicColor\" : {\"r\": 0.492,\"g\": 0.147, \"b\": 0.112, \"a\": 1.0},\"guardSelectedColor\" : {\"r\": 0.592,\"g\": 0.247, \"b\": 0.212, \"a\": 1.0},\"vgColor\" : {\"r\": 0.692,\"g\": 0.347, \"b\": 0.312, \"a\": 0.2}}"
const invalid_map_no_CName = "{\"GName\" : \"test_Mecca\", \"OuterVertices\" : [{\"x\": 4.0,\"y\": 5.0},{\"x\": -1.0,\"y\": 7.0},{\"x\": -3.0,\"y\": 3.0},{\"x\": -1.0,\"y\": -1.0},{\"x\": 3.0,\"y\": 1.0},{\"x\": 4.0,\"y\": 5.0}],\"holes\" : [{ \"innerVertices\" : [{\"x\": 0.0,\"y\": 3.0},{\"x\": -1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 5.0},{\"x\": 1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 3.0}]}],\"lineColor\" : {\"r\": 0.392,\"g\": 0.047, \"b\": 0.012, \"a\": 1.0},\"backgroundColor\" : {\"r\": 0.706,\"g\": 0.765, \"b\": 1.000, \"a\": 1.0},\"guardBasicColor\" : {\"r\": 0.492,\"g\": 0.147, \"b\": 0.112, \"a\": 1.0},\"guardSelectedColor\" : {\"r\": 0.592,\"g\": 0.247, \"b\": 0.212, \"a\": 1.0},\"vgColor\" : {\"r\": 0.692,\"g\": 0.347, \"b\": 0.312, \"a\": 0.2}}"
// to be exact, following has typo for OuterVertices property
const invalid_map_no_OuterVertices = "{\"GName\" : \"test_Mecca\", \"CName\" : \"Random\",\"outer\" : [{\"x\": 4.0,\"y\": 5.0},{\"x\": -1.0,\"y\": 7.0},{\"x\": -3.0,\"y\": 3.0},{\"x\": -1.0,\"y\": -1.0},{\"x\": 3.0,\"y\": 1.0},{\"x\": 4.0,\"y\": 5.0}],\"holes\" : [{ \"innerVertices\" : [{\"x\": 0.0,\"y\": 3.0},{\"x\": -1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 5.0},{\"x\": 1.0,\"y\": 4.0},{\"x\": 0.0,\"y\": 3.0}]}],\"lineColor\" : {\"r\": 0.392,\"g\": 0.047, \"b\": 0.012, \"a\": 1.0},\"backgroundColor\" : {\"r\": 0.706,\"g\": 0.765, \"b\": 1.000, \"a\": 1.0},\"guardBasicColor\" : {\"r\": 0.492,\"g\": 0.147, \"b\": 0.112, \"a\": 1.0},\"guardSelectedColor\" : {\"r\": 0.592,\"g\": 0.247, \"b\": 0.212, \"a\": 1.0},\"vgColor\" : {\"r\": 0.692,\"g\": 0.347, \"b\": 0.312, \"a\": 0.2}}"

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

    describe('POST /map', function() {
        it('should successfully post map file', function (done) {
            hVal = sha256(valid_map);
            formData = {
                "UserId": 2,
                "MapHash": hVal,
                "MapFile": valid_map
            }
            request.post(
                {
                    url: server_addr + "/map",
                    form: formData
                },
                function (err, res, body) {
                    assert.equal(res.statusCode, 201);
                    assert.equal(body, "Submitted");
                    done();
                }
            );
        });
        it('should unsuccessfully post map file(hash not match)', function (done) {
            hVal = sha256(invalid_map_no_GName);
            formData = {
                "UserId": 2,
                "MapHash": hVal,
                "MapFile": valid_map
            }
            request.post(
                {
                    url: server_addr + "/map",
                    form: formData
                },
                function (err, res, body) {
                    assert.equal(res.statusCode, 400);
                    assert.equal(body, "Invalid MapFile");
                    done();
                }
            );
        });
        it('should unsuccessfully post map file(invalid_map_no_GName)', function (done) {
            hVal = sha256(invalid_map_no_GName);
            formData = {
                "UserId": 2,
                "MapHash": hVal,
                "MapFile": invalid_map_no_GName
            }
            request.post(
                {
                    url: server_addr + "/map",
                    form: formData
                },
                function (err, res, body) {
                    assert.equal(res.statusCode, 400);
                    assert.equal(body, "Invalid MapFile");
                    done();
                }
            );
        });
        it('should unsuccessfully post map file(invalid_map_no_CName)', function (done) {
            hVal = sha256(invalid_map_no_CName);
            formData = {
                "UserId": 2,
                "MapHash": hVal,
                "MapFile": invalid_map_no_CName
            }
            request.post(
                {
                    url: server_addr + "/map",
                    form: formData
                },
                function (err, res, body) {
                    assert.equal(res.statusCode, 400);
                    assert.equal(body, "Invalid MapFile");
                    done();
                }
            );
        });
        it('should unsuccessfully post map file(invalid_map_no_OuterVertices)', function (done) {
            hVal = sha256(invalid_map_no_OuterVertices);
            formData = {
                "UserId": 2,
                "MapHash": hVal,
                "MapFile": invalid_map_no_OuterVertices
            }
            request.post(
                {
                    url: server_addr + "/map",
                    form: formData
                },
                function (err, res, body) {
                    assert.equal(res.statusCode, 400);
                    assert.equal(body, "Invalid MapFile");
                    done();
                }
            );
        });
    });
});
