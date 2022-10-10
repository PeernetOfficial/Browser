using System;

namespace Peernet.Browser.Application.Exceptions
{
    internal class PluginLoadingException : Exception
    {
        const string ExceptionMessage = "Exception loading {0} plugin.";

        public PluginLoadingException(string pluginName, Exception e)
            : base(string.Format(ExceptionMessage, pluginName), e)
        {
            PluginName = pluginName;
        }

        public string PluginName { get; }
    }
}
