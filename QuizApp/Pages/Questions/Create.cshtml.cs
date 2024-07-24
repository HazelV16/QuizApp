using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Entities;

namespace QuizApp.Pages.Questions;

public class CreateModel : PageModel
{
    private readonly QuizContext _context;

    public CreateModel(QuizContext context)
    {
        _context = context;
        // Initialize Answers list with four empty Answer objects
    }
    
    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Question Question { get; set; }
    [BindProperty]
    // public List<Answer> Answers { get; set; }
    public List<Answer> Answers { get; set; } = new List<Answer>
    {
        new Answer { IsCorrect = false },
        new Answer { IsCorrect = false },
        new Answer { IsCorrect = false },
        new Answer { IsCorrect = false }
    };

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Questions.Add(Question);
        await _context.SaveChangesAsync();

        foreach (var answer in Answers)
        {
            answer.QuestionId = Question.Id;
            _context.Answers.Add(answer);
        }

        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}