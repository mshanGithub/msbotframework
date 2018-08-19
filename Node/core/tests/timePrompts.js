var assert = require('assert');
var builder = require('../');

describe('timePrompts', function () {
    this.timeout(5000);

    it('should recognize weekdays in german', function (done) {
        var connector = new builder.ConsoleConnector();
        var bot = new builder.UniversalBot(connector);
        bot.dialog('/', [
            function (session) {
                assert(session.message.text == 'start');
                builder.Prompts.time(session, 'enter date', { refDate: '2018-08-22', locale: 'de' });
            },
            function (session, results) {
                const date = builder.EntityRecognizer.resolveTime([results.response]);
                assert(date.getFullYear() === 2018);
                assert(date.getMonth() + 1 === 8);
                assert(date.getDate() === 20);
                done();
            },
        ]);
        bot.on('send', function (message) {
            assert(message.text == 'enter date');
            connector.processMessage('Montag');
        });
        connector.processMessage('start');
    });

    it('should recognize weekdays in german (forward date)', function (done) {
        var connector = new builder.ConsoleConnector();
        var bot = new builder.UniversalBot(connector);
        bot.dialog('/', [
            function (session) {
                assert(session.message.text == 'start');
                builder.Prompts.time(session, 'enter date', { refDate: '2018-08-22', forwardDate: true, locale: 'de' });
            },
            function (session, results) {
                const date = builder.EntityRecognizer.resolveTime([results.response]);
                assert(date.getFullYear() === 2018);
                assert(date.getMonth() + 1 === 8);
                assert(date.getDate() === 27);
                done();
            },
        ]);
        bot.on('send', function (message) {
            assert(message.text == 'enter date');
            connector.processMessage('Montag');
        });
        connector.processMessage('start');
    });
});