using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Chronic;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Builder.Dialogs.Utilities
{
    /// <summary>
    /// Utility class used to parse & resolve common entities like datetimes received from LUIS.
    /// </summary>
    public class EntityRecognizer
    {
        /// <summary>
        /// Searches for the first occurance of a specific entity type within a set.
        /// </summary>
        /// <param name="entities">Set of entities to search over.</param>
        /// <param name="entityType">Type of entity to find.</param>
        /// <returns>The first <see cref="EntityRecommendation" /> in <paramref name="entities" /> of <paramref name="entityType" /> or null if a matching <see cref="EntityRecommendation" /> cannot be found.</returns>
        public static EntityRecommendation FindEntity(IEnumerable<EntityRecommendation> entities, string entityType)
        {
            return entities.FirstOrDefault(e => e.Type == entityType);
        }

        /// <summary>
        /// Finds all occurrences of a specific entity type within a set.
        /// </summary>
        /// <param name="entities">Set of entities to search over.</param>
        /// <param name="entityType">Type of entity to find.</param>
        /// <returns>List of <see cref="EntityRecommendation" /> in <paramref name="entities" /> of <paramref name="entityType" /> or null if a matching <see cref="EntityRecommendation" /> cannot be found.</returns>
        public static IList<EntityRecommendation> FindEntities(IEnumerable<EntityRecommendation> entities, string entityType)
        {
            return entities.Where(e => e.Type == entityType).ToList();
        }

        /// <summary>
        /// Parses a date / time from a set of LUIS entities.
        /// </summary>
        /// <param name="entities">Set of entities to search over.</param>
        /// <param name="parsedDate">When this method returns, parsedDate.value contains the parsed date
        /// if a date can be determined. If no date can be determined then a null DateTime object is returned.
        /// </param>
        /// <param name="attemptResolvePartialDates">Indicates if partial dates should be resolved. e.g. '2016-02' or 'XXXX-WXX-4'. 
        /// If resolution is attempted then either the next occurance will be used, e.g. 'XXXX-WXX-02' (Tuesday) will resolve to the date of the next Tuesday or, 
        /// if a specific period is identified, such as '2016-02' (February 2016) the output will resolve to 2016-01-01, the first date of that period.</param>
        public static void ParseDateTime(IEnumerable<EntityRecommendation> entities, out DateTime? parsedDate, bool attemptResolvePartialDates = true)
        {
            parsedDate = ResolveDateTime(entities, attemptResolvePartialDates);
        }

        /// <summary>
        /// Parses a date / time from a users utterance.
        /// </summary>
        /// <param name="utterance">Text utterance to parse. The utterance is parsed using 
        /// the [Chronic.Signed](https://www.nuget.org/packages/Chronic.Signed/) library.</param>
        /// <param name="parsedDate">When this method returns, parsedDate.value contains the parsed date
        /// if a date can be determined. If no date can be determined then a null DateTime object is returned.
        /// </param>
        public static void ParseDateTime(string utterance, out DateTime? parsedDate)
        {
            parsedDate = RecognizeTime(utterance);
        }

        /// <summary>
        /// Parses a number from a users utterance.
        /// </summary>
        /// <param name="utterance">Text utterance to parse. The utterance is parsed using 
        /// the [Chronic.Signed](https://www.nuget.org/packages/Chronic.Signed/) library.</param>
        /// <param name="parsedNumber">When this method returns, this contains the parsed number
        /// if a one can be found. If no number can be determined then double.NaN is returned.
        /// </param>
        public static void ParseNumber(string utterance, out double parsedNumber)
        {
            Regex numbeRegex = new Regex(@"[+-]?(?:\d+\.?\d*|\d*\.?\d+)");
            var matches = numbeRegex.Matches(utterance);

            if (matches.Count > 0)
            {
                double.TryParse(matches[0].Value, out parsedNumber);
                if (!double.IsNaN(parsedNumber))
                    return;
            }

            parsedNumber = double.NaN;
        }

        /// <summary>
        /// Parses a number from a set of LUIS entities.
        /// </summary>
        /// <param name="entities">Set of entities to resolve.</param>
        /// <param name="parsedNumber">When this method returns, this contains the parsed number
        /// if a one can be found. If no number can be determined then double.NaN is returned.
        /// </param>
        public static void ParseNumber(IEnumerable<EntityRecommendation> entities, out double parsedNumber)
        {
            var numberEntities = FindEntities(entities, "builtin.number");

            if (numberEntities != null && numberEntities.Any())
            {
                double.TryParse(numberEntities.First().Entity, out parsedNumber);
                if (!double.IsNaN(parsedNumber))
                    return;
            }

            parsedNumber = double.NaN;
        }

        /// <summary>
        /// Parses a boolean from a users utterance.
        /// </summary>
        /// <param name="utterance">Text utterance to parse.</param>
        /// <param name="parsedBoolean">Value equal to true if utterance contains any of the following *1, y, yes, yep, sure, ok, true*.
        /// 
        /// Value equal to if utterance contains any of the following *2, n, no, nope, not, false*.
        /// 
        /// If no boolean value can be parsed then a null bool object is returned.</param>
        public static void ParseBoolean(string utterance, out bool? parsedBoolean)
        {
            var boolTrueRegex = new Regex("(?i)^(1|y|yes|yep|sure|ok|true)");
            var boolFalseRegex = new Regex("(?i)^(2|n|no|nope|not|false)");

            var trueMatches = boolTrueRegex.Matches(utterance);
            if (trueMatches.Count > 0)
            {
                parsedBoolean = true;
                return;
            }

            var falseMatches = boolFalseRegex.Matches(utterance);
            if (falseMatches.Count > 0)
            {
                parsedBoolean = false;
                return;
            }

            parsedBoolean = null;
        }

        /// <summary>
        /// Finds the best match for a users utterance given a list of choices.
        /// </summary>
        /// <param name="choices">IEnumerable of string values to compare against the users utterance.</param>
        /// <param name="utterance">Text utterance to parse.</param>
        /// <param name="threshold">(Optional) minimum score needed for a match to be considered. The default value is 0.6.</param>
        /// <param name="ignoreCase">(Optional) should the matching be insensitive to case. The default value is true.</param>
        /// <param name="ignoreNonAlphanumeric">(Optional) if true then all non-alphanumeric characters (except spaces) are removed from utterance and choices before matching. The default value is true.</param>
        /// <returns>string value of the best match found from the list of choices. If no match is found then null is returned.</returns>
        public static string FindBestMatch(IEnumerable<string> choices, string utterance, double threshold = 0.6, bool ignoreCase = true, bool ignoreNonAlphanumeric = true)
        {
            StringMatch bestMatch = null;
            var matches = FindAllMatches(choices, utterance, threshold, ignoreCase, ignoreNonAlphanumeric);
            foreach (var match in matches)
            {
                if (bestMatch == null || match.Score > bestMatch.Score)
                {
                    bestMatch = match;
                }
            }

            return bestMatch?.Choice;
        }

        /// <summary>
        /// Finds all possible matches for a users utterance given a list of choices.
        /// </summary>
        /// <param name="choices">IEnumerable of string values to compare against the users utterance.</param>
        /// <param name="utterance">Text utterance to parse.</param>
        /// <param name="threshold">(Optional) minimum score needed for a match to be considered. The default value is 0.6.</param>
        /// <param name="ignoreCase">(Optional) should the matching be insensitive to case. The default value is true.</param>
        /// <param name="ignoreNonAlphanumeric">(Optional) if true then all non-alphanumeric characters (except spaces) are removed from utterance and choices before matching. The default value is true.</param>
        /// <returns>A list of <see cref="StringMatch"/> objects representing all matches identified against the list of <paramref name="choices"/>. Empty list returned if no matches found.</returns>
        private static IEnumerable<StringMatch> FindAllMatches(IEnumerable<string> choices, string utterance, double threshold = 0.6, bool ignoreCase = true, bool ignoreNonAlphanumeric = true)
        {
            var matches = new List<StringMatch>();

            var choicesList = choices as IList<string> ?? choices.ToList();

            if (!choicesList.Any())
                return matches;

            var utteranceToCheck = ignoreNonAlphanumeric
                ? utterance.ReplaceAll(@"[^A-Za-z0-9 ]", string.Empty).Trim()
                : utterance.Trim();

            var tokens = utterance.Split(' ');

            foreach (var choice in choicesList)
            {
                double score = 0;
                var choiceValue = choice.Trim();
                if (ignoreNonAlphanumeric)
                    choiceValue = choiceValue.ReplaceAll(@"[^A-Za-z0-9 ]", string.Empty);

                if (choiceValue.IndexOf(utteranceToCheck, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0)
                {
                    score = (double)utteranceToCheck.Length / choiceValue.Length;
                }
                else if (utteranceToCheck.IndexOf(choiceValue, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0)
                {
                    score = Math.Min(0.5 + ((double)choiceValue.Length / utteranceToCheck.Length), 0.9);
                }
                else
                {
                    foreach (var token in tokens)
                    {
                        var matched = string.Empty;

                        if (choiceValue.IndexOf(token, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) >= 0)
                        {
                            matched += token;
                        }

                        score = (double)matched.Length / choiceValue.Length;
                    }
                }

                if (score >= threshold)
                {
                    matches.Add(new StringMatch { Choice = choice, Score = score });
                }
            }

            return matches;
        }

        /// <summary>
        /// Finds the best match for a users utterance given a list of choices.
        /// </summary>
        /// <param name="choices">Delimited list of choices to match against.</param>
        /// <param name="utterance">Text utterance to parse.</param>
        /// <param name="threshold">(Optional) minimum score needed for a match to be considered. The default value is 0.6.</param>
        /// <param name="ignoreCase">(Optional) should the matching be insensitive to case. The default value is true.</param>
        /// <param name="ignoreNonAlphanumeric">(Optional) if true then all non-alphanumeric characters (except spaces) are removed from utterance and choices before matching. The default value is true.</param>
        /// <param name="choiceListSeperator">(Optional) seperator that should be used to split the string list of <paramref name="choices"/>. The default value is a pipe ('|')</param>
        /// <returns>string value of the best match found from the list of choices. If no match is found then null is returned.</returns>
        public static string FindBestMatch(string choices, string utterance, double threshold = 0.5,
            bool ignoreCase = true, bool ignoreNonAlphanumeric = true, char choiceListSeperator = '|')
        {
            var choicesList = ExpandChoices(choices, choiceListSeperator);

            return FindBestMatch(choicesList, utterance, threshold, ignoreCase, ignoreNonAlphanumeric);
        }

        /// <summary>
        /// Converts a set of choices into an array of strings.
        /// </summary>
        /// <param name="choices">Delimited list of values.</param>
        /// <param name="seperator">(Optional) seperator that should be used to split the string list of <paramref name="choices"/>. The default value is a pipe ('|')</param>
        /// <returns></returns>
        private static IEnumerable<string> ExpandChoices(string choices, char seperator = '|')
        {
            if (string.IsNullOrEmpty(choices))
                return new List<string>();

            return choices.Split(seperator).ToList();
        }

        /// <summary>
        /// Calculates a DateTime from a set of datetime entities.
        /// </summary>
        /// <param name="entities">List of LUIS entities to extract date from.</param>
        /// <param name="attemptResolvePartialDates">Indicates if partial dates should be resolved. e.g. '2016-02' or 'XXXX-WXX-4'. 
        /// If resolution is attempted then either the next occurance will be used, e.g. 'XXXX-WXX-02' (Tuesday) will resolve to the date of the next Tuesday or, 
        /// if a specific period is identified, such as '2016-02' (February 2016) the output will resolve to 2016-01-01, the first date of that period.</param>
        /// <returns>A DateTime object with a value set to the resolved date / time. If no date can be resolved then a null DateTime object is returned.</returns>
        private static DateTime? ResolveDateTime(IEnumerable<EntityRecommendation> entities, bool attemptResolvePartialDates = true)
        {
            var date = new DateTime?();
            var time = new DateTime?();

            foreach (var entity in entities)
            {
                if ((entity.Type.Contains("builtin.datetime.date")
                    || entity.Type.Contains("builtin.datetime.time"))
                    && entity.Resolution.Any() && (!date.HasValue || !time.HasValue))
                {
                    var resolutionStr = entity.Resolution.ContainsKey("date")
                        ? entity.Resolution["date"]
                        : entity.Resolution.ContainsKey("time") ? entity.Resolution["time"] : string.Empty;

                    if (resolutionStr == "PRESENT_REF")
                    {
                        date = DateTime.Now;
                        time = DateTime.Now;
                    }
                    else
                    {
                        var dateTimeParts = resolutionStr.Split('T');
                        var dateRegex = new Regex(@"([0-9X]{4})(-?W?([0-9X]{2})?(-?([0-9X]{2}))?)?");

                        if (dateRegex.IsMatch(dateTimeParts[0]))
                        {
                            date = ParseLuisDateString(dateTimeParts[0], attemptResolvePartialDates);

                            if (dateTimeParts.Length > 1)
                            {
                                time = ParseLuisTimeString(dateTimeParts[1]);
                            }
                        }
                        else
                        {
                            time = ParseLuisTimeString(resolutionStr);
                        }
                    }
                }
            }

            if (date.HasValue)
            {
                if (time.HasValue)
                {
                    return date.Value.Date + time.Value.TimeOfDay;
                }

                return date.Value;
            }

            if (time.HasValue)
            {
                return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time.Value.Hour, time.Value.Minute, time.Value.Second);
            }

            return new DateTime?();
        }

        /// <summary>
        /// Attempts to parse the date part of a LUIS date resolution string.
        /// </summary>
        /// <param name="value">The resolution string (or the time part of the resolution string) from a LUIS <see cref="EntityRecommendation"/>.</param>
        /// <param name="attemptResolvePartialDates">Indicates if partial dates should be resolved. e.g. '2016-02' or 'XXXX-WXX-4'. 
        /// If resolution is attempted then either the next occurance will be used, e.g. 'XXXX-WXX-02' (Tuesday) will resolve to the date of the next Tuesday or, 
        /// if a specific period is identified, such as '2016-02' (February 2016) the output will resolve to 2016-01-01, the first date of that period.</param>
        /// <returns>A DateTime with it's value set to the parsed date.  If only a time can be resolved then the current date is used to complete the date. If not date / time can be resolved then a null DateTime is returned.</returns>
        private static DateTime? ParseLuisDateString(string value, bool attemptResolvePartialDates = true)
        {
            DateTime resolvedDate;
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out resolvedDate))
            {
                return resolvedDate;
            }

            if (attemptResolvePartialDates)
            {
                // RESOLVED YEAR
                if (Regex.IsMatch(value, @"^[0-9]{4}$"))
                {
                    return new DateTime(Convert.ToInt16(value), 1, 1);
                }

                // MONTH & RESOLVED / UNRESOLVED YEAR
                if (Regex.IsMatch(value, @"^[0-9X]{4}(-([0-9]{2}))$"))
                {
                    var dateValueParts = value.Split('-');
                    if (dateValueParts[0] == "XXXX")
                    {
                        resolvedDate = new DateTime(DateTime.Now.Year, Convert.ToInt16(dateValueParts[1]), 1);
                        return resolvedDate < DateTime.Now ? resolvedDate.AddYears(1) : resolvedDate;
                    }

                    return new DateTime(Convert.ToInt16(dateValueParts[0]), Convert.ToInt16(dateValueParts[1]), 1);
                }

                // RESOLVED YEAR & WEEK NUMBER
                if (Regex.IsMatch(value, @"^[0-9]{4}(-W([0-9]{2}))$"))
                {
                    var dateValueParts = value.Split('-');
                    return FirstDateOfWeek(Convert.ToInt16(dateValueParts[0]), Convert.ToInt16(dateValueParts[1].TrimStart('W')));
                }

                // PARTIAL DAY OF WEEK
                if (Regex.IsMatch(value, @"^[0-9X]{4}-(WXX)(-[0-9]{1,2})$"))
                {
                    DayOfWeek dayOfWeek;
                    Enum.TryParse(value.Split('-')[2], out dayOfWeek);
                    return GetNextDateForSpecifiedDayOfWeek(dayOfWeek);
                }

                // PARTIAL DAY
                if (Regex.IsMatch(value, @"^[0-9X]{4}-([0-9X]{2})?(-[0-9]{1,2})$"))
                {
                    var year = DateTime.Now.Year;
                    var month = DateTime.Now.Month;

                    string[] dateValueParts = value.Split('-');

                    if (dateValueParts[0] != "XXXX")
                    {
                        year = Convert.ToInt16(dateValueParts[0]);
                    }

                    if (!dateValueParts[1].Contains("XX"))
                    {
                        month = Convert.ToInt16(dateValueParts[1]);
                    }

                    return new DateTime(year, month, Convert.ToInt16(dateValueParts[2]));
                }
            }

            return new DateTime?();
        }

        private static DateTime GetNextDateForSpecifiedDayOfWeek(DayOfWeek dayOfWeek)
        {
            var today = DateTime.Today;
            int daysUntilTuesday = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilTuesday);
        }

        /// <summary>
        /// Attempts to parse the time part of a LUIS date / time resolution string
        /// </summary>
        /// <param name="value">The resolution string (or the time part of the resolution string) from a LUIS <see cref="EntityRecommendation"/>.</param>
        /// <returns>A DateTime with it's value set to the parsed time, with the date part of the DateTime using DateTime.MinDate.  If no time can be resolved then a null DateTime is returned.
        /// 
        /// The following times are returned for the standard time strings returned from LUIS
        /// - *MO* (Morning) 9am
        /// - *MI* (Midday) 12pm
        /// - *AF* (Afternoon) 3pm
        /// - *EV* (Evening) 6pm
        /// - *NI* (Night) 8pm
        /// </returns>
        private static DateTime? ParseLuisTimeString(string value)
        {
            value = value.TrimStart('T');

            switch (value)
            {
                case "MO":
                    return DateTime.MinValue.AddHours(9);
                case "MI":
                    return DateTime.MinValue.AddHours(12);
                case "AF":
                    return DateTime.MinValue.AddHours(15);
                case "EV":
                    return DateTime.MinValue.AddHours(18);
                case "NI":
                    return DateTime.MinValue.AddHours(20);
                default:
                    var timeParts = value.Split(':');
                    int hours = 0;
                    int minutes = 0;

                    if (timeParts[0] != null)
                    {
                        int.TryParse(timeParts[0], out hours);
                    }

                    if (timeParts.Length > 1)
                    {
                        int.TryParse(timeParts[1], out minutes);
                    }

                    DateTime returnDate = DateTime.MinValue;

                    if (hours > 0)
                        returnDate = returnDate.AddHours(hours);

                    if (minutes > 0)
                        returnDate = returnDate.AddMinutes(minutes);

                    return returnDate;
            }
        }

        /// <summary>
        /// Parses a time from a users utterance.
        /// </summary>
        /// <param name="utterance">Text utterance to parse. The utterance is parsed using 
        /// the [Chronic.Signed](https://www.nuget.org/packages/Chronic.Signed/) library.</param>
        /// <returns>A nullable DateTime object with it's value set to the parsed time. If no time can be parsed then a null DateTime is returned.</returns>
        private static DateTime? RecognizeTime(string utterance)
        {
            try
            {
                var parser = new Parser();
                if (!string.IsNullOrEmpty(utterance))
                {
                    Span parsedObj = parser.Parse(utterance);
                    DateTime? parsedDateTime = parsedObj?.Start;
                    return parsedDateTime;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Calulates the first date of the week for a given week number in a given year.
        /// </summary>
        /// <param name="year">Specified year.</param>
        /// <param name="weekOfYear">Specified week number within the specified year.</param>
        /// <returns>DateTime object representing the first day of the week for <paramref name="weekOfYear"/> in <paramref name="year"/>.</returns>
        private static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            var januaryFirst = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - januaryFirst.DayOfWeek;

            var firstThursday = januaryFirst.AddDays(daysOffset);
            var calendar = CultureInfo.CurrentCulture.Calendar;
            var firstWeek = calendar.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }

    /// <summary>
    /// Represents a match found by <see cref="EntityRecognizer.FindAllMatches"/>
    /// </summary>
    internal class StringMatch
    {
        /// <summary>
        /// The string value of the match
        /// </summary>
        public string Choice { get; set; }

        /// <summary>
        /// The score of the match between 0 and 1 (100% match).
        /// </summary>
        public double Score { get; set; }
    }
}
