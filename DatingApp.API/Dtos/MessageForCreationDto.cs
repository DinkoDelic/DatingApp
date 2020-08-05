using System;

namespace DatingApp.API.Dtos
{
    public class MessageForCreationDto
    {
        // Constructror to asign current time when the DTO is created
        public MessageForCreationDto()
        {
            this.MessageSent = DateTime.Now;
        }
        public int SenderId { get; set; }

        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }

        public string Content { get; set; }
    }
}