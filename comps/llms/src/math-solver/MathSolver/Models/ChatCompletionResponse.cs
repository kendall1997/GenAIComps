// Copyright (C) 2024 Intel Corporation
// SPDX-License-Identifier: Apache-2.0

namespace MathSolver.Models{
    #region Using
    using Newtonsoft.Json;
    #endregion

    public class ChatCompletionResponse
    {
        /// <summary>
        /// Chat id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Object of the chat
        /// </summary>
        [JsonProperty(PropertyName = "object")]
        public string Object { get; set; } = "chat.completition";

        /// <summary>
        /// Timestamp when the chat was created
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public long Created { get; set; } = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

        /// <summary>
        /// Model processing the chat
        /// </summary>
        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; } = String.Empty;

        /// <summary>
        /// Model different responses
        /// </summary>
        [JsonProperty(PropertyName = "choices")]
        public List<ChatCompletionResponseChoice> Choices { get; set; } = new List<ChatCompletionResponseChoice>();

        /// <summary>
        /// Usage Info
        /// </summary>
        [JsonProperty(PropertyName = "usage")]
        public UsageInfo Usage { get; set; } = new UsageInfo();
    }

    public class ChatCompletionResponseChoice
    {
        /// <summary>
        /// Choise index
        /// </summary>
        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        /// <summary>
        /// Message response
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public ChatMessage Message { get; set; } = new ChatMessage();

        /// <summary>
        /// Model finish reason
        /// </summary>
        [JsonProperty(PropertyName = "finish_reason")]
        public string FinishReason { get; set; } = String.Empty;

        /// <summary>
        /// Model response metadata
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public string Metadata { get; set; } = String.Empty;

    }

    public class ChatMessage
    {
        /// <summary>
        /// Chat Message response role
        /// </summary>
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; } = String.Empty;

        /// <summary>
        /// Chat Message response content
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; } = String.Empty;
    }

    public class UsageInfo
    {
        /// <summary>
        /// Amount of tokens in the prompt
        /// </summary>
        [JsonProperty(PropertyName = "prompt_tokens")]
        public int PromptTokens { get; set; }

        /// <summary>
        /// Amount of tokens in the prompt
        /// </summary>
        [JsonProperty(PropertyName = "total_tokens")]
        public int TotalTokens { get; set; }

        /// <summary>
        /// Amount of completion tokens
        /// </summary>
        [JsonProperty(PropertyName = "completion_tokens")]
        public int CompletionTokens { get; set; }
    }
}

