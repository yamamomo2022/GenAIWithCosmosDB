
using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using EchoBot.Services;
using EchoBot.Model;


namespace EchoBot.Bots;
// Represents a bot saves and echoes back user input.
public class EchoBot : ActivityHandler
{
   // variable used to save user input to CosmosDb Storage.
   private readonly IStorage _myStorage;
   private readonly IGptService _gptService;

   private string _model = "gpt-4o-mini";
   private string _embeddingModel = "text-embedding-3-small";


   // Create cancellation token (used by Async Write operation).
   public CancellationToken cancellationToken { get; private set; }

   public EchoBot(IStorage storage,IGptService gptService)
   {
      if (storage is null) throw new ArgumentNullException();
      _myStorage = storage;

      _gptService = gptService;
   }


   // Echo back user input.
   protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
   {
      // preserve user input.
      var message = turnContext.Activity.Text;
      var embedding = await _gptService.GetEmbeddingAsync(text:message,embeddingModel:_embeddingModel);

      var userMessage = new UserMessage
      {
         ConversationId = turnContext.Activity.Conversation.Id,
         Sender = "User",
         Message = message,
         EmbeddingModel = _embeddingModel,
         Embedding = embedding 
      };

      var Response = await _gptService.GetChatResponseAsync(prompt : message, model:_model);
      embedding = await _gptService.GetEmbeddingAsync(text:Response,embeddingModel:_embeddingModel);

      await turnContext.SendActivityAsync(Response);

      var assistantMessage = new AssistantMessage
      {
         ConversationId = turnContext.Activity.Conversation.Id,
         Sender = "Assistant",
         Message = Response,
         EmbeddingModel = _embeddingModel,
         Embedding = embedding,
         Model = _model,
         ResponseTime = DateTime.Now
      };
      
      var currentTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");

      // Create dictionary object to hold received user messages.
      var changes = new Dictionary<string, object>();
      {
         changes.Add($"userMessage_{currentTimeStamp}.json", userMessage);
         changes.Add($"assistantMessage_{currentTimeStamp}.json", assistantMessage);
      }
      try
      {
         // Save the user message to your Storage.
         await _myStorage.WriteAsync(changes, cancellationToken);
      }
      catch
      {
         // Inform the user an error occurred.
         await turnContext.SendActivityAsync("Sorry, something went wrong storing your message!");
      }
   }
}