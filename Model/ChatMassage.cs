using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EchoBot.Model;

public class ChatMessage
{
    /// <summary>
    /// Unique identifier for each document in Cosmos DB
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Unique identifier for the conversation
    /// </summary>
    [JsonPropertyName("conversationId")]
    public string ConversationId { get; set; }

    /// <summary>
    /// Sender of the message (User or Assistant)
    /// </summary>
    [JsonPropertyName("sender")]
    public string Sender { get; set; } // Example: "User", "Assistant"

    /// <summary>
    /// Content of the message
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// Timestamp when the message was sent
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Information about the embedding model used
    /// </summary>
    [JsonPropertyName("embeddingModel")]
    public string EmbeddingModel { get; set; }

    /// <summary>
    /// Embedding vector for RAG
    /// </summary>
    [JsonPropertyName("embedding")]
    public float[] Embedding { get; set; }
}

public class UserMessage : ChatMessage
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; }
}

public class AssistantMessage : ChatMessage
{

    /// <summary>
    /// Information about the AI generation model used
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("responseTime")]
    public DateTime ResponseTime { get; set; }
}
