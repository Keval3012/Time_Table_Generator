using TimeTableGenerator.Models;

namespace TimeTableGenerator.ViewModel
{
    public class TimeTableViewModel
    {
        public TimeTableSubjectModel InitialInput { get; set; }
        public List<SubjectHour> SubjectHours { get; set; }
        public string[,]? GeneratedTable { get; set; }
    }
}
