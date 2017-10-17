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

import { Prompt, IPromptFeatures, IPromptContext, IPromptOptions } from './Prompt'
import { PromptRecognizers } from './PromptRecognizers';
import { IRecognizeContext } from './IntentRecognizer';
import { IPromptNumberOptions } from './PromptNumber';
import { IPromptAttachmentOptions, PromptAttachment } from './PromptAttachment';
import * as consts from '../consts';

export interface IPromptMultiTypesOptions extends IPromptOptions {
    disallowAttachment?: boolean;
    disallowTime?: boolean;
    disallowNumber?: boolean;
    disallowText?: boolean;
    numberOptions?: IPromptNumberOptions;
    timeOptions?: IPromptOptions;
    attachmentOptions?: IPromptAttachmentOptions;
}

export interface IPromptMultiTypeFeatures extends IPromptFeatures {
    recognizeScore?: number;
}

interface IRecognitionResult {
    score: number;
    entity: any;
}

export class PromptMultiTypes extends Prompt<IPromptMultiTypeFeatures> {
    constructor(features?: IPromptMultiTypeFeatures) {
        super({
            defaultRetryPrompt: 'default_text',
            defaultRetryNamespace: consts.Library.system,
            recognizeScore: 0.5
        });
        this.updateFeatures(features);

        // Distinguish between the different supported types using the existing input types' recognizers
        this.onRecognize((context, cb) => {
            let options: IPromptMultiTypesOptions = context.dialogData.options;

            var entities = <IEntity<any>[]>[];
            if (!options.disallowTime) {
                var dateEntities = <IEntity<any>[]>this.recognizeTime(context, options.timeOptions);
                entities = entities.concat(dateEntities);
            }
            if (!options.disallowNumber) {
                var numberEntities = <IEntity<any>[]>this.recognizeNumber(context, options.numberOptions);
                entities = entities.concat(numberEntities);
            }
            if (!options.disallowAttachment) {
                var attachmentEntities = <IEntity<any>[]>this.recognizeAttachment(context, options.attachmentOptions);
                entities = entities.concat(attachmentEntities);
            }

            let top = PromptRecognizers.findTopEntity(entities);
            if (top) {
                // if any of the recognizers returned a valud result, we take it
                if (top.type === 'chrono.duration') {
                    cb(null, top.score, { "type": top.type, "data": top });
                } else {
                    cb(null, top.score, { "type": top.type, "data": top.entity });
                }
            } else {
                // otherwise, it is either a text or not a valid input
                if (!options.disallowText && context.message.text.length > 0) {
                    cb(null, 1.0, { "type": "text", "data": context.message.text });
                } else {
                    cb(null, 0.0);
                }
            }
        });
    }

    private recognizeNumber(context: IRecognizeContext, options?: IPromptNumberOptions): IEntity<number>[] {
        return PromptRecognizers.recognizeNumbers(context, options);
    }

    private recognizeTime(context: IRecognizeContext, options?: IPromptOptions): IEntity<string>[] {
        return PromptRecognizers.recognizeTimes(context, options);
    }

    private recognizeAttachment(context: IRecognizeContext, options?: IPromptAttachmentOptions): IEntity<IAttachment[]>[] {
        let allowAll = true;
        let contentTypes = <string[]>[];
        if (options) {
            contentTypes = typeof options.contentTypes == 'string' ? options.contentTypes.split('|') : options.contentTypes;
            allowAll = false;
        }

        let attachments: IAttachment[] = [];
        context.message.attachments.forEach((value) => {
            if (allowAll || PromptAttachment.allowed(value, contentTypes)) {
                console.log('adding ## attach ## ' + value)
                attachments.push(value);
            }
        });

        var entities = [];
        if (attachments.length > 0) {
            entities.push({ entity: attachments, type: 'attachments', score: 1.0 });
        }
        return entities;
    }
}