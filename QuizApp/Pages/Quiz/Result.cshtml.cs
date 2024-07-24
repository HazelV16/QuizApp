using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuizApp.Pages.Quiz;

public class ResultModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int Score { get; set; }
    [BindProperty(SupportsGet = true)]
    public int Total { get; set; }
    public void OnGet(int score, int total)
    {
        Score = score;
        Total = total;
    }
}