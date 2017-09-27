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

import { Prompt, IPromptFeatures, IPromptContext, IPromptOptions } from './Prompt';
import * as chrono from 'chrono-node';
import * as consts from '../consts';

export interface IPromptMultiTypesOptions extends IPromptOptions {
    allowAttachment?: boolean;
    allowDateTime?: boolean;
    allowNumber?: boolean;
    allowText?: boolean;
}

export interface IPromptMultiTypeFeatures extends IPromptFeatures {
    recognizeScore?: number;
}

export class PromptMultiTypes extends Prompt<IPromptMultiTypeFeatures> {
    constructor(features?: IPromptMultiTypeFeatures) {
        super({
            defaultRetryPrompt: 'default_text',
            defaultRetryNamespace: consts.Library.system,
            recognizeScore: 0.5
        });
        this.updateFeatures(features);
        
        // Distinguish between the different supported types
        this.onRecognize((context, cb) => {
            // the options object holds which types are allowed
            let options: IPromptMultiTypesOptions = context.dialogData.options;
            
            var strAsDateTime = chrono.parseDate(context.message.text);      
            if (options.allowAttachment && context.message.attachments.length === 1) {
                cb(null, 1.0, { "type": "attachment", "data": context.message.attachments[0] });
            }
            else if (options.allowDateTime && strAsDateTime) {
                cb(null, 1.0, { "type": "date", "data": strAsDateTime });
            }
            else if (options.allowNumber && !isNaN(Number(context.message.text))) {
                cb(null, 1.0, { "type": "number", "data": Number(context.message.text) });
            }
            else if (options.allowText && context.message.text.length > 0) {
                cb(null, 1.0, { "type": "text", "data": context.message.text });
            } else {
                // Failed to identify the right input method
                cb(null, 0.0);
            }
        });
    }
}