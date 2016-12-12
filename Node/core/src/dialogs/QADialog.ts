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

/// <reference path="../Scripts/typings/node/node.d.ts" />

import https = require('https');
import querystring = require('querystring');
import { ResumeReason, IDialogResult } from './Dialog';
import { SimpleDialog } from './SimpleDialog';
import { Session } from '../Session';
import * as consts from '../consts';
import * as logger from '../logger';

export interface IDialogWaterfallStep {
    (session: Session, result?: any, skip?: (results?: IDialogResult<any>) => void): void;
}

export class QADialog extends SimpleDialog {
    constructor(private kbId: string) {
        super((session) => {
            const message = session.message.text;
            const urlEncodedMessage = querystring.escape(message);
            const kbId = this.kbId;
            const path = `/KBService.svc/GetAnswer?kbId=${kbId}&question=${urlEncodedMessage}`;

            const options = {
                path: path,
                host: 'qnaservice.cloudapp.net'
            };

            const req = https.request(options, (res) => {
                let data = '';
                res.on('data', (chunk: string) => data += chunk);
                res.on('end', () => {
                    let answer = JSON.parse(data).answer;
                    session.endDialog(answer);
                });
            });
            req.end();
        });
    }
}