using Newtonsoft.Json;

namespace Proftaak_S3_API.Models
{
    public class UserJWT
    {
        [JsonProperty("encryptedJWT")]
        public string encryptedJWT { get; set; }
    }
}
