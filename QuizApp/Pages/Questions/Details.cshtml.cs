using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Entities;

namespace QuizApp.Pages.Questions;

public class DetailsModel : PageModel
{
    private readonly QuizContext _context;
    [BindProperty] public Question Question { get; set; }

    public DetailsModel(QuizContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> OnGetAsync(int id)
    {
        Question = await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (Question == null)
        {
            return NotFound();
        }

        return Page();
    }
}