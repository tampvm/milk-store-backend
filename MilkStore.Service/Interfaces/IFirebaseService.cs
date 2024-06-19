using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> UploadAvatarImageAsync(Stream stream, string fileName);
        Task<string> UploadProductImageAsync(Stream stream, string fileName);
        Task<string> UploadWebImageAsync(Stream stream, string fileName);
    }
}
