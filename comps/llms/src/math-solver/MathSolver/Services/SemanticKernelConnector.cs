// Copyright (C) 2024 Intel Corporation
// SPDX-License-Identifier: Apache-2.0

using System;
using MathSolver.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.HuggingFace;
using Kernel = Microsoft.SemanticKernel.Kernel;

namespace MathSolver.Services;

public class SemanticKernelConnector : ISemanticKernelConnector
{
    private IConfiguration _config;
    private readonly IKernelBuilder _builder;
    private readonly Kernel _kernel;
    private KernelPlugin? _pluginFunctions;

    public SemanticKernelConnector(IConfiguration config)
    {
        _config = config;
        _builder = Kernel.CreateBuilder();

        var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromMinutes(10);

        _kernel = _builder.
            AddHuggingFaceChatCompletion(
                model: _config.GetValue<string>("LLM_MODEL_ID") ?? string.Empty,
                endpoint: new Uri(_config.GetValue<string>("LLM_ENDPOINT") ?? string.Empty),
                httpClient: httpClient
            )
        .Build();
    }

    /// <summary>
    /// Invokes the Semantic Kernel library using the parameters given
    /// </summary>
    /// <param name="executionSettings">Model settings</param>
    /// <param name="kernelArguments">Parameters to be given into the LLM</param>
    /// <returns></returns>
    public async Task<FunctionResult> InvokeAsync(KernelArguments kernelArguments)
    {
        // Load plugin directory
        var funPluginDirectoryPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
            "PluginPrompt",
            "MathPlugin");

        if(_pluginFunctions == null){
            // Load the Plugin from the Plugins Directory
            _pluginFunctions = _kernel.ImportPluginFromPromptDirectory(funPluginDirectoryPath);
        }

        // Run the Function
        FunctionResult result = await _kernel.InvokeAsync(_pluginFunctions["Problem"], kernelArguments);

        return result;
    }
}
