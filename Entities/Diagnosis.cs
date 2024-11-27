namespace _5PJS.Entities
{
    public class Diagnosis
    {
        public int Id { get; set; }
        public string CrmDoctor { get; set; }
        public string CpfPatient { get; set; }
        public DateTime Date { get; set; }
        public bool Result { get; set; }
        public int IdMethodDiagnosis { get; set; }
        public string Observation {  get; set; }

        public ICollection<FeedBack> Feedbacks { get; set; }

        public List<ReportDiagnosis> ReportList { get; set; }

    }
}
