namespace Application.Queries.GetEmail
{
    public interface IGetEmailHandler
    {
        public GetEmailResult Handle(GetEmailQuery query);
    }
}
