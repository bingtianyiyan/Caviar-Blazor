using Caviar.SharedKernel.Entities;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Caviar.AntDesignUI;
using Caviar.AntDesignBlazor.Client;

namespace Caviar.AntDesignBlazor.Client
{

    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            var baseAddress = new Uri(builder.HostEnvironment.BaseAddress + CurrencyConstant.Api);
            builder.Services.AddScoped(sp =>
            {
                return new HttpClient() { BaseAddress = baseAddress };
            });
            builder.AddCavWasm();
            builder.Services.AddAdminCaviar(new Type[] { typeof(Program)});
            PublicInit();

            await builder.Build().RunAsync();
        }

        /// <summary>
        /// service和wasm公共初始化
        /// </summary>
        public static void PublicInit()
        {
#if DEBUG
            Config.IsDebug = true;
#endif
            InAssemblyLanguageService.UserLanguage = LanguageService.UserLanguage;
            InAssemblyLanguageService.GetUserLanguageList = LanguageService.GetLanguageList;
        }
    }
}
