using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;

namespace MilkStore.WebApp.Pages
{
    public class DisplayImagesModel : PageModel
    {
        private readonly AppDbContext _context;

        public DisplayImagesModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Image> Images { get; set; }

        public void OnGet()
        {
            Images = _context.Images.ToList();
        }
    }
}
