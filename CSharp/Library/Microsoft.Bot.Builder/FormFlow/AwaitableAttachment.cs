// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Bot Framework: http://botframework.com
// 
// Bot Builder SDK GitHub:
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
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace Microsoft.Bot.Builder.FormFlow.Advanced
{
    [Serializable]
    // TODO-MK: now we are resolving to a Stream - we could probably return stream + additional data (original name, type, etc)
    // discuss what about to return an Attachment with the Stream or byte[] in the 'object content' property
    // download attachment data for validation and store in another place?
    // use data annotation validators as parameter to ctor and validate source?
    public class AwaitableAttachment : IAwaitable<Stream>, IAwaiter<Stream>, ISerializable
    {
        [NonSerialized]
        private readonly IAwaiter<Stream> awaiter;

        private readonly Attachment source;

        public AwaitableAttachment(Attachment source)
        {
            this.source = source;

            this.awaiter = Awaitable.FromSource(source, this.ResolveFromSource) as IAwaiter<Stream>;
        }

        private AwaitableAttachment(SerializationInfo info, StreamingContext context)
        {
            // constructor arguments
            var contentType = default(string);
            var contentUrl = default(string);
            var name = default(string);

            SetField.NotNullFrom(out contentType, nameof(Attachment.ContentType), info);
            SetField.NotNullFrom(out contentUrl, nameof(Attachment.ContentUrl), info);
            SetField.NotNullFrom(out name, nameof(Attachment.Name), info);

            this.source = new Attachment
            {
                ContentType = contentType,
                ContentUrl = contentUrl,
                Name = name
            };

            this.awaiter = Awaitable.FromSource(this.source, this.ResolveFromSource) as IAwaiter<Stream>;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // constructor arguments
            info.AddValue(nameof(Attachment.ContentType), this.source.ContentType);
            info.AddValue(nameof(Attachment.ContentUrl), this.source.ContentUrl);
            info.AddValue(nameof(Attachment.Name), this.source.Name);
        }

        public bool IsCompleted
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IAwaiter<Stream> GetAwaiter()
        {
            return this.awaiter;
        }

        public Stream GetResult()
        {
            throw new NotImplementedException();
        }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        protected virtual async Task<Stream> ResolveFromSource(Attachment source)
        {
            var stream = new MemoryStream();

            // TODO-MK: handle specific channel stuff - ie. authorization, etc
            // here another client/stream type could be used as well to handle big payloads
            using (HttpClient httpClient = new HttpClient())
            {
                var bytes = await httpClient.GetByteArrayAsync(source.ContentUrl);
                await stream.WriteAsync(bytes, 0, bytes.Length);

                // reset to start
                stream.Position = 0;
            }

            // TODO-MK: add some validation when type resolved?

            return stream;
        }
    }
}
