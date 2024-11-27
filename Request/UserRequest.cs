namespace _5PJS.Request
{
    public class UserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string PasswordConfirmed { get; set; }
        public string Phone { get; set; }
        public DateTime DateBirth { get; set; }

        //Doctor
        public string Crm { get; set; }
        public string Specialty { get; set; }
    }
}
