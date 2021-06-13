namespace Application.Queries.GetAllEmails
{
    public interface IGetAllEmailsHandler
    {
        public GetAllEmailsResult Handle(GetAllEmailsQuery query);
    }
}
