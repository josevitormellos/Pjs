namespace _5PJS.Request
{
    public class DiagnosisRequest
    {
        public string CrmDoctor { get; set; }
        public string CpfPatient { get; set; }
        public DateTime Date { get; set; }
        public bool Result { get; set; }
        public int IdMethodDiagnosis { get; set; }

        public string Observation { get; set; }
    }
}
