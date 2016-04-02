var assert = require('assert');
var builder = require('../');

var yesTokens = '1|y|yes|yep|sure|ok|true'.split('|');
var noTokens = '0|n|no|nope|not|false'.split('|');

function createPaddedUtterance(utterance) {
    return [
        utterance,
        ' ' + utterance,
        utterance + ' ',
        ' ' + utterance + ' '
    ];
}

function createPaddedUtterances(utterances) {
    return [].concat.apply(
        [], 
        utterances.map(createPaddedUtterance));
}

describe('EntityRecognizer', function() {
    describe('parseBoolean', function() {
        it('should return true', function() {
            var utterances = createPaddedUtterances(yesTokens);
            
            utterances.forEach(function(u) {
                assert(builder.EntityRecognizer.parseBoolean(u) === true, 'utterance: ' + u);
            });
        });
        
        it('should return false', function() {        
            var utterances = createPaddedUtterances(noTokens);
            
            utterances.forEach(function(u) {
                assert(builder.EntityRecognizer.parseBoolean(u) === false, 'utterance: ' + u);
            });
        });
        
        it ('should return undefined', function() {                
            var utterances = createPaddedUtterances([
                'no I dont think so',
                'yes lets do it',
                'no thankyou',
                'yes please',
                'yesno',
                'yes no'
            ]);
            
            utterances.forEach(function(u) {
                assert(builder.EntityRecognizer.parseBoolean(u) === undefined, 'utterance: ' + u);
            });
        });
    });
});