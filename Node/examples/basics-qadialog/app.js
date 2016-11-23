const builder = require('botbuilder');
const querystring = require('querystring');
const https = require('https');

const connector = new builder.ConsoleConnector();
const bot = new builder.UniversalBot(connector);
connector.listen();

const kbId = 'de59185d39ad479cb47108f5a6d1b615';
bot.dialog('/', new builder.QADialog(kbId));