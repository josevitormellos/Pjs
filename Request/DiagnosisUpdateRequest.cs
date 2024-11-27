namespace _5PJS.Request
{
    public class DiagnosisUpdateRequest
    {
        public int Id { get; set; }
        public string CrmDoctor { get; set; }
        public string CpfPatient { get; set; }
        public bool Result { get; set; }
        public int IdMethodDiagnosis { get; set; }

        public string Observation { get; set; }
    }
}
