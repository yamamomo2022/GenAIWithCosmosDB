using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using OpenAI;
using OpenAI.Embeddings;

namespace EchoBot.Services
{
    public class GptService : IGptService
    {
        private string _apiKey;
        public GptService(IConfiguration configuration)
        {
            _apiKey = configuration["OpenAIApiKey"];
            OpenAIClient _client = new(_apiKey);
        }

        /// <summary>
        /// ChatGPTを使用してユーザーのプロンプトに応答します。
        /// </summary>
        /// <param name="prompt">ユーザーからの入力プロンプト</param>
        /// <returns>ChatGPTからの応答テキスト</returns>
        public async Task<string> GetChatResponseAsync(string prompt, string model = "gpt-4o-mini")
        {
            ChatClient _chatClient = new(model: model, _apiKey);
            ChatCompletion completion = await _chatClient.CompleteChatAsync(prompt);
            
            return completion.ToString();
        }

        /// <summary>
        /// OpenAIのEmbeddings APIを使用してテキストの埋め込みを取得します。
        /// </summary>
        /// <param name="text">埋め込みを取得するテキスト</param>
        /// <returns>テキストの埋め込みベクトル</returns>
        public async Task<float[]> GetEmbeddingAsync(string text, string model = "text-embedding-3-small")
        {
            EmbeddingClient _embeddingClient = new(model: model, _apiKey);
            Embedding embedding = await _embeddingClient.GenerateEmbeddingAsync(text);
            ReadOnlyMemory<float> vector = embedding.Vector;
            
            return vector.ToArray();
        }
    }
}
