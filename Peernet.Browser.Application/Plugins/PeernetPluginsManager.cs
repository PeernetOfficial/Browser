using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Exceptions;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Plugins;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Peernet.Browser.Application.Plugins
{
    public class PeernetPluginsManager
    {
        private const string PeernetBrowserPluginsAssemblyPattern = "Peernet.Browser.Plugins.";
        private readonly ISettingsManager settingsManager;

        public PeernetPluginsManager(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public List<IPlugin> Plugins { get; } = new();

        public void LoadPlugins(ServiceCollection services)
        {
            if (settingsManager.PluginsLocation != null)
            {
                var pluginsAbsoluteLocation = Path.Combine(Directory.GetCurrentDirectory(), settingsManager.PluginsLocation);
                Directory.CreateDirectory(pluginsAbsoluteLocation);
                var plugins = Directory.GetDirectories(pluginsAbsoluteLocation);
                foreach (var pluginPath in plugins)
                {
                    var path = GetDllPath(pluginPath);
                    var dll = Assembly.LoadFrom(path);
                    var types = dll.GetTypes().ToList();
                    var type = types.Find(a => typeof(IPlugin).IsAssignableFrom(a));
                    if (type != null)
                    {
                        var instance = (IPlugin)Activator.CreateInstance(type);
                        Plugins.Add(instance);
                        try
                        {
                            instance.Load(services);
                        }
                        catch (Exception e)
                        {
                            throw new PluginLoadingException(type.Name, e);
                        }
                    }
                }
            }
        }

        private string GetDllPath(string pluginPath)
        {
            var files = Directory.GetFiles(pluginPath, "*.dll");
            foreach (var file in files)
            {
                var assemblyName = FileVersionInfo.GetVersionInfo(file).ProductName;
                if (assemblyName.StartsWith(PeernetBrowserPluginsAssemblyPattern))
                {
                    return file;
                }
            }
            return string.Empty;
        }
    }
}