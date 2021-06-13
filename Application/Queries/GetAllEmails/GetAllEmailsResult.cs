using Domain.Models;
using System.Collections.Generic;

namespace Application.Queries.GetAllEmails
{
    public class GetAllEmailsResult
    {
        public readonly Dictionary<string, Email> Emails;

        public GetAllEmailsResult()
        {

        }

        public GetAllEmailsResult(Dictionary<string, Email> emails)
        {
            this.Emails = emails;
        }
    }
}