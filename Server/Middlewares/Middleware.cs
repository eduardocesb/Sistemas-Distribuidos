using Application.Commands.DeleteAllEmails;
using Application.Commands.DeleteEmail;
using Application.Commands.SaveEmail;
using Application.Queries.GetAllEmails;
using Application.Queries.GetEmail;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Server.Middlewares
{
    public class Middleware
    {
        private readonly RequestDelegate next;
        private readonly IGetEmailHandler getEmailHandler;
        private readonly IGetAllEmailsHandler getAllEmailsHandler;
        private readonly ISaveEmailHandler saveEmailHandler;
        private readonly IDeleteAllEmailsHandler deleteAllEmailsHandler;
        private readonly IDeleteEmailHandler deleteEmailHandler;

        public Middleware(RequestDelegate next, IGetEmailHandler getEmailHandler, IGetAllEmailsHandler getAllEmailsHandler, ISaveEmailHandler saveEmailHandler, IDeleteAllEmailsHandler deleteAllEmailsHandler, IDeleteEmailHandler deleteEmailHandler)
        {
            this.next = next;

            this.getEmailHandler = getEmailHandler;
            this.getAllEmailsHandler = getAllEmailsHandler;
            this.saveEmailHandler = saveEmailHandler;
            this.deleteAllEmailsHandler = deleteAllEmailsHandler;
            this.deleteEmailHandler = deleteEmailHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsHealthcheck(context))
            {
                await next(context);
                return;
            }

            if (HttpMethods.IsGet(context.Request.Method))
            {
                await DoGet(context);
            }
            else if (HttpMethods.IsPost(context.Request.Method))
            {
                await DoPost(context);
            }
            else if (HttpMethods.IsDelete(context.Request.Method))
            {
                await DoDelete(context);
            }
        }

        private async Task DoGet(HttpContext context)
        {
            string path = context.Request.Path.Value;

            List<string> pathSplited = new List<string>(path.Split("/"));

            pathSplited = pathSplited.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            object body = null;
            HttpStatusCode statusCode = HttpStatusCode.NoContent;

            if (pathSplited.Count() == 0)
            {
                body = new { Message = "Hello World!" };
                statusCode = HttpStatusCode.OK;
            }
            else if (pathSplited.Count() == 1)
            {
                GetAllEmailsQuery query = new GetAllEmailsQuery()
                {
                    User = pathSplited[0].Trim().ToLower()
                };

                body = getAllEmailsHandler.Handle(query).Emails;

                statusCode = HttpStatusCode.OK;
            }
            else if (pathSplited.Count() == 2)
            {
                GetEmailQuery query = new GetEmailQuery()
                {
                    User = pathSplited[0].Trim().ToLower(),
                    Id = pathSplited[1].Trim().ToLower()
                };

                body = getEmailHandler.Handle(query).Email;

                if (body == null)
                {
                    statusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    statusCode = HttpStatusCode.OK;
                }
            }
            else
            {
                statusCode = HttpStatusCode.NotFound;
            }

            await PrepareResponse(context, statusCode, body);
        }

        private async Task DoPost(HttpContext context)
        {
            string path = context.Request.Path.Value;

            List<string> pathSplited = new List<string>(path.Split("/"));

            pathSplited = pathSplited.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            object body = null;
            HttpStatusCode statusCode = HttpStatusCode.NoContent;

            if (pathSplited.Count() == 1 || pathSplited.Count() == 2)
            {
                Email email = JsonConvert.DeserializeObject<Email>(await GetBody(context));

                if (!PrepareAndValidateEmail(email, pathSplited[0]))
                {
                    statusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    SaveEmailCommand command = new SaveEmailCommand()
                    {
                        Email = email,
                        Recipient = email.Recipient.Name,
                        Id = (pathSplited.Count() == 2 ? pathSplited[1] : Guid.NewGuid().ToString())
                    };

                    saveEmailHandler.Handle(command);

                    body = new { Message = "Email successfully sent!", Id = command.Id, Email = command.Email };
                    statusCode = HttpStatusCode.OK;
                }
            }
            else
            {
                body = new { Message = "Help me to help you!" };
                statusCode = HttpStatusCode.NotFound;
            }

            await PrepareResponse(context, statusCode, body);
        }

        private async Task DoDelete(HttpContext context)
        {
            string path = context.Request.Path.Value;

            List<string> pathSplited = new List<string>(path.Split("/"));

            pathSplited = pathSplited.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            object body = null;
            HttpStatusCode statusCode = HttpStatusCode.NoContent;

            if (pathSplited.Count() == 1)
            {
                DeleteAllEmailsCommand command = new DeleteAllEmailsCommand()
                {
                    User = pathSplited[0].Trim().ToLower()
                };

                deleteAllEmailsHandler.Handle(command);
            }
            else if (pathSplited.Count() == 2)
            {
                DeleteEmailCommand command = new DeleteEmailCommand()
                {
                    User = pathSplited[0].Trim().ToLower(),
                    Id = pathSplited[1].Trim().ToLower()
                };

                deleteEmailHandler.Handle(command);
            }
            else
            {
                body = new { Message = "Help me to help you!" };
                statusCode = HttpStatusCode.BadRequest;
            }

            await PrepareResponse(context, statusCode, body);
        }

        private bool PrepareAndValidateEmail(Email email, string user)
        {
            email.Recipient.Name = email.Recipient.Name.Trim().ToLower();

            if (email.Recipient.Name.Length == 0 || email.Body == null)
            {
                return false;
            }

            email.Sender = new User()
            {
                Name = user.Trim().ToLower()
            };

            return true;
        }

        private async Task<string> GetBody(HttpContext context)
        {
            StreamReader sr = new StreamReader(context.Request.Body);

            return await sr.ReadToEndAsync();
        }

        private async Task PrepareResponse(HttpContext context, HttpStatusCode statusCode, object body)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.Headers.Add("Content-type", "application/json");

            if (body != null)
            {
                await PrepareBody(context, body);
            }
        }

        private async Task PrepareBody(HttpContext context, object body)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body));

            MemoryStream stream = new MemoryStream(byteArray);

            await stream.CopyToAsync(context.Response.Body);
        }

        private bool IsHealthcheck(HttpContext context) =>
            (HttpMethods.IsGet(context.Request.Method) && context.Request.Path.StartsWithSegments("/meta/healthcheck"));
    }
}
