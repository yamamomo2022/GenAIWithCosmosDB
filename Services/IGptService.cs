using System.Threading.Tasks;

namespace EchoBot.Services;

public interface IGptService
{
    Task<string> GetChatResponseAsync(string prompt, string model);
    Task<float[]> GetEmbeddingAsync(string text, string embeddingModel);

}
