using System.ComponentModel.DataAnnotations;
using Khach155.Models;

namespace Khach155.Models.RoomViewModel
{
    public class RoomViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [RegularExpression(@"^\w+( \w+)*$", ErrorMessage = "Characters allowed: letters, numbers, and one space between words.")]
        public string Name { get; set; }
        public int UserCreate { set; get; }

        public static implicit operator RoomViewModel(Room room)
        {
            return new RoomViewModel
            {
                Id = room.Id,
                Name = room.Name,
                UserCreate = room.UserCreate
            };
        }

        public static implicit operator Room(RoomViewModel vm)
        {
            return new Room
            {
                Id = vm.Id,
                Name = vm.Name,
                UserCreate = vm.UserCreate
            };
        }
    }
}
