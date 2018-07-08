var assert = require('assert');

let chai = require('chai');
let chaiHttp = require('chai-http');

chai.use(chaiHttp);

var request = require("request");
let server = require('../server.js');

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
                assert.equal(res.statusCode, 200);
                assert.equal(body, "This is server for Project Fill");
                done();
            })
        })
    });
});
