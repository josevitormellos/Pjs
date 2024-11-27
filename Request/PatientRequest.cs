namespace _5PJS.Request
{
    public class PatientRequest
    {
        public string CrmMedico { get; set; }
        public string Name { get; set; }
        public DateTime DateBirth { get; set; }
        public string Cpf { get; set; }
        public int IdAddress { get; set; }

        public string PublicPlace { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
    }
}
