/*-----------------------------------------------------------------------------
A simple "Hello World" bot that can be run from a console window.

# RUN THE BOT:

    Run the bot from the command line using "node app.js" and then type 
    "hello" to wake the bot up.

-----------------------------------------------------------------------------*/

var builder = require('../../core/');

var connector = new builder.RestfulConnector().listen();
var bot = new builder.UniversalBot(connector);
var intents = new builder.IntentDialog();

bot.dialog('/', intents);

intents.matches(/^simon says/i, [
    function (session) {
        builder.Prompts.text(session, "What do you want simon to say?");
    },
    function (session, results) {
        session.send("Simon says... %s", results.response);
    }
]);