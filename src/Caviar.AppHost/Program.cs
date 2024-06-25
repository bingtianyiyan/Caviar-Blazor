// Copyright (c) BeiYinZhiNian (1031622947@qq.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: http://www.caviar.wang/ or https://gitee.com/Cherryblossoms/caviar.

using Caviar.AppHost;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddForwardedHeaders();
var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";
var webApp = builder.AddProject<Projects.Caviar_AntDesignBlazor>("CaviarAntDesignBlazor",launchProfileName)
      //The OTLP endpoint. This endpoint hosts an OTLP service and receives telemetry. When the dashboard is launched by the .NET Aspire app host this address is secured with HTTPS.
      //Securing the dashboard with HTTPS is recommended.
      // 修改这边 Standalone Aspire dashboard 地址为自己部署或者本地的地址
      .WithEnvironment("DOTNET_DASHBOARD_URL", "http://192.168.99.100:18888")
       .WithEnvironment("OTEL_EXPORTER_OTLP_PROTOCOL", "grpc")
      .WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "http://192.168.99.100:4317")
    .WithExternalHttpEndpoints();
builder.Build().Run();

static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "CAVIAR_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}
