using Domain.Models;
using Domain.Persistence;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Persistence
{
    public class EmailStorage : IEmailStorage
    {
        private Dictionary<string, Dictionary<string, Email>> emails;
        private const string FILE_NAME = "emails.json";

        public EmailStorage()
        {
            emails = new Dictionary<string, Dictionary<string, Email>>();
        }

        public Dictionary<string, Email> GetAllEmails(string user)
        {
            try
            {
                ReadFile();

                if (!emails.ContainsKey(user))
                {
                    return new Dictionary<string, Email>();
                }
                else
                {
                    return emails[user];
                }
            }
            finally
            {
                SaveFile();
            }
        }

        public Email GetEmail(string user, string id)
        {
            try
            {
                ReadFile();

                if (!emails.ContainsKey(user) || !emails[user].ContainsKey(id))
                {
                    return null;
                }
                else
                {
                    return emails[user][id];
                }
            }
            finally
            {
                SaveFile();
            }
        }

        public void SaveEmail(string user, string id, Email email)
        {
            try
            {
                ReadFile();

                if (!emails.ContainsKey(user))
                {
                    emails.Add(user, new Dictionary<string, Email>());
                }

                if (!emails[user].ContainsKey(id))
                {
                    emails[user].Add(id, email);
                }
                else
                {
                    emails[user][id] = email;
                }
            }
            finally
            {
                SaveFile();
            }
        }

        private void CreateFile()
        {
            if (!File.Exists(FILE_NAME))
            {
                File.Create(FILE_NAME).Close();
            }
        }

        private void ReadFile()
        {
            CreateFile();

            emails = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Email>>>(File.ReadAllText(FILE_NAME));

            if (emails is null)
            {
                emails = new Dictionary<string, Dictionary<string, Email>>();
            }
        }

        private void SaveFile()
        {
            CreateFile();

            File.WriteAllText(FILE_NAME, JsonConvert.SerializeObject(emails));
        }

        public void DeleteAllEmails(string user)
        {
            try
            {
                ReadFile();

                if (emails.ContainsKey(user))
                {
                    emails.Remove(user);
                }
            }
            finally
            {
                SaveFile();
            }
        }

        public void DeleteEmail(string user, string id)
        {
            try
            {
                ReadFile();

                if (emails.ContainsKey(user) && emails[user].ContainsKey(id))
                {
                    emails[user].Remove(id);
                }
            }
            finally
            {
                SaveFile();
            }
        }
    }
}
