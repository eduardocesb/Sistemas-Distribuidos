using Domain.Models;
using Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries.GetAllEmails
{
    public class GetAllEmailsHandler : IGetAllEmailsHandler
    {
        private readonly IEmailStorage storage;

        public GetAllEmailsHandler(IEmailStorage storage)
        {
            this.storage = storage;
        }

        public GetAllEmailsResult Handle(GetAllEmailsQuery query)
        {
            return new GetAllEmailsResult(storage.GetAllEmails(query.User));
        }
    }
}
