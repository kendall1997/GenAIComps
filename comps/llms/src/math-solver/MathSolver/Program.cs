// Copyright (C) 2024 Intel Corporation
// SPDX-License-Identifier: Apache-2.0

using MathSolver.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Newtonsoft.Json.Serialization;

#region Builder
var builder = WebApplication.CreateBuilder(args);
#endregion

#region Logging
var logger = LoggerFactory.Create(config => {
    config.AddConsole();
    config.AddConfiguration(builder.Configuration.GetSection("Logging"));
}).CreateLogger("Program");
#endregion

#region Configuration
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
#endregion

#region Dependencies
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IChatCompletition, ChatCompletition>();
builder.Services.AddSingleton<ISemanticKernelConnector, SemanticKernelConnector>();
builder.Services.AddSingleton<ILogger>(logger);
#endregion

#region OpenAPI
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
#endregion

#region Versioning
builder.Services.AddApiVersioning(o => {
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(
        new HeaderApiVersionReader("X-Version"),
        new QueryStringApiVersionReader("api-version"),
        new MediaTypeApiVersionReader("ver"));

});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
#endregion

#region Application
// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion