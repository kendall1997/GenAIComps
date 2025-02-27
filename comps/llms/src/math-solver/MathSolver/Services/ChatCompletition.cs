// Copyright (C) 2024 Intel Corporation
// SPDX-License-Identifier: Apache-2.0

using System;
using MathSolver.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.HuggingFace;

namespace MathSolver.Services;

public class ChatCompletition : IChatCompletition
{
    private IConfiguration _config;

    private ISemanticKernelConnector _semanticKernelConnector;

    public ChatCompletition(IConfiguration config, ISemanticKernelConnector semanticKernelConnector)
    {
        _config = config;
        _semanticKernelConnector = semanticKernelConnector;
    }

    /// <summary>
    /// Handles each of the requests coming from the Controller
    /// </summary>
    /// <param name="request">Request from the client</param>
    /// <returns>Response from the SK/LLM service</returns>
    public async Task<ChatCompletionResponse?> ProcessRequest(ChatCompletionRequest request)
    {

        // Build arguments
        var huggingFacePromptExecutionSettings = GetHuggingFacePromptExecutionSettings(request);
        var arguments = GetKernelArguments(request, huggingFacePromptExecutionSettings);

        // Call the SK/LLM service
        FunctionResult data = await _semanticKernelConnector.InvokeAsync(arguments);

        // Create the response
        var response = CreateResponse(data);

        return response;
    }

    /// <summary>
    /// Extracts the input from the request
    /// </summary>
    /// <param name="request">HTTP Request DTO</param>
    /// <returns>The request as string</returns>
    private string GetInput(ChatCompletionRequest request)
    {
        if(request.Messages == null || request.Messages.Count == 0){
            return string.Empty;
        }

        return request.Messages[0]["content"];
    }

    /// <summary>
    /// Extracts the settings from the request
    /// </summary>
    /// <param name="request">HTTP Request DTO</param>
    /// <returns>The HuggingFacePromptExecutionSettings created</returns>
    private HuggingFacePromptExecutionSettings GetHuggingFacePromptExecutionSettings(ChatCompletionRequest request)
    {
        return new HuggingFacePromptExecutionSettings()
        {
            Temperature = request.Temperature,
            MaxTokens = request.MaxTokens,
            TopP = request.TopP,
            RepetitionPenalty = request.FrequencyPenalty,
            UseCache = true,
            WaitForModel = true,
            ResultsPerPrompt = request.N,
            LogProbs = request.Logprobs,
            Seed = request.Seed,
            Stop = request.Stop,
            TopLogProbs = request.TopLogprobs,
            Details = true,
            ModelId = request.Model
        };
    }

    /// <summary>
    /// Extracts the arguments from the request
    /// </summary>
    /// <param name="request">HTTP Request DTO</param>
    /// <param name="huggingFacePromptExecutionSettings">The HuggingFacePromptExecutionSettings</param>
    /// <returns>The KernelArguments</returns>
    private KernelArguments GetKernelArguments(ChatCompletionRequest request, HuggingFacePromptExecutionSettings huggingFacePromptExecutionSettings)
    {
        return new KernelArguments(huggingFacePromptExecutionSettings)
        {
            ["input"] = GetInput(request)
        };
    }

    /// <summary>
    /// Creates the response to be sent back to the client
    /// </summary>
    /// <param name="data">Response from Semantic Kernel call to the LLM</param>
    /// <returns>The DTO populated</returns>
    private ChatCompletionResponse? CreateResponse(FunctionResult data){

        var response = new ChatCompletionResponse();

        if(data == null){
            return null;
        }

        string? textResponse = string.Empty;

        object? dataId = string.Empty;
        object? dataObject = string.Empty;
        object? dataCreated = 0;
        object? dataFinishReason = string.Empty;
        object? dataModel = string.Empty;
        object? dataCompletionTokens = 0;
        object? dataPromptTokens = 0;
        object? dataTotalTokens = 0;

        if(data != null && data.Metadata != null){
            data.Metadata.TryGetValue("Id",out dataId);
            data.Metadata.TryGetValue("Model",out dataModel);
            data.Metadata.TryGetValue("Object",out dataObject);
            data.Metadata.TryGetValue("Created",out dataCreated);
            data.Metadata.TryGetValue("FinishReason",out dataFinishReason);
            data.Metadata.TryGetValue("UsageCompletionTokens",out dataCompletionTokens);
            data.Metadata.TryGetValue("UsagePromptTokens",out dataPromptTokens);
            data.Metadata.TryGetValue("UsageTotalTokens",out dataTotalTokens);

            textResponse = data.ToString();
        }

        // Top Level elements
        response.Id = (string) (dataId ?? string.Empty);
        response.Model = (string) (dataModel ?? string.Empty);
        response.Object = (string) (dataObject ?? string.Empty);
        response.Created = (long) (dataCreated ?? 0);

        // Choises
        response.Choices.Add(new ChatCompletionResponseChoice {
            Index = 0,
            Message = new ChatMessage{
                Role = "system",
                Content = textResponse,
            },
            FinishReason = (string) (dataFinishReason ?? string.Empty)
        });

        // Usage information
        response.Usage.CompletionTokens = (int) (dataCompletionTokens ?? 0);
        response.Usage.PromptTokens = (int) (dataPromptTokens ?? 0);
        response.Usage.TotalTokens = (int) (dataTotalTokens ?? 0);

        return response;
    }
}
