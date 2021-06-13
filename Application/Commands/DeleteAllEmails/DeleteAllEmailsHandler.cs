using Domain.Persistence;

namespace Application.Commands.DeleteAllEmails
{
    public class DeleteAllEmailsHandler : IDeleteAllEmailsHandler
    {
        private readonly IEmailStorage storage;

        public DeleteAllEmailsHandler(IEmailStorage storage)
        {
            this.storage = storage;
        }

        public void Handle(DeleteAllEmailsCommand command)
        {
            storage.DeleteAllEmails(command.User);
        }
    }
}
