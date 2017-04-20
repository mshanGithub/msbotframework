using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using System.Resources;
using System.Globalization;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System.Collections.Concurrent;

namespace Microsoft.Bot.Builder.Dialogs
{
    public class RecognizeEntity<T>
    {
        public T Entity { get; set; }
        public double Score { get; set; }
    }
    public interface IPromptRecognizeNumbersOptions
    {
        int? MinValue { get; set; }
        int? MaxValue { get; set; }
        bool? IntegerOnly { get; set; }
    }

    public class PromptRecognizeNumbersOptions : IPromptRecognizeNumbersOptions
    {
        /// <summary>
        /// (Optional) Minimum value allowed.
        /// </summary>
        public int? MinValue { get; set; }
        /// <summary>
        /// (Optional) Maximum value allowed.
        /// </summary>
        public int? MaxValue { get; set; }
        /// <summary>
        /// (Optional) If true, then only integers will be recognized.
        /// </summary>
        public bool? IntegerOnly { get; set; }
    }

    public interface IPromptRecognizeValuesOptions
    {
        bool? AllowPartialMatches { get; set; }
        int? MaxTokenDistance { get; set; }
    }

    public interface IPromptRecognizeChoicesOptions : IPromptRecognizeValuesOptions
    {
        bool? ExcludeValue { get; set; }
        bool? ExcludeAction { get; set; }
    }

    public class PromptRecognizeChoicesOptions : IPromptRecognizeChoicesOptions
    {
        /// <summary>
        /// (Optional) If true, the choices value will NOT be recognized over.
        /// </summary>
        public bool? ExcludeValue { get; set; }
        /// <summary>
        /// (Optional) If true, the choices action will NOT be recognized over.
        /// </summary>
        public bool? ExcludeAction { get; set; }
        /// <summary>
        /// (Optional) if true, then only some of the tokens in a value need to exist to be considered a match.The default value is "false".
        /// </summary>
        public bool? AllowPartialMatches { get; set; }
        /// <summary>
        /// (Optional) maximum tokens allowed between two matched tokens in the utterance.So with
        /// a max distance of 2 the value "second last" would match the utternace "second from the last"
        /// but it wouldn't match "Wait a second. That's not the last one is it?". 
        /// The default value is "2".  
        /// </summary>
        public int? MaxTokenDistance { get; set; }
    }

    public class ChronoDuration
    {
        public string Entity { get; internal set; }
        public ChronoDurationResolution Resolution { get; set; }

        public ChronoDuration()
        {
            this.Resolution = new ChronoDurationResolution();
        }
    }

    public class ChronoDurationResolution
    {
        public string ResolutionType { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }

    public interface IPromptRecognizers
    {
        /// <summary>Recognizer using a RegEx to match expressions.</summary>
        /// <param name="message">Message context.</param>
        /// <param name="expressionKey">Name of the resource with the RegEx.</param>
        /// <param name="resourceManager">Resources with the localized expression.</param>
        IEnumerable<RecognizeEntity<string>> RecognizeLocalizedRegExp(IMessageActivity message, string expressionKey, ResourceManager resourceManager);

        /// <summary>Recognizer for a number.</summary>
        /// <param name="message">Message context.</param>
        /// <param name="synonymsDictionary">Dictionary with the options to choose from as a key and their synonyms as a value.</param>
        /// <param name="options">Options of the Recognizer. <see cref="IPromptRecognizeChoicesOptions" /></param>
        IEnumerable<RecognizeEntity<T>> RecognizeChoices<T>(IMessageActivity message, IDictionary<T, IEnumerable<T>> synonymsDictionary, IPromptRecognizeChoicesOptions options = null);

        /// <summary>Recognizer for a number.</summary>
        /// <param name="message">Message context.</param>
        /// <param name="synonymsKey">Name of the resource with the synonyms.</param>
        /// <param name="resourceManager">Resources with the localized synonyms.</param>
        /// <param name="options">Options of the Recognizer. <see cref="IPromptRecognizeChoicesOptions" /></param>
        IEnumerable<RecognizeEntity<string>> RecognizeLocalizedChoices(IMessageActivity message, string synonymsKey, ResourceManager resourceManager, IPromptRecognizeChoicesOptions options = null);

        /// <summary>Recognizer for a number.</summary>
        /// <param name="message">Message context.</param>
        /// <param name="options">Options of the Recognizer. <see cref="IPromptRecognizeNumbersOptions" /></param>
        IEnumerable<RecognizeEntity<double>> RecognizeNumbers(IMessageActivity message, IPromptRecognizeNumbersOptions options = null);

