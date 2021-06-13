using Domain.Persistence;

namespace Application.Commands.SaveEmail
{
    public class SaveEmailHandler : ISaveEmailHandler
    {
        private readonly IEmailStorage storage;

        public SaveEmailHandler(IEmailStorage storage)
        {
            this.storage = storage;
        }

        public void Handle(SaveEmailCommand command)
        {
            storage.SaveEmail(command.Recipient, command.Id, command.Email);
        }
    }
}
