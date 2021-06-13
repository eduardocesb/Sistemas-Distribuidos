using Newtonsoft.Json;

namespace Domain.Models
{
    public class Email
    {
        [JsonProperty(PropertyName = "Sender", NullValueHandling = NullValueHandling.Ignore)]
        public User Sender { get; set; }

        [JsonProperty(PropertyName = "Recipient", NullValueHandling = NullValueHandling.Ignore)]
        public User Recipient { get; set; }

        [JsonProperty(PropertyName = "Subject Matter", NullValueHandling = NullValueHandling.Ignore)]
        public string SubjectMatter { get; set; }

        [JsonProperty(PropertyName = "Body", NullValueHandling = NullValueHandling.Ignore)]
        public object Body { get; set; }
    }
}
