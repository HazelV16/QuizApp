using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Pages.Quiz;

    public class IndexModel : PageModel
    {
        private readonly QuizContext _context;

        public IndexModel(QuizContext context)
        {
            _context = context;
        }

        public List<Question> Questions { get; set; }

        public async Task OnGetAsync()
        {
            Questions = await _context.Questions.Include(q => q.Answers).ToListAsync();
            // Shuffle questions
            Questions = Questions.OrderBy(q => Guid.NewGuid()).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Questions = await _context.Questions.Include(q => q.Answers).ToListAsync();

            var submittedAnswers = new Dictionary<int, int>();

            foreach (var question in Questions)
            {
                var answerIdStr = Request.Form[$"answers_{question.Id}"];
                if (!string.IsNullOrEmpty(answerIdStr) && int.TryParse(answerIdStr, out int answerId))
                {
                    submittedAnswers[question.Id] = answerId;
                }
            }

            // Calculate the score
            int score = 0;

            foreach (var question in Questions)
            {
                var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                if (correctAnswer != null && submittedAnswers.ContainsKey(question.Id) && submittedAnswers[question.Id] == correctAnswer.Id)
                {
                    score++;
                }
            }

            return RedirectToPage("Result", new { score = score, total = Questions.Count });
        }
    }
