using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Kastra.Core.Business;
using Kastra.Core.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace Kastra.Core.Services
{
    public class JsonStringLocalizer<T> : IStringLocalizer<T> where T : class
    {
        private readonly IConfiguration _configuration;

        public JsonStringLocalizer(IHostingEnvironment app, IViewManager viewManager)
        {
            string objectNamespace = typeof(T).Namespace;
            string resourceName = typeof(T).Name;
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            if(objectNamespace.ToLower().Contains("module"))
            {
                ModuleDefinitionInfo moduleDefinition = viewManager.GetModuleDefsList().SingleOrDefault();
                string modulePath = Path.Combine(app.ContentRootPath, Constants.ModuleConfig.ModuleRootDirectory, moduleDefinition.Path, "Resources");
                configurationBuilder.SetBasePath(modulePath);
            }
            else
            {
                configurationBuilder.SetBasePath(Path.Combine(app.ContentRootPath, "Resources"));
            }

            configurationBuilder.AddJsonFile($"{resourceName}.json");
            configurationBuilder.AddJsonFile($"{resourceName}.{CultureInfo.CurrentUICulture.Name}.json", true);

            _configuration = configurationBuilder.Build();
        }

        public LocalizedString this[string name]
        {
            get
            {
                return this[name, new object[0]];
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                string value = _configuration[name];

                return new LocalizedString(
                    name,
                    String.Format(value ?? name, arguments),
                    value == null
                );
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
