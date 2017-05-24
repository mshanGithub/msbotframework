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

        private async Task<Stream> ResolveFromSource(Attachment source)
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
