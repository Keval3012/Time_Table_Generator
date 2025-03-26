using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimeTableGenerator.Models;
using TimeTableGenerator.ViewModel;

namespace TimeTableGenerator.Controllers
{
    public class TimeTableController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new TimeTableSubjectModel());
        }

        [HttpPost]
        public IActionResult SubjectHours(TimeTableSubjectModel timeTableInput)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", timeTableInput);
            }

            var subjecthours = new TimeTableViewModel
            {
                InitialInput = timeTableInput,
                SubjectHours = Enumerable.Range(0, timeTableInput.TotalSubjects)
                    .Select(_ => new SubjectHour())
                    .ToList()
            };

            return View(subjecthours);
        }

        [HttpPost]
        public IActionResult Generate(TimeTableViewModel timeTableViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("SubjectHours", timeTableViewModel);
            }

            int totalHours = timeTableViewModel.InitialInput.TotalHours;
            int sumOfSubjectHours = timeTableViewModel.SubjectHours.Sum(x => x.Hours);
            if (sumOfSubjectHours != totalHours)
            {
                ModelState.AddModelError("", $"Total subject hours ({sumOfSubjectHours}) must equal total weekly hours ({totalHours})");
                return View("SubjectHours", timeTableViewModel);
            }

            timeTableViewModel.GeneratedTable = GenerateTimeTable(timeTableViewModel);
            return View("Result", timeTableViewModel);
        }

        private string[,] GenerateTimeTable(TimeTableViewModel timeTableViewModel)
        {
            var table = new string[timeTableViewModel.InitialInput.SubjectsPerDay, timeTableViewModel.InitialInput.WorkingDays];
            var subjectHours = new Dictionary<string, int>(timeTableViewModel.SubjectHours.ToDictionary(x => x.SubjectName, x => x.Hours));
            Random rand = new Random();

            for (int row = 0; row < timeTableViewModel.InitialInput.SubjectsPerDay; row++)
            {
                for (int col = 0; col < timeTableViewModel.InitialInput.WorkingDays; col++)
                {
                    var availableSubjects = subjectHours.Where(x => x.Value > 0).ToList();
                    if (availableSubjects.Any())
                    {
                        var subject = availableSubjects[rand.Next(availableSubjects.Count)];
                        table[row, col] = subject.Key;
                        subjectHours[subject.Key]--;
                    }
                }
            }
            return table;
        }
    }
}
