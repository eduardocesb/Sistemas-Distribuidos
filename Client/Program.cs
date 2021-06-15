using Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        private static bool logged = false;
        private static string user;
        private static string id;
        private static string host = "localhost:5000";
        private static Email email;
        private static HttpClient httpClient = new HttpClient();

        public static async Task Main(string[] args)
        {
            while (true)
            {
                if (logged)
                {
                    await LoggedMenu();
                }
                else if (await MainMenu() == false)
                {
                    break;
                }
            }

            Console.WriteLine("Thanks to use our system!");
        }

        private static async Task<bool> MainMenu()
        {
            Console.WriteLine("Welcome!\n");
            Console.WriteLine("0 -- Exit");
            Console.WriteLine("1 -- Login\n");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine().Trim();

            if (option == "1")
            {
                Console.Write("Write your email: ");
                user = Console.ReadLine();

                return logged = true;
            }
            else if (option == "0")
            {
                return false;
            }
            else
            {
                return await MainMenu();
            }
        }

        private static async Task LoggedMenu()
        {
            Console.WriteLine("0 - Logout");
            Console.WriteLine("1 - Send message");
            Console.WriteLine("2 - Show messages");
            Console.WriteLine("3 - Open message");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine().Trim();

            if (option == "0")
            {
                logged = false;
            }
            else if (option == "1")
            {
                await SendMessage();
            }
            else if (option == "2")
            {
                await ShowMessages();
            }
            else if (option == "3")
            {
                await OpenMessage();
            }
        }

        private static async Task OpenMessage()
        {
            Console.WriteLine("-------- Open Message --------\n");
            Console.Write("Write the message id: ");

            id = Console.ReadLine().Trim();

            if (id.Length == 0)
            {
                await OpenMessage();
            }
            else
            {
                var response = await httpClient.GetAsync($"http://{host}/{user}/{id}/");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Ops, we don't found this message!");
                }
                else if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Ops, we're bad!");
                }
                else
                {
                    email = JsonConvert.DeserializeObject<Email>(await response.Content.ReadAsStringAsync());

                    Console.WriteLine("Sender: " + email.Sender.Name);
                    Console.WriteLine("Subject Matter: " + email.SubjectMatter);
                    Console.WriteLine("Message: " + email.Body.Message);

                    await OpenMessageMenu();
                }
            }
        }

        private static async Task OpenMessageMenu()
        {
            Console.WriteLine("\n\n0 - Back");
            Console.WriteLine("1 - Delete message");
            Console.WriteLine("2 - Forward message");
            Console.WriteLine("3 - Reply message");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine().Trim();

            if (option == "1")
            {
                await DeleteMessage();
            }
            else if (option == "2")
            {
                await ForwardMessage();
            }
            else if (option == "3")
            {
                await ReplyMessage();
            }
            else if (option != "0")
            {
                await OpenMessageMenu();
            }
        }

        private static async Task ReplyMessage()
        {
            Console.WriteLine("----- Reply Message -----\n");
            Console.WriteLine("Write the answer: ");

            string message = Console.ReadLine();

            email.Body.Message = message + "\n\n-----------------------\n\n" + email.Body.Message;
            email.SubjectMatter = "Re: " + email.SubjectMatter;

            email.Recipient = email.Sender;

            await SendEmail();
        }

        private static async Task SendEmail()
        {
            using (StringContent content = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json"))
            {
                var response = await httpClient.PostAsync($"http://{host}/{user}/", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Email sent successfully!");
                }
                else
                {
                    Console.WriteLine("Ops, we're bad!");
                }

                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }

        private static async Task ForwardMessage()
        {
            Console.WriteLine("----- Forward Message -----\n");
            Console.WriteLine("Write the destinatary: ");

            string destinatary = Console.ReadLine().Trim();

            email.SubjectMatter = "Fwd: " + email.SubjectMatter;

            email.Recipient = new User()
            {
                Name = destinatary
            };

            await SendEmail();
        }

        private static async Task DeleteMessage()
        {
            var response = await httpClient.DeleteAsync($"http://localhost:5000/{user}/{id}/");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Message delete successfully!");
            }
            else
            {
                Console.WriteLine("Ops, we're bad!");
            }
        }

        private static async Task ShowMessages()
        {
            Console.WriteLine("----- Show Messages -----\n\n");

            var response = await httpClient.GetAsync($"http://{host}/{user}/");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Ops, we're bad!");
            }
            else
            {
                Dictionary<string, Email> emails = JsonConvert.DeserializeObject<Dictionary<string, Email>>(await response.Content.ReadAsStringAsync());

                foreach (var email in emails)
                {
                    Console.WriteLine("Id: " + email.Key);
                    Console.WriteLine("Sender: " + email.Value.Sender.Name);
                    Console.WriteLine("Subject Matter: " + email.Value.SubjectMatter);
                    Console.WriteLine("Message: " + email.Value.Body.Message);
                    Console.WriteLine("\n\n^v^v^v^v^v^v^v^v^v^v^v^v^v\n");
                }

                if (emails.Count == 0)
                {
                    Console.WriteLine("No messages found!");
                }
                else
                {
                    await OpenMessagesMenu();
                }
            }
        }

        private static async Task OpenMessagesMenu()
        {
            Console.WriteLine("0 - Back");
            Console.WriteLine("1 - Delete all messages");
            Console.Write("Choose an option: ");

            string option = Console.ReadLine().Trim();

            if (option == "1")
            {
                await DeleteMessages();
            }
            else if (option != "0")
            {
                await OpenMessagesMenu();
            }
        }

        private static async Task DeleteMessages()
        {
            var response = await httpClient.DeleteAsync($"http://{host}/{user}/");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Ops, we're bad!");
            }
            else
            {
                Console.WriteLine("Messages deleted successfully!");
            }
        }

        private static async Task SendMessage()
        {
            email = new Email();

            Console.WriteLine("----- Send Message -----\n");

            Console.WriteLine("Write the destinatary: ");
            string destinatary = Console.ReadLine().Trim();

            Console.WriteLine("Write the subject matter: ");
            email.SubjectMatter = Console.ReadLine().Trim();

            Console.WriteLine("Write the message: ");
            string message = Console.ReadLine().Trim();

            email.Recipient = new User()
            {
                Name = destinatary
            };

            email.Body = new Body()
            {
                Message = message
            };

            await SendEmail();
        }
    }
}