        /// <summary>Recognizer for a ordinal number.</summary>
        /// <param name="message">Message context.</param>
        IEnumerable<RecognizeEntity<long>> RecognizeOrdinals(IMessageActivity message);

        /// <summary>Recognizer for a time or duration.</summary>
        /// <param name="message">Message context.</param>
        IEnumerable<RecognizeEntity<string>> RecognizeTimes(IMessageActivity message);

        /// <summary>Recognizer for true/false expression.</summary>
        /// <param name="message">Message context.</param>
        IEnumerable<RecognizeEntity<bool>> RecognizeBooleans(IMessageActivity message);
    }

    internal class SynonymsDictionary : Dictionary<string, IEnumerable<string>> { }

    internal class LocalizedDictionary<T> : ConcurrentDictionary<string, T> { }

    internal class ResourcesCache<T> : ConcurrentDictionary<string, LocalizedDictionary<T>> { }

    [Serializable]
    public class PromptRecognizers : IPromptRecognizers
    {
        private const string ResourceKeyCardinals = "NumberTerms";
        private const string ResourceKeyOrdinals = "NumberOrdinals";
        private const string ResourceKeyReverserOrdinals = "NumberReverseOrdinals";
        private const string ResourceKeyNumberRegex = "NumberExpression";
        private const string ResourceKeyBooleans = "BooleanChoices";

        private static Regex simpleTokenizer = new Regex(@"\w+", RegexOptions.IgnoreCase);
        private static ResourcesCache<Regex> expCache = new ResourcesCache<Regex>();
        private static ResourcesCache<SynonymsDictionary> synonymsCache = new ResourcesCache<SynonymsDictionary>();

        public PromptRecognizers()
        {
        }

        public IEnumerable<RecognizeEntity<string>> RecognizeLocalizedRegExp(IMessageActivity message, string expressionKey, ResourceManager resourceManager)
        {
            var entities = new List<RecognizeEntity<string>>();
            var locale = message?.Locale ?? string.Empty;
            var utterance = message?.Text?.Trim().ToLowerInvariant() ?? string.Empty;

            LocalizedDictionary<Regex> cachedLocalizedRegex;
            if (!expCache.TryGetValue(expressionKey, out cachedLocalizedRegex))
            {
                var localizedRegex = new LocalizedDictionary<Regex>();
                cachedLocalizedRegex = expCache.GetOrAdd(expressionKey, localizedRegex);
            }

            Regex cachedRegex;
            if (!cachedLocalizedRegex.TryGetValue(locale, out cachedRegex))
            {
                var expression = GetLocalizedResource(expressionKey, locale, resourceManager);
                var regex = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                cachedRegex = cachedLocalizedRegex.GetOrAdd(locale, regex);
            }
            
            foreach (Match match in cachedRegex.Matches(utterance))
            {
                if (match.Success)
                {
                    entities.Add(new RecognizeEntity<string>
                    {
                        Entity = match.Value,
                        Score = CalculateScore(utterance, match.Value)
                    });
                }

            }
            return entities;
        }
        
        public IEnumerable<RecognizeEntity<string>> RecognizeLocalizedChoices(IMessageActivity message, string synonymsKey, ResourceManager resourceManager, IPromptRecognizeChoicesOptions options = null)
        {
            var locale = message?.Locale ?? string.Empty;

            LocalizedDictionary<SynonymsDictionary> cachedLocalizedSynonyms;
            if (!synonymsCache.TryGetValue(synonymsKey, out cachedLocalizedSynonyms))
            {
                var localizedSynonyms = new LocalizedDictionary<SynonymsDictionary>();
                cachedLocalizedSynonyms = synonymsCache.GetOrAdd(synonymsKey, localizedSynonyms);
            }

            SynonymsDictionary cachedSynonyms;
            if (!cachedLocalizedSynonyms.TryGetValue(locale, out cachedSynonyms))
            {
                var synonymArray = GetLocalizedResource(synonymsKey, locale, resourceManager).Split('|');
                var synonyms = ConvertToSynonyms(synonymArray);
                cachedSynonyms = cachedLocalizedSynonyms.GetOrAdd(locale, synonyms);
            }
            
            return RecognizeChoices(message, cachedSynonyms, options);
        }
        
