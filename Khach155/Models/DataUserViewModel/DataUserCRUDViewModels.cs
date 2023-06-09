﻿
using Khach155.Models;
using System.ComponentModel.DataAnnotations;
namespace Khach155.Models.DataUserViewModel
{
    public class DataUserCRUDViewModels
    {
        public int? Id { get; set; }
        public decimal? TienDangCo { get; set; }
        [Required]		
        
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]

		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Mật khẩu không giống")]
		[DataType(DataType.Password)]
		public string? ConfirmPassword { get; set; }
		public decimal SoDiem { get; set; }


        public static implicit operator DataUserCRUDViewModels(DataUser _UserData)
        {
            return new DataUserCRUDViewModels
            {
                Id = _UserData.Id,
                TienDangCo = _UserData.TienDangCo,
                UserName = _UserData.UserName,
                Password = _UserData.Password,
                SoDiem = _UserData.SoDiem


			};
        }

        public static implicit operator DataUser(DataUserCRUDViewModels vm)
        {
            return new DataUser
            {
                Id = vm.Id,
                TienDangCo = vm.TienDangCo,
                UserName = vm.UserName,
                Password = vm.Password,
                SoDiem = vm.SoDiem

            };
        }
    }
}
