using System;

namespace Microsoft.Bot.Builder.Extensions.Telemetry
{
    /// <summary>
    /// Interface IDateTimeProvider
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Curernt DateTime.
        /// </summary>
        /// <returns>DateTimeOffset.</returns>
        DateTimeOffset Now();
    }
}