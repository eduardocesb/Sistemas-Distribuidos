using Domain.Models;
using System.Collections.Generic;

namespace Domain.Persistence
{
    public interface IEmailStorage
    {
        public Email GetEmail(string user, string id);

        public Dictionary<string, Email> GetAllEmails(string user);

        public void SaveEmail(string user, string id, Email email);

        public void DeleteAllEmails(string user);

        public void DeleteEmail(string user, string id);
    }
}
