using Domain.Models;

namespace Application.Queries.GetEmail
{
    public class GetEmailResult
    {
        public readonly Email Email;

        public GetEmailResult()
        {
            this.Email = null;
        }

        public GetEmailResult(Email email)
        {
            this.Email = email;
        }
    }
}