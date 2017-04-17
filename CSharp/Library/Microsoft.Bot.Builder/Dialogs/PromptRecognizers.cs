using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Bot.Connector;
using System.Resources;
using System.Globalization;

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
        bool? IntergerOnly { get; set; }
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
        public bool? IntergerOnly { get; set; }
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
    
    public class LocalizedCache<T>
    {
        public string Id { get; set; }
        public IDictionary<string, T> Locales { get; set; }

        public LocalizedCache()
        {
            this.Locales = new Dictionary<string, T>();
        }
        public LocalizedCache(string key)
            : this()
        {
            this.Id = key;
        }
    }

    public class LocalizedCacheList<T> : List<LocalizedCache<T>>
    {
        public LocalizedCache<T> this[string key]
        {
            get { return this.SingleOrDefault(x => x.Id == key); }
        }
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

    public class PromptRecognizers
    {
        private static LocalizedCacheList<Regex> expCache = new LocalizedCacheList<Regex>();
        private static LocalizedCacheList<IDictionary<string, IEnumerable<string>>> choicesCache = new LocalizedCacheList<IDictionary<string, IEnumerable<string>>>();

        public static IEnumerable<RecognizeEntity<string>> RecognizeLocalizedRegExp(IMessageActivity context, string expressionKey, ResourceManager resourceManager)
        {
            var entities = new List<RecognizeEntity<string>>();
            var locale = context.Locale;
            var utterance = context.Text ?? string.Empty;
            var cache = expCache[expressionKey];
            if (cache == null)
            {
                cache = new LocalizedCache<Regex>(expressionKey);
                expCache.Add(cache);
            }
            if (string.IsNullOrEmpty(locale))
            {
                locale = string.Empty;
            }

            if (!cache.Locales.ContainsKey(locale))
            {
                var expression = GetLocalizedResource(expressionKey, locale, resourceManager);
                cache.Locales[locale] = new Regex(expression, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            }
            
            foreach (Match match in cache.Locales[locale].Matches(utterance))
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
        
        public static IEnumerable<RecognizeEntity<string>> RecognizeLocalizedChoices(IMessageActivity context, string choicesKey, ResourceManager resourceManager, IPromptRecognizeChoicesOptions options = null)
        {
            var utterance = context?.Text ?? string.Empty;
            var cache = choicesCache[choicesKey];
            var locale = context.Locale;
            if (cache == null)
            {
                cache = new LocalizedCache<IDictionary<string, IEnumerable<string>>>(choicesKey);
                choicesCache.Add(cache);
            }
            if (string.IsNullOrEmpty(locale))
            {
                locale = string.Empty;
            }
            if (!cache.Locales.ContainsKey(locale))
            {
                var choicesArray = GetLocalizedResource(choicesKey, locale, resourceManager).Split('|');
                cache.Locales[locale] = ConvertToChoices(choicesArray);
            }
            var choices = cache.Locales[locale];
            return RecognizeChoices(utterance, choices, options);
        }

        private static IDictionary<string, IEnumerable<string>> ConvertToChoices(IEnumerable<string> values)
        {
            var result = new Dictionary<string, IEnumerable<string>>();
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

        public static IEnumerable<RecognizeEntity<double>> RecognizeNumbers(IMessageActivity context, IPromptRecognizeNumbersOptions options = null)
        {
            var entities = new List<RecognizeEntity<double>>();

            Func<RecognizeEntity<double>, bool> minValueWhere = (x => ((options == null || !options.MinValue.HasValue) || x.Entity >= options.MinValue));
            Func<RecognizeEntity<double>, bool> maxValueWhere = (x => ((options == null || !options.MaxValue.HasValue) || x.Entity <= options.MaxValue));
            Func<RecognizeEntity<double>, bool> integerOnlyWhere = (x => ((options != null && options.IntergerOnly.HasValue) ? !options.IntergerOnly.Value : true) || Math.Floor(x.Entity) == x.Entity);
            Func<RecognizeEntity<string>, RecognizeEntity<double>> selector = (x => new RecognizeEntity<double> { Entity = double.Parse(x.Entity), Score = x.Score });
            
            var matches = RecognizeLocalizedRegExp(context, "NumberExpression", Resource.Resources.ResourceManager);
            if (matches != null && matches.Any())
            {
                entities.AddRange(matches.Select(selector)
                    .Where(minValueWhere)
                    .Where(maxValueWhere)
                    .Where(integerOnlyWhere));
            }

            var resource = GetLocalizedResource("NumberTerms", context.Locale, Resource.Resources.ResourceManager);

            var choices = ConvertToChoices(resource.Split('|'));

            // Recognize any term based numbers
            var results = RecognizeChoices(context.Text, choices, new PromptRecognizeChoicesOptions { ExcludeValue = true });
            if (results != null && results.Any())
            {
                entities.AddRange(results.Select(selector)
                    .Where(minValueWhere)
                    .Where(maxValueWhere)
                    .Where(integerOnlyWhere));
            }
            
            return entities;
        }

        public static IEnumerable<RecognizeEntity<long>> RecognizeOrdinals(IMessageActivity context)
        {
            var entities = new List<RecognizeEntity<long>>();

            var resourceOrdinales = GetLocalizedResource("NumberOrdinals", context.Locale, Resource.Resources.ResourceManager);
            var resourceReverseOrdinals = GetLocalizedResource("NumberReverseOrdinals", context.Locale, Resource.Resources.ResourceManager);

            var ordinals = resourceOrdinales.Split('|');
            var reverseOrdinals = resourceReverseOrdinals.Split('|');

            var values = ordinals.Concat(reverseOrdinals);
            
            var choices = ConvertToChoices(values);
            
            // Recognize any term based numbers
            var results = RecognizeChoices(context.Text, choices, new PromptRecognizeChoicesOptions { ExcludeValue = true });
            if (results != null && results.Any())
            {
                entities.AddRange(results.Select(x => new RecognizeEntity<long> { Entity = long.Parse(x.Entity), Score = x.Score }));
            }

            return entities;
        }

        public static IEnumerable<RecognizeEntity<string>> RecognizeTimes(IMessageActivity context)
        {
            var entities = new List<RecognizeEntity<string>>();

            var utterance = context.Text.Trim();
            var entity = RecognizeTime(utterance);
            
            entities.Add(new RecognizeEntity<string>() {
                Entity = entity.Entity,
                Score = CalculateScore(utterance, entity.Entity)
            });

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
        
        public static IEnumerable<RecognizeEntity<T>> RecognizeChoices<T>(string utterance, IDictionary<T, IEnumerable<T>> choices, IPromptRecognizeChoicesOptions options = null)
        {
            var text = utterance ?? string.Empty;
            var entities = new List<RecognizeEntity<T>>();
            var index = 0;
            foreach (var choice in choices)
            {
                var values = choice.Value?.ToList() ?? new List<T>();
                var excludeValue = options?.ExcludeValue ?? false;
                if (!excludeValue)
                {
                    values.Add(choice.Key);
                }
                var match = FindTopEntity(RecognizeValues(text, values, options));
                if (match != null)
                {
                    entities.Add(new RecognizeEntity<T> {
                        Entity = choice.Key,
                        Score = match.Score
                    });
                }
                index++;
            }
            return entities;
        }

        public static IEnumerable<RecognizeEntity<bool>> RecognizeBooleans(IMessageActivity context)
        {
            var entities = new List<RecognizeEntity<bool>>();

            var results = RecognizeLocalizedChoices(context, "BooleanChoices", Resource.Resources.ResourceManager, new PromptRecognizeChoicesOptions());
            if (results != null)
            {
                entities.AddRange(
                    results.Select(x => new RecognizeEntity<bool> { Entity = bool.Parse(x.Entity), Score = x.Score })
                );
            }
            
            return entities;
        }

        private static Regex simpleTokinizer = new Regex(@"\w+", RegexOptions.IgnoreCase);

        public static IEnumerable<RecognizeEntity<T>> RecognizeValues<T>(string utterance, IEnumerable<T> values, IPromptRecognizeChoicesOptions options = null)
        {
            var entities = new List<RecognizeEntity<T>>();
            IList<string> tokens = new List<string>();
            foreach(Match match in simpleTokinizer.Matches(utterance.Trim().ToLowerInvariant()))
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
                foreach (Match match in simpleTokinizer.Matches(text))
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

        public static RecognizeEntity<T> FindTopEntity<T>(IEnumerable<RecognizeEntity<T>> entities)
        {
            RecognizeEntity<T> top = null;
            if (entities != null)
            {
                foreach(var entity in entities)
                {
                    if (top == null || top.Score < entity.Score)
                    {
                        top = entity;
                    }
                }
            }
            return top;
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