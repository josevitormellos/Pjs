namespace _5PJS.Entities
{
    public class Report
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string CrmDoctor { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public List<ReportDiagnosis> DiagnosisList { get; set; }
    }
}
