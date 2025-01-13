using Microsoft.AspNetCore.Http;

namespace UseCases.Appeals.Messages.Commands
{
    public class CreateAppealMessageCommand
    {
        public long AppealId { get; set; }
        public required string Message { get; set; }
        public required string UserToken { get; set; }
        public ICollection<IFormFile>? Files { get; set; }
    }
}
