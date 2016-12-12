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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.Bot.Builder.Dialogs.Internals
{
    /// <summary>
    /// Drives a conversation between the bot and a user via a QnA Maker service endpoint (http://qnamaker.azurewebsites.net)
    /// </summary>
    /// <seealso cref="Dialogs.IDialog{T}" />
    [Serializable]
    public class QnADialog : IDialog<bool>
    {
        protected readonly string _kbId;
        /// <summary>
        /// Initializes a new instance of the <see cref="QnADialog"/> class.
        /// </summary>
        /// <param name="kbId">The kb identifier related to the target QnA Maker service endpoint.</param>
        /// <param name="greeting">The greeting text to send to the user.</param>
        /// <param name="askAnother">The prompt to show the user to ask them for another question.</param>
        /// <param name="notFound">The message to send the user when an answer is not found in the QnA endpoint.</param>
        /// <param name="quitPhrases">A collection of phrases users can say to end the QnA dialog.</param>
        /// <exception cref="ArgumentNullException">
        /// kbId
        /// or
        /// greeting
        /// or
        /// askAnother
        /// or
        /// notFound
        /// </exception>
        /// <exception cref="ArgumentException">If specifying quit phrases, must not give an empty collection - quitPhrases</exception>
        public QnADialog(string kbId,
            string greeting = @"Hi, Please ask me a question and I will see if I can help or you can type 'bye' to quit",
            string askAnother = @"Ask me another question, or type 'bye' to quit",
            string notFound = @"I'm sorry, we do not appear to have information on your query",
            IEnumerable<string> quitPhrases = null)
        {
            if (string.IsNullOrWhiteSpace(kbId)) throw new ArgumentNullException(nameof(kbId));

            _kbId = kbId;

            if (string.IsNullOrWhiteSpace(greeting)) throw new ArgumentNullException(nameof(greeting));
            this.Greeting = greeting;

            if (string.IsNullOrWhiteSpace(askAnother)) throw new ArgumentNullException(nameof(askAnother));
            this.AskAnother = askAnother;

            if (string.IsNullOrWhiteSpace(notFound)) throw new ArgumentNullException(nameof(notFound));
            this.NotFound = notFound;

            if (quitPhrases?.Any() == false) throw new ArgumentException(@"If specifying quit phrases, must not give an empty collection", nameof(quitPhrases));
            this.QuitPhrases = quitPhrases ?? new[] { @"bye", @"done", @"quit" };
        }

        /// <summary>Gets the quit phrases.</summary>
        protected virtual IEnumerable<string> QuitPhrases { get; }
        /// <summary>Gets the greeting.</summary>
        protected virtual string Greeting { get; }
        /// <summary>Gets the ask another.</summary>
        protected virtual string AskAnother { get; }
        /// <summary>Gets the not found.</summary>
        protected virtual string NotFound { get; }

        /// <inheritdoc />
        public virtual Task StartAsync(IDialogContext context)
        {
            PromptDialog.Text(context, MessageReceivedAsync, this.Greeting);

            return Task.CompletedTask;
        }

        protected virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            if (this.QuitPhrases.Contains(message, StringComparer.InvariantCultureIgnoreCase))
            {
                context.Done(true);
            }
            else
            {
                var response = await FetchResponse(message);

                await context.PostAsync(string.IsNullOrWhiteSpace(response) ? this.NotFound : response);

                PromptDialog.Text(context, MessageReceivedAsync, this.AskAnother);
            }
        }

        protected virtual async Task<string> FetchResponse(string question)
        {
            try
            {
                var urlFormattedQuestion = Uri.EscapeDataString(question);

                using (WebClient w = new WebClient())
                {
                    var webResponse = await w.DownloadStringTaskAsync($"http://qnaservice.cloudapp.net/KBService.svc/GetAnswer?kbId={_kbId}&question={urlFormattedQuestion}");
                    var jObj = Newtonsoft.Json.Linq.JObject.Parse(webResponse);
                    return jObj["answer"].ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting response from QnA service: {ex.ToString()}");

                return null;
            }
        }
    }
}