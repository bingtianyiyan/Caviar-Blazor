using Caviar.SharedKernel.Entities;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Caviar.AntDesignBlazor.Client
{
    public class LanguageService
    {
        public static List<(string CultureName, string ResourceName)> GetLanguageList()
        {
            var _resourcesAssembly = Assembly.GetExecutingAssembly();
            var availableResources = _resourcesAssembly
                    .GetManifestResourceNames()
                    .Select(x => Regex.Match(x, @"^.*Resources.Language\.(.+)\.json"))
                    .Where(x => x.Success)
                    .Select(x => (CultureName: x.Groups[1].Value, ResourceName: x.Value))
                    .ToList();
            return availableResources;
        }

        public static string UserLanguage(string name)
        {
            var _resourcesAssembly = Assembly.GetExecutingAssembly();
            var availableResources = GetLanguageList();
            var (_, resourceName) = availableResources.FirstOrDefault(x => x.CultureName.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (resourceName == null)
            {
                (_, resourceName) = availableResources.FirstOrDefault(x => x.CultureName.Equals(CurrencyConstant.DefaultLanguage, StringComparison.OrdinalIgnoreCase));
            }
            using var fileStream = _resourcesAssembly.GetManifestResourceStream(resourceName);
            using var streamReader = new StreamReader(fileStream);
            return streamReader.ReadToEnd();
        }
    }
}
