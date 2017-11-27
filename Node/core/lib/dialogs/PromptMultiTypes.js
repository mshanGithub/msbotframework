"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var Prompt_1 = require("./Prompt");
var PromptRecognizers_1 = require("./PromptRecognizers");
var PromptAttachment_1 = require("./PromptAttachment");
var consts = require("../consts");
var PromptMultiTypes = (function (_super) {
    __extends(PromptMultiTypes, _super);
    function PromptMultiTypes(features) {
        var _this = _super.call(this, {
            defaultRetryPrompt: 'default_text',
            defaultRetryNamespace: consts.Library.system,
            recognizeScore: 0.5
        }) || this;
        _this.updateFeatures(features);
        _this.onRecognize(function (context, cb) {
            var options = context.dialogData.options;
            var entities = [];
            if (!options.disallowTime) {
                var dateEntities = _this.recognizeTime(context, options.timeOptions);
                entities = entities.concat(dateEntities);
            }
            if (!options.disallowNumber) {
                var numberEntities = _this.recognizeNumber(context, options.numberOptions);
                entities = entities.concat(numberEntities);
            }
            if (!options.disallowAttachment) {
                var attachmentEntities = _this.recognizeAttachment(context, options.attachmentOptions);
                entities = entities.concat(attachmentEntities);
            }
            var top = PromptRecognizers_1.PromptRecognizers.findTopEntity(entities);
            if (top) {
                if (top.type === 'chrono.duration') {
                    cb(null, top.score, { "type": top.type, "data": top });
                }
                else {
                    cb(null, top.score, { "type": top.type, "data": top.entity });
                }
            }
            else {
                if (!options.disallowText && context.message.text.length > 0) {
                    cb(null, 1.0, { "type": "text", "data": context.message.text });
                }
                else {
                    cb(null, 0.0);
                }
            }
        });
        return _this;
    }
    PromptMultiTypes.prototype.recognizeNumber = function (context, options) {
        return PromptRecognizers_1.PromptRecognizers.recognizeNumbers(context, options);
    };
    PromptMultiTypes.prototype.recognizeTime = function (context, options) {
        return PromptRecognizers_1.PromptRecognizers.recognizeTimes(context, options);
    };
    PromptMultiTypes.prototype.recognizeAttachment = function (context, options) {
        var allowAll = true;
        var contentTypes = [];
        if (options) {
            contentTypes = typeof options.contentTypes == 'string' ? options.contentTypes.split('|') : options.contentTypes;
            allowAll = false;
        }
        var attachments = [];
        context.message.attachments.forEach(function (value) {
            if (allowAll || PromptAttachment_1.PromptAttachment.allowed(value, contentTypes)) {
                console.log('adding ## attach ## ' + value);
                attachments.push(value);
            }
        });
        var entities = [];
        if (attachments.length > 0) {
            entities.push({ entity: attachments, type: 'attachments', score: 1.0 });
        }
        return entities;
    };
    return PromptMultiTypes;
}(Prompt_1.Prompt));
exports.PromptMultiTypes = PromptMultiTypes;
