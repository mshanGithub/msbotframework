using System;

using Microsoft.Bot.Builder.Calling.ObjectModel.Contracts;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Microsoft.Bot.Builder.Calling.Events
{
    public class IncomingCallEvent
    {
        public IncomingCallEvent(Conversation conversation, Workflow resultingWorkflow, IEnumerable<KeyValuePair<string, string>> callParameters = null)
        {
            if (conversation == null)
                throw new ArgumentNullException(nameof(conversation));
            if (resultingWorkflow == null)
                throw new ArgumentNullException(nameof(resultingWorkflow));
            IncomingCall = conversation;
            ResultingWorkflow = resultingWorkflow;
            IncomingCallParameters = callParameters ?? new Dictionary<string, string>();
        }

        public Conversation IncomingCall { get; set; }

        public Workflow ResultingWorkflow { get; set; }
        public IEnumerable<KeyValuePair<string, string>> IncomingCallParameters { get; set; }
    }
}
