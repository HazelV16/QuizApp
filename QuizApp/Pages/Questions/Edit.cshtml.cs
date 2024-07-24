using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Entities;

namespace QuizApp.Pages.Questions;

public class EditModel : PageModel
{
    private readonly QuizContext _context;
    private readonly ILogger<EditModel> _logger;

    public EditModel(QuizContext context, ILogger<EditModel> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    [BindProperty]
    public Question Question { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            _logger.LogError("No ID provided.");
            return NotFound();
        }
        
        // _logger.LogInformation($"Form Data: Question ID = {Question.Id}, Text = {Question.Text}");

        Question = await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (Question == null)
        {
            _logger.LogError($"Question with ID {id} not found.");
            return NotFound();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is invalid.");
            return Page();
        }

        var questionToUpdate = await _context.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == Question.Id);

        if (questionToUpdate == null)
        {
            _logger.LogError($"Question with ID {Question.Id} not found on post.");
            return NotFound();
        }

        questionToUpdate.Text = Question.Text;
        questionToUpdate.Answers = Question.Answers;

        _context.Attach(questionToUpdate).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!QuestionExists(Question.Id))
            {
                _logger.LogError($"Concurrency issue: Question with ID {Question.Id} no longer exists.");
                return NotFound();
            }
            else
            {
                _logger.LogError($"Concurrency exception: {ex.Message}");
                throw;
            }
        }
        _logger.LogInformation("Question updated successfully, redirecting to index.");
        return RedirectToPage("./Index");
    }

    private bool QuestionExists(int id)
    {
        return _context.Questions.Any(e => e.Id == id);
    }
}