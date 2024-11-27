namespace _5PJS.Request
{
    public class AddressUpdateRequest
    {
        public string CrmDoctor { get; set; }
        public string CpfPatient { get; set; }
        public int Id { get; set; }
        public string PublicPlace { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
    }
}