        public IEnumerable<RecognizeEntity<double>> RecognizeNumbers(IMessageActivity message, IPromptRecognizeNumbersOptions options = null)
        {
            var entities = new List<RecognizeEntity<double>>();

            Func<RecognizeEntity<double>, bool> minValueWhere = (x => ((options == null || !options.MinValue.HasValue) || x.Entity >= options.MinValue));
            Func<RecognizeEntity<double>, bool> maxValueWhere = (x => ((options == null || !options.MaxValue.HasValue) || x.Entity <= options.MaxValue));
            Func<RecognizeEntity<double>, bool> integerOnlyWhere = (x => ((options != null && options.IntegerOnly.HasValue) ? !options.IntegerOnly.Value : true) || Math.Floor(x.Entity) == x.Entity);
            Func<RecognizeEntity<string>, RecognizeEntity<double>> selector = (x => new RecognizeEntity<double> { Entity = double.Parse(x.Entity), Score = x.Score });
            
            var matches = RecognizeLocalizedRegExp(message, ResourceKeyNumberRegex, Resource.Resources.ResourceManager);
            if (matches != null && matches.Any())
            {
                entities.AddRange(matches.Select(selector)
                    .Where(minValueWhere)
                    .Where(maxValueWhere)
                    .Where(integerOnlyWhere));
            }

            var resource = GetLocalizedResource(ResourceKeyCardinals, message?.Locale, Resource.Resources.ResourceManager);

            var synonyms = ConvertToSynonyms(resource.Split('|'));

            // Recognize any term based numbers
            var results = RecognizeChoices(message, synonyms, new PromptRecognizeChoicesOptions { ExcludeValue = true });
            if (results != null && results.Any())
            {
                entities.AddRange(results.Select(selector)
                    .Where(minValueWhere)
                    .Where(maxValueWhere)
                    .Where(integerOnlyWhere));
            }
            
            return entities;
        }

        public IEnumerable<RecognizeEntity<long>> RecognizeOrdinals(IMessageActivity message)
        {
            var entities = new List<RecognizeEntity<long>>();

            var resourceOrdinales = GetLocalizedResource(ResourceKeyOrdinals, message?.Locale, Resource.Resources.ResourceManager);
            var resourceReverseOrdinals = GetLocalizedResource(ResourceKeyReverserOrdinals, message?.Locale, Resource.Resources.ResourceManager);

            var ordinals = resourceOrdinales.Split('|');
            var reverseOrdinals = resourceReverseOrdinals.Split('|');

            var values = ordinals.Concat(reverseOrdinals);
            
            var synonyms = ConvertToSynonyms(values);
            
            // Recognize any term based numbers
            var results = RecognizeChoices(message, synonyms, new PromptRecognizeChoicesOptions { ExcludeValue = true });
            if (results != null && results.Any())
            {
                entities.AddRange(results.Select(x => new RecognizeEntity<long> { Entity = long.Parse(x.Entity), Score = x.Score }));
            }

            return entities;
        }

        public IEnumerable<RecognizeEntity<string>> RecognizeTimes(IMessageActivity message)
        {
            var entities = new List<RecognizeEntity<string>>();

            var utterance = message?.Text?.Trim();
            var entity = RecognizeTime(utterance);
            
            entities.Add(new RecognizeEntity<string>() {
                Entity = entity.Entity,
                Score = CalculateScore(utterance, entity.Entity)
            });

            return entities;
        }
        
        public IEnumerable<RecognizeEntity<T>> RecognizeChoices<T>(IMessageActivity message, IDictionary<T, IEnumerable<T>> synonymsDictionary, IPromptRecognizeChoicesOptions options = null)
        {
            var entities = new List<RecognizeEntity<T>>();
            var index = 0;
            foreach (var synonyms in synonymsDictionary)
            {
                var values = synonyms.Value?.ToList() ?? new List<T>();
                var excludeValue = options?.ExcludeValue ?? false;
                if (!excludeValue)
                {
                    values.Add(synonyms.Key);
                }
                var match = RecognizeValues(message, values, options).MaxBy(x => x.Score);
                if (match != null)
                {
                    entities.Add(new RecognizeEntity<T> {
                        Entity = synonyms.Key,
                        Score = match.Score
                    });
                }
                index++;
            }
            return entities;
        }

