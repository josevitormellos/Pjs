namespace _5PJS.Request
{
    public class FeedbackRequest
    {
        public string CrmDoctor { get; set; }
        public int IdDiagnosis { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Sort { get; set; }
        public int TestDate { get; set; }
    }
}
