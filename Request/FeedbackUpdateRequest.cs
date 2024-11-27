namespace _5PJS.Request
{
    public class FeedbackUpdateRequest
    {
        public int Id { get; set; }
        public string CrmDoctor { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Sort { get; set; }
        public int TestDate { get; set; }
    }
}
