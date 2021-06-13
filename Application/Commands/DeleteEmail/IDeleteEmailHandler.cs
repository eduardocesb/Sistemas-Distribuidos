namespace Application.Commands.DeleteEmail
{
    public interface IDeleteEmailHandler
    {
        public void Handle(DeleteEmailCommand command);
    }
}
