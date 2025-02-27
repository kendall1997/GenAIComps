// Copyright (C) 2024 Intel Corporation
// SPDX-License-Identifier: Apache-2.0

using MathSolver.Models;
using MathSolver.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MathSolver.Controllers;

[Route("v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class ChatController : ControllerBase
{
    /// <summary>
    /// Controller's logger
    /// </summary>
    private ILogger _logger;

    private IChatCompletition _chatCompletionService;

    public ChatController(ILogger logger, IChatCompletition chatCompletionService)
    {
        _logger = logger;
        _chatCompletionService = chatCompletionService;
    }

    /// <summary>
    /// Endpoint to get completions for a given math problem
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("completions")]
    public async Task<IActionResult> Completitions(ChatCompletionRequest request)
    {
        try
        {
            _logger.LogInformation("Request received");

            var data = await _chatCompletionService.ProcessRequest(request);

            if(data == null){
                return StatusCode(500, "Error processing the request");
            }

            return Ok(data);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error processing the request");
            return StatusCode(500, "Internal Server Error");
        }
    }

}
