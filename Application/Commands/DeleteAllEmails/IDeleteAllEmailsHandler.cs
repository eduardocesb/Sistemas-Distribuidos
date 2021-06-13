using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands.DeleteAllEmails
{
    public interface IDeleteAllEmailsHandler
    {
        public void Handle(DeleteAllEmailsCommand command);
    }
}
