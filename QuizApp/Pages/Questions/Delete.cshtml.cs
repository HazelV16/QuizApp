using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Entities;

namespace QuizApp.Pages.Questions;

public class DeleteModel : PageModel
{
    [BindProperty] public Question Question { get; set; }
    private readonly QuizContext _context;

    public DeleteModel(QuizContext context)
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
    
    public async Task<IActionResult> OnPostAsync(int id)
    {
        Question = await _context.Questions.FindAsync(id);

        if (Question != null)
        {
            _context.Questions.Remove(Question);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}