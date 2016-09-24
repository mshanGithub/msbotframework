//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
//
// Microsoft Bot Framework: http://botframework.com
//
// Bot Builder SDK Github:
// https://github.com/Microsoft/BotBuilder
//
// Copyright (c) Microsoft Corporation
// All rights reserved.
//
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

import readline = require('readline');
import ub = require('./UniversalBot');
import mb = require('../Message');
import utils = require('../utils');
let restify = require('restify');    //let instead of import because there's no typings for restify

interface IOutMessageDictionary {
    [key: string]: Array<IMessage>;
};

export class RestfulConnector implements ub.IConnector {
    private handler: (events: IEvent[], cb?: (err: Error) => void) => void;
    private rl: readline.ReadLine;
    private replyCnt = 0;

    private server = restify.createServer();

    private outgoingMessages: IOutMessageDictionary;
    
    public listen(): this {
        this.outgoingMessages = {};

        this.startServer();
        
        this.rl = readline.createInterface({ input: process.stdin, output: process.stdout, terminal: false });
        this.rl.on('line', (line: string) => {
            this.replyCnt = 0;
            line = line || '';
            if (line.toLowerCase() == 'quit') {
                this.rl.close();
                process.exit();
            } else {
                this.processMessage(line, 'console');
            }
        });
        return this; 
    }

    private startServer() {
        
        //This is needed to support POST requests
        this.server.use(restify.bodyParser());

        this.server.pre(restify.CORS({
            credentials: false,
            headers: ['authorization']
        }));
        this.server.use(restify.fullResponse());

        this.server.get('/hello/:name', (req: any, res: any, next: any) => {
            res.send('hello ' + req.params.name);
            next(); 
        });

        //https://docs.botframework.com/en-us/restapi/directline/

        //Create a conversation
        this.server.post('/api/conversations', (req: any, res: any, next: any) => {
            //generate a conversationId;
            let r = this.generateRandomString();
            var responseObject = {
                conversationId: r,
                token: "",
                eTag: ""
            };

            //Initialize outbound messages
            this.outgoingMessages[r] = new Array<IMessage>();

            console.log('Started conversation %s', r);

            res.send(responseObject);
        });

        //Get messages in the conversation
        this.server.get('/api/conversations/:conversationid/messages', (req: any, res: any, next: any) => {
            let responseObj = <any>{};
            let item: IMessage;
            let conversationId = req.params.conversationid;

            try {
                responseObj.messages = new Array<any>();
                while(item = this.outgoingMessages[conversationId].pop()) {
                    responseObj.messages.push({
                        "conversationId": conversationId,
                        "created": (new Date()).toISOString(),
                        "text": item.text
                    });
                }
               this.outgoingMessages[conversationId] = new Array<IMessage>();
            } catch(err) {
                console.error('Error sending a message')
            }

            responseObj.eTag = this.generateRandomString();

            res.send(responseObj);
        });

        //Send a message
        this.server.post('/api/conversations/:conversationid/messages', (req: any, res: any, next: any) => {
            res.header("Access-Control-Allow-Origin", "*");

            console.log('User said: ' + req.body.text);
            try {
                this.processMessage(req.body.text, req.params.conversationid);    //response will be sent in here
            } catch(err) {
                console.error('Error sending a message')
            }
            
            res.send(204);
        });

        //Generate a token for a new conversation (no-op)
        this.server.post('/api/tokens/conversation', (req: any, res: any, next: any) => {
            let token = this.generateRandomString();

            res.send(token);
        });

        //Renew a token
        this.server.get('/api/tokens/:conversationId/renew', (req: any, res: any, next: any) => {
             res.send('no-op');
        });

        this.server.listen(3978, () => {
            console.log('%s listening to %s', this.server.name, this.server.url); 
        });
    }

    private generateRandomString() {
        return Math.random().toString(36).replace(/[^a-z]+/g, '');
    }

    public processMessage(line: string, conversationId: string): this {
        if (this.handler) {
            // TODO: Add some sort of logic to support attachment uploads.
            var msg = new mb.Message()
                .address({
                    channelId: 'rest',
                    user: { id: 'user', name: 'User1' },
                    bot: { id: 'bot', name: 'Bot' },
                    conversation: { id: conversationId }
                })
                .timestamp()
                .text(line);
            this.handler([msg.toMessage()]);
        }
        return this;
    }
    
    public onEvent(handler: (events: IEvent[], cb?: (err: Error) => void) => void): void {
        this.handler = handler;
    }
    
    public send(messages: IMessage[], done: (err: Error) => void): void {
        for (var i = 0; i < messages.length; i++ ){
            if (this.replyCnt++ > 0) {
                console.log();
            }
            var msg = messages[i];
            let conversationId = msg.address.conversation.id;
            this.outgoingMessages[conversationId].push(msg);
            console.log("Server has a message for conversation %s", conversationId);

            if (msg.attachments && msg.attachments.length > 0) {
                console.warn('The bot wants to send attachments, but that is not supported');
            }
        }        

        done(null);
    }

    public startConversation(address: IAddress, cb: (err: Error, address?: IAddress) => void): void {
        var adr = utils.clone(address);
        adr.conversation = { id: 'Convo1' };
        cb(null, adr);
    }
}