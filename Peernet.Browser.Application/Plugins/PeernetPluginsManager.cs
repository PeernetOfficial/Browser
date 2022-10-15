using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Managers;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
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
        private readonly INotificationsManager notificationsManager;

        public PeernetPluginsManager(ISettingsManager settingsManager, INotificationsManager notificationsManager)
        {
            this.settingsManager = settingsManager;
            this.notificationsManager = notificationsManager;
        }

        public List<IPlugin> Plugins { get; } = new();

        public void LoadPlugins(IServiceCollection services)
        {
            if (settingsManager.PluginsLocation != null)
            {
                var pluginsAbsoluteLocation = Path.Combine(Directory.GetCurrentDirectory(), settingsManager.PluginsLocation);
                Directory.CreateDirectory(pluginsAbsoluteLocation);
                var plugins = Directory.GetDirectories(pluginsAbsoluteLocation);
                foreach (var pluginPath in plugins)
                {
                    var path = GetDllPath(pluginPath);
                    var assembly = Assembly.LoadFrom(path);
                    LoadPlugin(assembly, services);
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

        private void LoadPlugin(Assembly assembly, IServiceCollection services)
        {
            try
            {
                var types = assembly.GetTypes().ToList();
                var type = types.Find(a => typeof(IPlugin).IsAssignableFrom(a));
                if (type != null)
                {
                    var instance = (IPlugin)Activator.CreateInstance(type);
                    Plugins.Add(instance);

                    instance.Load(services);
                }
            }
            catch (Exception e)
            {
                notificationsManager.Notifications.Add(new($"Failed to load {assembly.FullName}. Continuing...", e.Message, Severity.Warning, e));
            }
        }
    }
}