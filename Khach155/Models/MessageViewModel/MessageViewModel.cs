using System;
using System.ComponentModel.DataAnnotations;

namespace Khach155.Models.MessageViewModel
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public string FromUserName { get; set; }
        [Required]
        public string Room { get; set; }
        public string Avatar { get; set; }


        public static implicit operator MessageViewModel(Message ms)
        {
            return new MessageViewModel
            {
                Id = ms.Id,
                Content = ms.Content,
                Timestamp = ms.Timestamp,
            };
        }

        public static implicit operator Message(MessageViewModel vm)
        {
            return new Message
            {
                Id = vm.Id,
                Content = vm.Content,
                Timestamp = vm.Timestamp,
            };
        }
    }
}