        public IEnumerable<RecognizeEntity<bool>> RecognizeBooleans(IMessageActivity message)
        {
            var entities = new List<RecognizeEntity<bool>>();

            var results = RecognizeLocalizedChoices(message, ResourceKeyBooleans, Resource.Resources.ResourceManager, new PromptRecognizeChoicesOptions());
            if (results != null)
            {
                entities.AddRange(
                    results.Select(x => new RecognizeEntity<bool> { Entity = bool.Parse(x.Entity), Score = x.Score })
                );
            }
            
            return entities;
        }
        
        private static IEnumerable<RecognizeEntity<T>> RecognizeValues<T>(IMessageActivity message, IEnumerable<T> values, IPromptRecognizeChoicesOptions options = null)
        {
            var utterance = message?.Text?.Trim().ToLowerInvariant() ?? string.Empty;
            var entities = new List<RecognizeEntity<T>>();
            IList<string> tokens = new List<string>();
            foreach(Match match in simpleTokenizer.Matches(utterance))
            {
                tokens.Add(match.Value);
            }
            var maxDistance = options?.MaxTokenDistance ?? 2;
            var index = 0;
            foreach(var value in values)
            {
                var text = value.ToString();
                var topScore = 0.0;
                IList<string> vTokens = new List<string>();
                foreach (Match match in simpleTokenizer.Matches(text))
                {
                    vTokens.Add(match.Value);
                }
                for (int i = 0; i < tokens.Count; i++)
                {
                    var score = MatchValue(tokens.ToArray(), vTokens.ToArray(), i, maxDistance, options?.AllowPartialMatches ?? false);
                    if (topScore < score)
                    {
                        topScore = score;
                    }
                }
                if (topScore > 0.0)
                {
                    entities.Add(new RecognizeEntity<T>
                    {
                        Entity = value,
                        Score = topScore
                    });
                }
                index++;
            }
            return entities;
        }

        private static ChronoDuration RecognizeTime(string utterance)
        {
            ChronoDuration response = null;
            try
            {
                Chronic.Parser parser = new Chronic.Parser();
                var results = parser.Parse(utterance);

                if (results != null)
                {
                    response = new ChronoDuration()
                    {
                        Entity = results.ToTime().TimeOfDay.ToString(),
                        Resolution = new ChronoDurationResolution()
                        {
                            Start = results.Start,
                            End = results.End
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error recognizing time: {ex.Message}");
                response = null;
            }

            return response;
        }

        private static SynonymsDictionary ConvertToSynonyms(IEnumerable<string> values)
        {
            var result = new SynonymsDictionary();
            foreach (var term in values)
            {
                var subTerm = term.Split('=');
                if (subTerm.Count() == 2)
                {
                    var synonyms = subTerm[1].Split(',');
                    result.Add(subTerm[0], synonyms);
                }
                else
                {
                    result.Add(subTerm[0], Enumerable.Empty<string>());
                }
            }
            return result;
        }


        private static double MatchValue(string[] tokens, string[] vTokens, int index, int maxDistance, bool allowPartialMatches)
        {
            var startPosition = index;
            double matched = 0;
            var totalDeviation = 0;
            foreach(var token in vTokens)
            {
                var pos = IndexOfToken(tokens.ToList(), token, startPosition);
                if (pos >= 0)
                {
                    var distance = matched > 0 ? pos - startPosition : 0;
                    if (distance <= maxDistance)
                    {
                        matched++;
                        totalDeviation += distance;
                        startPosition = pos + 1;
                    }
                }
            }

            var score = 0.0;
            if (matched > 0 && (matched == vTokens.Length || allowPartialMatches))
            {
                var completeness = matched / vTokens.Length;
                var accuracy = completeness * (matched / (matched + totalDeviation));
                var initialScore = accuracy * (matched / tokens.Length);

                score = 0.4 + (0.6 * initialScore);
            }
            return score;
        }

        private static int IndexOfToken(List<string> tokens, string token, int startPos)
        {
            if (tokens.Count <= startPos) return -1;
            return tokens.FindIndex(startPos, x => x == token);
        }

        private static double CalculateScore(string utterance, string entity, double max = 1.0, double min = 0.5)
        {
            return Math.Min(min + (entity.Length / (double)utterance.Length), max);
        }
        
        private static string GetLocalizedResource(string resourceKey, string locale, ResourceManager resourceManager)
        {
            CultureInfo culture;
            try
            {
                culture = new CultureInfo(locale);
            }
            catch
            {
                culture = new CultureInfo("en-US");
            }
            return resourceManager.GetString(resourceKey, culture);
        }
    }
}