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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Bot.Builder.Dialogs.Utilities;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Builder.Tests
{
    [TestClass]
    public sealed class EntityRecognizerTests
    {
        public static EntityRecommendation EntityFor(string type, string entity,
            string resolutionType = null, string resolutionValue = null)
        {
            var entityRecommendation = new EntityRecommendation(type: type) { Entity = entity };
            if (!string.IsNullOrEmpty(resolutionValue) && !string.IsNullOrEmpty(resolutionType))
            {
                entityRecommendation.Resolution = new Dictionary<string, string> { { resolutionType, resolutionValue } };
            }

            return entityRecommendation;
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Not_Resolved_If_Partial_Resolution_Disabled()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.date", "february", "date", "XXXX-WXX-02"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate, false);
            var dateTimeToMatch = GetNextDateForSpecifiedDayOfWeek(DayOfWeek.Tuesday);

            Assert.AreEqual(null, parsedDate);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_Day_Of_Week_Only()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.date", "february", "date", "XXXX-WXX-02"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = GetNextDateForSpecifiedDayOfWeek(DayOfWeek.Tuesday);

            Assert.AreEqual(dateTimeToMatch, parsedDate.Value);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_Partial_Month_Only()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.date", "february", "date", "XXXX-02"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = new DateTime(2016, 2, 1);
            if (dateTimeToMatch < DateTime.Now)
            {
                dateTimeToMatch = dateTimeToMatch.AddYears(1);
            }

            Assert.AreEqual(dateTimeToMatch, parsedDate.Value);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_Partial_Year_And_Month_Only()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.date", "february 2016", "date", "2016-02"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = new DateTime(2016, 2, 1);

            Assert.AreEqual(dateTimeToMatch, parsedDate.Value);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_Partial_Year_Only()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.date", "this year", "date", "2016"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = new DateTime(2016, 1, 1);

            Assert.AreEqual(dateTimeToMatch, parsedDate.Value);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_PRESENT_REF()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.time", "now", "time", "PRESENT_REF"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = DateTime.Now;

            Assert.IsTrue((parsedDate - dateTimeToMatch) < new TimeSpan(0, 0, 1, 0));
        }

        [TestMethod]
        public void DateTime_Parsed_With_Hours_And_Minutes_From_Entities()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05T09:15"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = new DateTime(2016, 08, 05, 09, 15, 0);

            Assert.AreEqual(dateTimeToMatch, parsedDate);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_Null_When_No_Date_Found()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.number", "3.4"));
            entities.Add(EntityFor("builtin.number", "3.4"));
            entities.Add(EntityFor("builtin.number", "3"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);

            Assert.AreEqual(null, parsedDate);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_Time_Entity_Only()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.number", "3.4"));
            entities.Add(EntityFor("builtin.datetime.time", "midday", "time", "TNI"));
            entities.Add(EntityFor("builtin.number", "3"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);

            Assert.AreEqual(dateTimeToMatch, parsedDate);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_Date_Only()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.number", "3.4"));
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "3"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = new DateTime(2016, 08, 05, 0, 0, 0);

            Assert.AreEqual(dateTimeToMatch, parsedDate);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Entities_LUIS_Resolved_Date_Entity_And_Time_Entity()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "3"));
            entities.Add(EntityFor("builtin.datetime.time", "morning", "time", "TMO"));
            entities.Add(EntityFor("builtin.datetime.time", "night", "time", "TNI"));

            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime(entities, out parsedDate);
            var dateTimeToMatch = new DateTime(2016, 08, 05, 9, 00, 0);

            Assert.AreEqual(dateTimeToMatch, parsedDate);
        }

        #region BestMatch Tests

        [TestMethod]
        public void Best_Match_Returns_Null_For_No_Match()
        {
            string[] choices = { "Hi", "Hello There!", "How are you?", "How're you?", "Goodbye", "Bye" };

            // no match due to threshold too high
            var bestMatch = EntityRecognizer.FindBestMatch(choices, "hello", 1, true, true);
            Assert.AreEqual(null, bestMatch);

            // no match due to match case
            bestMatch = EntityRecognizer.FindBestMatch(choices, "hi", 1, false, true);
            Assert.AreEqual(null, bestMatch);

            // no match due to not ignoring non alphanumeric
            bestMatch = EntityRecognizer.FindBestMatch(choices, "Howre you", 0.5, true, false);
            Assert.AreEqual(null, bestMatch);
        }

        [TestMethod]
        public void Best_Match_Sucessfully_Found_From_String_Array()
        {
            string[] choices = { "Hi", "Hello There!", "How are you?", "Goodbye", "Bye" };

            var bestMatch = EntityRecognizer.FindBestMatch(choices, "hello", 0.4, true, true);
            Assert.AreEqual("Hello There!", bestMatch);

            bestMatch = EntityRecognizer.FindBestMatch(choices, "Hi", 1, true, true);
            Assert.AreEqual("Hi", bestMatch);

            bestMatch = EntityRecognizer.FindBestMatch(choices, "how are you", 1, true, true);
            Assert.AreEqual("How are you?", bestMatch);
        }

        [TestMethod]
        public void Best_Match_Sucessfully_Found_From_Delimited_List()
        {
            var choices = "Hi|Hello There!|How are you?|Goodbye|Bye";

            var bestMatch = EntityRecognizer.FindBestMatch(choices, "hello", 0.4, true, true);
            Assert.AreEqual("Hello There!", bestMatch);

            bestMatch = EntityRecognizer.FindBestMatch(choices, "Hi", 1, true, true);
            Assert.AreEqual("Hi", bestMatch);

            bestMatch = EntityRecognizer.FindBestMatch(choices, "how are you", 1, true, true);
            Assert.AreEqual("How are you?", bestMatch);
        }

        [TestMethod]
        public void Best_Match_Sucessfully_Found_From_Delimited_List_Custom_Seperator()
        {
            var choices = "Hi,Hello There!,How are you?,Goodbye,Bye";

            var bestMatch = EntityRecognizer.FindBestMatch(choices, "hello", 0.4, true, true, ',');
            Assert.AreEqual("Hello There!", bestMatch);

            bestMatch = EntityRecognizer.FindBestMatch(choices, "Hi", 1, true, true, ',');
            Assert.AreEqual("Hi", bestMatch);

            bestMatch = EntityRecognizer.FindBestMatch(choices, "how are you", 1, true, true, ',');
            Assert.AreEqual("How are you?", bestMatch);
        }

        #endregion

        #region DateTime Parsing from Utterances Tests

        [TestMethod]
        public void DateTime_Parsed_From_Invalid_Utterance_Returns_Null()
        {
            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime("Hi, How are you?", out parsedDate);
            Assert.AreEqual(null, parsedDate);
        }

        [TestMethod]
        public void DateTime_Parsed_From_Utterance()
        {
            DateTime? parsedDate;
            EntityRecognizer.ParseDateTime("tomorrow", out parsedDate);
            var dateTimeToMatch = DateTime.Now.Date.AddDays(1);
            Assert.AreEqual(dateTimeToMatch, parsedDate);

            EntityRecognizer.ParseDateTime("tomorrow at 9:30am", out parsedDate);
            dateTimeToMatch = DateTime.Now.Date.AddDays(1).AddHours(9).AddMinutes(30);
            Assert.AreEqual(dateTimeToMatch, parsedDate);

            EntityRecognizer.ParseDateTime("yesterday at 5:30pm", out parsedDate);
            dateTimeToMatch = DateTime.Now.Date.AddDays(-1).AddHours(17).AddMinutes(30);
            Assert.AreEqual(dateTimeToMatch, parsedDate);
        }

        #endregion

        #region Find Entity Tests

        [TestMethod]
        public void FindEntities_Returns_Empty_List_When_No_Matching_Entities_Of_Type()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.time", "midday", "time", "2016-08-05TMI"));
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "3"));
            entities.Add(EntityFor("builtin.datetime.time", "night", "time", "TNI"));

            var foundEntities = EntityRecognizer.FindEntities(entities, "builtin.datetime.duration");

            Assert.IsNotNull(foundEntities);
            Assert.IsTrue(foundEntities.Count == 0);
        }

        [TestMethod]
        public void FindEntities_Finds_All_Entities_Of_Type()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.time", "midday", "time", "2016-08-05TMI"));
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "3"));
            entities.Add(EntityFor("builtin.datetime.time", "night", "time", "TNI"));

            var foundEntities = EntityRecognizer.FindEntities(entities, "builtin.datetime.time");

            Assert.IsTrue(foundEntities.First() == entities.First());
            Assert.AreEqual(foundEntities.Count, 2);
        }

        [TestMethod]
        public void FindEntity_Finds_First_Entity_Of_Type()
        {
            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.time", "midday", "time", "2016-08-05TMI"));
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "3"));
            entities.Add(EntityFor("builtin.datetime.time", "night", "time", "TNI"));

            var foundEntity = EntityRecognizer.FindEntity(entities, "builtin.datetime.date");

            Assert.AreEqual(foundEntity, entities.Skip(1).FirstOrDefault());
        }

        #endregion

        #region Parse Boolean and Numbers Tests

        [TestMethod]
        public void All_Numbers_Parsed_From_Entities_Correctly()
        {
            double parsedNumber;

            var entities = new List<EntityRecommendation>();
            entities.Add(EntityFor("builtin.datetime.time", "midday", "time", "2016-08-05TMI"));
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "50"));
            entities.Add(EntityFor("builtin.datetime.time", "night", "time", "TNI"));
            EntityRecognizer.ParseNumber(entities, out parsedNumber);
            Assert.AreEqual(50, parsedNumber);

            entities.Clear();
            entities.Add(EntityFor("builtin.datetime.time", "midday", "time", "2016-08-05TMI"));
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "-50"));
            entities.Add(EntityFor("builtin.datetime.time", "night", "time", "TNI"));
            EntityRecognizer.ParseNumber("-50", out parsedNumber);
            Assert.AreEqual(-50, parsedNumber);

            entities.Clear();
            entities.Add(EntityFor("builtin.datetime.time", "midday", "time", "2016-08-05TMI"));
            entities.Add(EntityFor("builtin.datetime.date", "tomorrow", "date", "2016-08-05"));
            entities.Add(EntityFor("builtin.number", "5.5"));
            entities.Add(EntityFor("builtin.datetime.time", "night", "time", "TNI"));
            EntityRecognizer.ParseNumber("5.5", out parsedNumber);
            Assert.AreEqual(5.5, parsedNumber);
        }

        [TestMethod]
        public void All_Numbers_Parsed_From_Utterance_Correctly()
        {
            double parsedNumber;
            EntityRecognizer.ParseNumber("50", out parsedNumber);
            Assert.AreEqual(50, parsedNumber);

            EntityRecognizer.ParseNumber("-50", out parsedNumber);
            Assert.AreEqual(-50, parsedNumber);

            EntityRecognizer.ParseNumber("5.5", out parsedNumber);
            Assert.AreEqual(5.5, parsedNumber);
        }

        [TestMethod]
        public void All_Booleans_Parsed_From_Utterance_Correctly()
        {
            bool? parsedBoolean;
            EntityRecognizer.ParseBoolean("yes", out parsedBoolean);
            Assert.AreEqual(true, parsedBoolean);

            EntityRecognizer.ParseBoolean("yes please", out parsedBoolean);
            Assert.AreEqual(true, parsedBoolean);

            EntityRecognizer.ParseBoolean("no thanks", out parsedBoolean);
            Assert.AreEqual(false, parsedBoolean);

            EntityRecognizer.ParseBoolean("false", out parsedBoolean);
            Assert.AreEqual(false, parsedBoolean);

            EntityRecognizer.ParseBoolean("true", out parsedBoolean);
            Assert.AreEqual(true, parsedBoolean);

            EntityRecognizer.ParseBoolean("2", out parsedBoolean);
            Assert.AreEqual(false, parsedBoolean);

            EntityRecognizer.ParseBoolean("1", out parsedBoolean);
            Assert.AreEqual(true, parsedBoolean);

            EntityRecognizer.ParseBoolean("invalid answer", out parsedBoolean);
            Assert.AreEqual(null, parsedBoolean);
        }

        #endregion

        private static DateTime GetNextDateForSpecifiedDayOfWeek(DayOfWeek dayOfWeek)
        {
            DateTime today = DateTime.Today;
            int daysUntilTuesday = ((int)dayOfWeek - (int)today.DayOfWeek + 7) % 7;
            return today.AddDays(daysUntilTuesday);
        }
    }
}
