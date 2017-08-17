using System;

namespace Microsoft.Bot.Builder.Extensions.Telemetry
{
    /// <summary>
    /// Class DateTimeProvider.
    /// </summary>
    /// <seealso cref="Microsoft.Bot.Builder.Extensions.Telemetry.IDateTimeProvider" />
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// Curernt DateTime.
        /// </summary>
        /// <returns>DateTimeOffset.</returns>
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}