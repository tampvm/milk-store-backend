using Firebase.Storage;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.WebApp.Models;

namespace MilkStore.WebApp.Pages
{
    public class UploadModel : PageModel
    {
        private readonly FirebaseSettings _firebaseSettings;
        private readonly FirebaseStorage _firebaseStorage;
        private readonly AppDbContext _context; // Thêm AppDbContext để tương tác với DB

        public UploadModel(IOptions<FirebaseSettings> firebaseSettings, AppDbContext context)
        {
            _firebaseSettings = firebaseSettings.Value;
            var credential = GoogleCredential.FromFile(_firebaseSettings.CredentialsPath);
            var storageClient = StorageClient.Create(credential);

            _firebaseStorage = new FirebaseStorage(
                _firebaseSettings.StorageBucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(string.Empty),
                    ThrowOnCancel = true
                });

            _context = context;
        }

        [BindProperty]
        public IFormFile UploadFile { get; set; }

        public string UploadResult { get; private set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadFile == null || UploadFile.Length == 0)
            {
                UploadResult = "Please select a file to upload.";
                return Page();
            }

            var fileName = $"{Guid.NewGuid()}_{UploadFile.FileName}";

            using (var stream = new MemoryStream())
            {
                await UploadFile.CopyToAsync(stream);
                stream.Seek(0, SeekOrigin.Begin);

                try
                {
                    var task = _firebaseStorage
                        .Child("images")
                        .Child(fileName)
                        .PutAsync(stream);

                    var downloadUrl = await task;

                    // Lưu thông tin ảnh vào cơ sở dữ liệu
                    var image = new Image
                    {
                        ImageUrl = downloadUrl,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = "admin",
                        ThumbnailUrl = downloadUrl
                    };

                    _context.Images.Add(image);
                    await _context.SaveChangesAsync();

                    UploadResult = $"File uploaded successfully: {downloadUrl}";
                }
                catch (Exception ex)
                {
                    UploadResult = $"Error: {ex.Message}";
                }
            }

            return Page();
        }
    }
}
