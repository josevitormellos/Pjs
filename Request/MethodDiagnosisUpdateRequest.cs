namespace _5PJS.Request
{
    public class MethodDiagnosisUpdateRequest
    {
        public int Id { get; set; }
        public string CrmDoctor { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public char Effectiveness { get; set; }
        public int ResponseTime { get; set; }

        public string Recommendations { get; set; }
    }
}
