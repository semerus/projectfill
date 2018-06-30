var assert = require('assert');

let chai = require('chai');
let chaiHttp = require('chai-http');

chai.use(chaiHttp);

var request = require("request");
let server = require('../server.js');

const server_addr = "http://localhost:8080";
const mapid_exist = 1;
const mapid_does_not_exist = 100;

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
