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
var chrono = require("chrono-node");
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
            }
            else {
                cb(null, 0.0);
            }
        });
        return _this;
    }
    return PromptMultiTypes;
}(Prompt_1.Prompt));
exports.PromptMultiTypes = PromptMultiTypes;
