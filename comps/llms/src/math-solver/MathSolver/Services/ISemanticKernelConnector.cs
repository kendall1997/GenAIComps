// Copyright (C) 2024 Intel Corporation
// SPDX-License-Identifier: Apache-2.0

using System;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.HuggingFace;

namespace MathSolver.Services;

public interface ISemanticKernelConnector
{
    public Task<FunctionResult> InvokeAsync(KernelArguments kernelArguments);
}
