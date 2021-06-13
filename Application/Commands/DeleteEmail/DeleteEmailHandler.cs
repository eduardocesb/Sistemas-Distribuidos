using Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.DeleteEmail
{
    public class DeleteEmailHandler : IDeleteEmailHandler
    {
        private readonly IEmailStorage storage;

        public DeleteEmailHandler(IEmailStorage storage)
        {
            this.storage = storage;
        }

        public void Handle(DeleteEmailCommand command)
        {
            storage.DeleteEmail(command.User, command.Id);
        }
    }
}
