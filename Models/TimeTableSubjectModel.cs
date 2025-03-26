using System.ComponentModel.DataAnnotations;

namespace TimeTableGenerator.Models
{
    public class TimeTableSubjectModel
    {
        [Range(1, 7, ErrorMessage = "Working days must be between 1 and 7")]
        public int WorkingDays { get; set; }

        [Range(1, 8, ErrorMessage = "Subjects per day must be less than 9")]
        public int SubjectsPerDay { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total subjects must be positive")]
        public int TotalSubjects { get; set; }

        public int TotalHours => WorkingDays * SubjectsPerDay;
    }

    public class SubjectHour
    {
        public string SubjectName { get; set; }

        public int Hours { get; set; }
    }
}
