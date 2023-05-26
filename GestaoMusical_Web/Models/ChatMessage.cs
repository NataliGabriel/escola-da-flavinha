using System.Text.Json.Serialization;

namespace GestaoMusical_Web.Models
{
    public class ChatMessage
    {
        [JsonPropertyName ("choices")]
        public string choices { get; set; }
    }
}
