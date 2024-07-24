using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Entities;

namespace QuizApp.Pages.Questions;

public class IndexModel : PageModel
{
    private readonly QuizContext _context;

    public IndexModel(QuizContext context)
    {
        _context = context;
    }

    public IList<Question> Questions { get; set; }

    public async Task OnGetAsync()
    {
        Questions = await _context.Questions
            .Include(q => q.Answers)
            .ToListAsync();
    }
}