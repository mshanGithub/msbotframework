using System;
using System.Configuration;

namespace Microsoft.Bot.Connector
{
    /// <summary>
    /// Credential provider which uses config settings to lookup appId and password
    /// </summary>
    public sealed class SettingsCredentialProvider : SimpleCredentialProvider
    {
        public SettingsCredentialProvider(string appIdSettingName = null, string appPasswordSettingName = null)
        {
            var appIdKey = appIdSettingName ?? MicrosoftAppCredentials.MicrosoftAppIdKey;
            var passwordKey = appPasswordSettingName ?? MicrosoftAppCredentials.MicrosoftAppPasswordKey;
            this.AppId = ConfigurationManager.AppSettings[appIdKey] ?? Environment.GetEnvironmentVariable(appIdKey, EnvironmentVariableTarget.Process);
            this.Password = ConfigurationManager.AppSettings[passwordKey] ?? Environment.GetEnvironmentVariable(passwordKey, EnvironmentVariableTarget.Process);
        }
    }
}
