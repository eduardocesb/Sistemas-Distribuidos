using Domain.Models;
using Domain.Persistence;

namespace Application.Queries.GetEmail
{
    public class GetEmailHandler : IGetEmailHandler
    {
        private readonly IEmailStorage storage;

        public GetEmailHandler(IEmailStorage storage)
        {
            this.storage = storage;
        }

        public GetEmailResult Handle(GetEmailQuery query)
        {
            return new GetEmailResult(storage.GetEmail(query.User, query.Id));
        }
    }
}
