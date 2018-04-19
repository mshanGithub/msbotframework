using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Builder.Luis;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs
{
    public class LuisTraceInfo
    {
        [JsonProperty("luisResult")]
        public LuisResult LuisResult { set; get; }

        [JsonProperty("luisOptions")]
        public ILuisOptions LuisOptions { get; set; }

        [JsonProperty("luisModel")]
        public ILuisModel LuisModel { get; set; }
    }
}
