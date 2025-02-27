// Copyright (C) 2024 Intel Corporation
// SPDX-License-Identifier: Apache-2.0

using System;
using MathSolver.Models;
using Microsoft.SemanticKernel;

namespace MathSolver.Services;

public interface IChatCompletition
{
    public Task<ChatCompletionResponse?> ProcessRequest(ChatCompletionRequest request);
}
