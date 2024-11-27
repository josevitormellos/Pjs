namespace _5PJS.Request
{
    public class MethodDiagnosisRequest
    {
        public string CrmDoctor { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public char Effectiveness { get; set; }
        public int ResponseTime { get; set; }

        public string Recommendations { get; set; }
    }
}
