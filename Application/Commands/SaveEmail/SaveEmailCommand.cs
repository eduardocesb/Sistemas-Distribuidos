using Domain.Models;

namespace Application.Commands.SaveEmail
{
    public class SaveEmailCommand
    {
        public string Recipient { get; set; }
        public string Id { get; set; }
        public Email Email { get; set; }
    }
}