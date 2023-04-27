using Microsoft.AspNetCore.Identity;
using System.Drawing;
using System.Collections;
using Khach155.Models;
namespace Khach155.Services
{
    public interface ICommon
    {
        Task<string> UploadedFile(IFormFile ProfilePicture);
        string GetMD5(string str);

        void SendEmail(DataUser request);
        string GenerateToken();
    }
}
