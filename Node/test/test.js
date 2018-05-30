var assert = require('assert');

let chai = require('chai');
let chaiHttp = require('chai-http');

chai.use(chaiHttp);

var request = require("request");
let server = require('../Node.js')

const server_addr = "http://localhost:8080";
const gameid_exist = 1;
const gameid_does_not_exist = 100;

describe('hooks', function() {
    before(function() {

    });
});

describe('Ping', function() {
    describe('GET /', function() {
        it('should answer', function(done) {
            request(server_addr, function(err, res, body) {
                assert.equal(res.statusCode, 200);
                done();
            })
        })
    });
});

describe('Scores', function() {
    describe('GET /top_scores?GameId=?', function() {
        it('should return json formatted top score (game id exists)', function(done) {
            request(server_addr + "/top_scores?GameId=" + gameid_exist, function(err, res, body) {
                assert.equal(res.statusCode, 200);
                done();
            });
        });
        it('should not return json formatted top score (game id does not exists)', function(done) {
            request(server_addr + "/top_scores?GameId=" + gameid_does_not_exist, function(err, res, body) {
                assert.equal(res.statusCode, 200);
                done();
            });
        });
    });
});
