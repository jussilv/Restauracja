using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restauracja.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<MenuItem> MenuItems { get; set; }

        public async Task OnGetAsync()
        {
            MenuItems = await _context.MenuItems.ToListAsync();
        }
    }
}