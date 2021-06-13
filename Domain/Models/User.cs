using Newtonsoft.Json;

namespace Domain.Models
{
    public class User
    {
        [JsonProperty(PropertyName = "Name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}
