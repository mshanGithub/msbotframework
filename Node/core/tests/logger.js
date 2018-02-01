process.env.NODE_DEBUG = 'botbuilder';
var assert = require('assert');
var logger = require('../lib/logger');
var Channel = require('../lib/Channel');

describe('the logger', function () {
    beforeEach(function () {
        Channel.channels.emulator = '';
    });
    it('should call the appropriate functions on the LogListener', function () {
        var calls = {};
        var logListener = {
            error: function (message) {
                calls.error = message;
            },

            warn: function (message) {
                calls.warn = message;
            },

            info: function (message) {
                calls.info = message;
            },

            debug: function (message) {
                calls.debug = message;
            },

            trace: function (message) {
                calls.trace = message;
            }
        };
        logger.listeners.add(logListener);

        logger.error('This is my %s', [ 'error' ]);
        assert.equal(calls.error, 'ERROR: ' + 'This is my error');

        logger.warn(null, 'This is my %s', [ 'warning' ]);
        assert.equal(calls.warn, 'WARN: ' + 'This is my warning');

        logger.info(null, 'This is my %s', [ 'info' ]);
        assert.equal(calls.info, 'This is my info');

        logger.trace('This is my %s', [ 'trace' ]);
        assert.equal(calls.trace, 'This is my trace');

        logger.debug('This is my %s', [ 'debug' ]);
        assert.equal(calls.debug, 'This is my debug');
    });
});