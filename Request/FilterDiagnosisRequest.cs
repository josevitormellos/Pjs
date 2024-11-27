namespace _5PJS.Request
{
    public class FilterDiagnosisRequest
    {
        
        //Patient
        public bool PacientFilter { get; set; }
        public string NamePatient { get; set; }
        public string CpfPatient { get; set; }
        public DateTime? DateBirthPatient { get; set; }

        //Doctor
        public bool DocterFilter { get; set; }
        public string CrmDoctor { get; set; }
        public string NameDoctor { get; set; }

        //FeedBack
        public bool FeedBackFilter { get; set; }
        public bool FeedBackView { get; set; }
        public string SortFeedBack { get; set; }

        //Data normal, um calendario serve
        public DateTime? ShippingDateFeedBack { get; set; }

        //Data normal, um calendario serve
        public int TestDateFeedBack { get; set; }

        //MethodDiagnosis
        public bool MethodDiagnosisFilter { get; set; }
        public string NameMethodDiagnosis { get; set; }

        //Aqui não é uma data comum , é um tempo, que demorou pra fazer efeito, pode ser, dias, meses ou anos
        public int ResponseTime { get; set; }

        //Address
        public bool AddressFilter { get; set; }
        public string PublicPlace { get; set; }
        public int Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Cep { get; set; }
    }
}
