using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Microsoft.Bot.Connector
{
    public static partial class Extensions
    {
        /// <summary>
        /// Creates a new attachment from <see cref="HeroCard"/>.
        /// </summary>
        /// <param name="card"> The instance of <see cref="HeroCard"/>.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this HeroCard card)
        {
            return CreateAttachment(card, HeroCard.ContentType);
        }

        /// <summary>
        /// Creates a new attachment from <see cref="ThumbnailCard"/>.
        /// </summary>
        /// <param name="card"> The instance of <see cref="ThumbnailCard"/>.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this ThumbnailCard card)
        {
            return CreateAttachment(card, ThumbnailCard.ContentType);
        }

        /// <summary>
        /// Creates a new attachment from <see cref="SigninCard"/>.
        /// </summary>
        /// <param name="card"> The instance of <see cref="SigninCard"/>.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this SigninCard card)
        {
            return CreateAttachment(card, SigninCard.ContentType);
        }

        /// <summary>
        /// Creates a new attachment from <see cref="ReceiptCard"/>.
        /// </summary>
        /// <param name="card"> The instance of <see cref="ReceiptCard"/>.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this ReceiptCard card)
        {
            return CreateAttachment(card, ReceiptCard.ContentType);
        }

        /// <summary>
        /// Creates a new attachment from <see cref="AudioCard"/>.
        /// </summary>
        /// <param name="card"> The instance of <see cref="AudioCard"/>.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this AudioCard card)
        {
            return CreateAttachment(card, AudioCard.ContentType);
        }


        /// <summary>
        /// Creates a new attachment from <see cref="VideoCard"/>.
        /// </summary>
        /// <param name="card"> The instance of <see cref="VideoCard"/>.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this VideoCard card)
        {
            return CreateAttachment(card, VideoCard.ContentType);
        }

        /// <summary>
        /// Creates a new attachment from <see cref="AnimationCard"/>.
        /// </summary>
        /// <param name="card"> The instance of <see cref="AnimationCard"/>.</param>
        /// <returns> The generated attachment.</returns>
        public static Attachment ToAttachment(this AnimationCard card)
        {
            return CreateAttachment(card, AnimationCard.ContentType);
        }

        private static Attachment CreateAttachment<T>(T card, string contentType)
        {
            return new Attachment
            {
                Content = card,
                ContentType = contentType
            };
        }

        /// <summary>
        /// Gets SSML markup for this <see cref="Attachment"/>
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        public static string GetSpeech(this Attachment attachment)
        {
            if (attachment.Content is ReceiptCard)
            {
                return GetSpeech(attachment.Content as ReceiptCard);
            }
            else if (attachment.Content is SigninCard)
            {
                return GetSpeech(attachment.Content as SigninCard);
            }
            else
            {
                return GetStandardCardSpeech((dynamic)(attachment.Content));
            }
        }
        /// <summary>
        /// Gets SSML markup for this <see cref="ReceiptCard"/>.
        /// </summary>
        /// <param name="receiptCard"></param>
        /// <returns></returns>
        public static string GetSpeech(this ReceiptCard receiptCard)
        {

            var title = receiptCard?.Title;
            var total = receiptCard?.Total;
            var itemCount = receiptCard?.Items?.Count;

            var optionsSsml = receiptCard.Buttons.AsOptionsSsml();

            var receiptSpeech = @"<p>";
            if (!string.IsNullOrWhiteSpace(title))
            {
                receiptSpeech += $@"<s>{title}</s>";
            }
            if (itemCount > 0)
            {
                receiptSpeech += $@"<s>Includes {itemCount} {(itemCount == 1 ? "item" : "items")}</s>";
            }
            if (!string.IsNullOrWhiteSpace(total))
            {
                receiptSpeech += $@"<s>Your total is {total}</s>";
            }

            return WrapSSML(receiptSpeech).CombineSSML(optionsSsml);
        }

        /// <summary>
        /// Gets SSML markup for this <see cref="SigninCard"/>
        /// </summary>
        /// <param name="signinCard"></param>
        /// <returns></returns>
        public static string GetSpeech(this SigninCard signinCard) => WrapSSML($@"<p>{(!string.IsNullOrWhiteSpace(signinCard.Text) ? signinCard.Text : null)}</p>").CombineSSML(signinCard.Buttons.AsOptionsSsml());
        /// <summary>
        /// Gets SSML markup for this <see cref="HeroCard"/>
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string GetSpeech(this HeroCard card) => GetStandardCardSpeech(card);
        /// <summary>
        /// Gets SSML markup for this <see cref="ThumbnailCard"/>
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string GetSpeech(this ThumbnailCard card) => GetStandardCardSpeech(card);
        /// <summary>
        /// Gets SSML markup for this <see cref="AudioCard"/>
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string GetSpeech(this AudioCard card) => GetStandardCardSpeech(card);
        /// <summary>
        /// Gets SSML markup for this <see cref="VideoCard"/>
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string GetSpeech(this VideoCard card) => GetStandardCardSpeech(card);
        /// <summary>
        /// Gets SSML markup for this <see cref="AnimationCard"/>
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public static string GetSpeech(this AnimationCard card) => GetStandardCardSpeech(card);

        private static string GetStandardCardSpeech(dynamic card)
        {
            string cardTitle = card?.Title;
            string cardSubtitle = card?.Subtitle;
            string cardText = card?.Text;

            var msgText = @"<p>";

            if (!string.IsNullOrWhiteSpace(cardTitle))
            {
                msgText += $@"<s>{cardTitle}</s>";
            }
            if (!string.IsNullOrWhiteSpace(cardSubtitle))
            {
                msgText += $@"<s>{cardSubtitle}</s>";
            }
            if (!string.IsNullOrWhiteSpace(cardText))
            {
                msgText += $@"<s>{cardText}</s>";
            }

            msgText += @"</p>";

            // SSML Markup with text as a paragraph boundary.
            return WrapSSML($@"<p>{msgText}</p>");
        }

        /// <summary>
        /// Gets SSML markup for the options presented in the current <see cref="IMessageActivity"/>. If the message contains <see cref="IMessageActivity.SuggestedActions"/>, these are used. Otherwise the <see cref="CardAction"/> values in the <c>Buttons</c> property of the first <see cref="Attachment"/> in <see cref="IMessageActivity.Attachments"/> are used as the options.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetOptionSpeech(this IMessageActivity msg)
        {
            try
            {
                // pull action text from suggested actions for our recipient or the group
                if (msg?.SuggestedActions?.Actions?.Any() == true
                    && (msg.SuggestedActions.To?.Any() == false
                        || msg.SuggestedActions.To.Contains(msg.Recipient.Id)))
                {
                    return msg.SuggestedActions.Actions.AsOptionsSsml();
                }
                else
                {   // if no suggested actions, pull the first attachment (card) and any buttons on it;
                    // use those as option speech
                    var cardContent = (dynamic)(msg?.Attachments?.FirstOrDefault()?.Content);
                    if (cardContent?.Buttons != null)
                    {
                        return (cardContent.Buttons as IEnumerable<CardAction>).AsOptionsSsml();
                    }
                }
            }
            catch { }

            return string.Empty;
        }

        public static string AsOptionsSsml(this IEnumerable<CardAction> buttons) => buttons?.Select(c => c.Title).AsOptionsSsml();

        private static string AsOptionsSsml(this IEnumerable<string> optionTexts)
        {
            var speakText = string.Empty;
            if (optionTexts?.Where(i => !string.IsNullOrWhiteSpace(i)).Any() == true)
            {   // results in "you can say this, that, or the other"
                speakText += @"<p>You can say ";
                var numResponses = optionTexts.Count();
                if (numResponses == 1)
                {
                    speakText += optionTexts.First();
                }
                else if (numResponses == 2)
                {
                    speakText += string.Join(@" or ", optionTexts);
                }
                else
                {
                    for (int i = 0; i < numResponses; i++)
                    {
                        if (i < numResponses - 1)
                        {
                            speakText += $@"{optionTexts.ElementAt(i)} <break /> ";
                        }
                        else
                        {
                            speakText += $@" or {optionTexts.ElementAt(i)}";
                        }
                    }
                }
                speakText += @"</p>";
            }

            return WrapSSML(speakText);
        }

        /// <summary>
        /// Creates SSML markup for the list of cards presented in a <see cref="AttachmentLayoutTypes.Carousel"/> layout within an <see cref="IMessageActivity"/>. The value puts together the <c>Title</c>, <c>Subtitle</c>, and <c>Text</c> properties of each <see cref="Attachment"/> in the carousel.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string GetSpeechForCarousel(this IMessageActivity msg)
        {
            string speakText = @"<p>";

            if (msg.AttachmentLayout == AttachmentLayoutTypes.Carousel
                && msg.Attachments?.Any() == true)
            {
                speakText += ". ";  // force a pause before reading it

                // Get title, subtitle, and text values from each card in the carousel. separate by ','
                var itemSsmls = msg.Attachments.Select(a => a.GetSpeech()).ToList();
                if (itemSsmls.Count == 1)
                {
                    speakText += itemSsmls[0];
                }
                else if (itemSsmls.Count == 2)
                {
                    speakText += $@"{itemSsmls[0]} and {itemSsmls[1]}";
                }
                else
                {
                    for (int i = 0; i < itemSsmls.Count; i++)
                    {
                        if (i < itemSsmls.Count - 1)
                        {
                            speakText += $@"{itemSsmls[i]} <break /> ";
                        }
                        else
                        {
                            speakText += $@"and {itemSsmls[i]}";
                        }
                    }
                }
            }

            return WrapSSML(speakText += @"</p>");
        }

        private static string WrapSSML(string ssmlBody) =>
        $@"<?xml version=""1.0""?>
            <speak version=""1.0"" xmlns=""http://www.w3.org/2001/10/synthesis""
                     xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                     xsi:schemaLocation=""http://www.w3.org/2001/10/synthesis
                               http://www.w3.org/TR/speech-synthesis/synthesis.xsd""
                     xml:lang=""en-US"">
            {ssmlBody}
            </speak>";

        /// <summary>
        /// Combines two SSML document strings in to one. Useful for aggregating output from the <c>GetSpeech</c> methods, <see cref="GetSpeechForCarousel(IMessageActivity)"/> and/or <see cref="GetOptionSpeech(IMessageActivity)"/>
        /// </summary>
        /// <param name="ssmlOne">The first SSML output, this will have the body of <paramref name="ssmlTwo"/> appended to the end of its body</param>
        /// <param name="ssmlTwo">The second SSML document. This body of which will be appended to the end of the body of <paramref name="ssmlOne"/></param>
        /// <returns></returns>
        /// <example>
        /// var msg = context.MakeMessage();
        /// // Add some cards, carousel, buttons here
        /// msg.Speak = 
        /// </example>
        public static string CombineSSML(this string ssmlOne, string ssmlTwo)
        {
            var docOne = XDocument.Parse(ssmlOne);

            if (docOne.Element(@"speak") == null)
            {
                throw new ArgumentException(@"SSML not valid", nameof(ssmlOne));
            }

            // pull out the "body" elements from the second SSML document
            var docTwo = XDocument.Parse(ssmlTwo);
            var docTwoBody = docTwo.Element(@"speak")?.Descendants();

            if (docTwoBody == null)
            {
                throw new ArgumentException(@"SSML not valid", nameof(ssmlTwo));
            }

            // Insert them at the end of the current body for the first SSML document
            docOne.Root.Add(docTwoBody);

            return docOne.ToString();
        }
    }
}
