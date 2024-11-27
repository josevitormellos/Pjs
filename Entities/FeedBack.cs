namespace _5PJS.Entities
{
    public class FeedBack
    {
        public int Id { get; set; }
        public string CrmDoctor { get; set; }
        public int IdDiagnosis { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Sort { get; set; }
        public DateTime ShippingDate {  get; set; }
        public int TestDate { get; set; }
    }
}
