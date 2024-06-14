// Copyright (c) BeiYinZhiNian (1031622947@qq.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: http://www.caviar.wang/ or https://gitee.com/Cherryblossoms/caviar.

using Caviar.AppHost;

var builder = DistributedApplication.CreateBuilder(args);
builder.AddForwardedHeaders();
var launchProfileName = ShouldUseHttpForEndpoints() ? "http" : "https";

var webApp = builder.AddProject<Projects.Caviar_AntDesignBlazor>("CaviarAntDesignBlazor",launchProfileName)
    .WithExternalHttpEndpoints();
builder.Build().Run();

static bool ShouldUseHttpForEndpoints()
{
    const string EnvVarName = "CAVIAR_USE_HTTP_ENDPOINTS";
    var envValue = Environment.GetEnvironmentVariable(EnvVarName);

    // Attempt to parse the environment variable value; return true if it's exactly "1".
    return int.TryParse(envValue, out int result) && result == 1;
}
