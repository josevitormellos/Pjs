namespace _5PJS.Request
{
    public class UserUpdateRequest
    {
        public string EmailVerific { get; set; }
        public string PasswordVerific { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
        public string Phone { get; set; }
        public DateTime DateBirth { get; set; }

        //Doctor
        public int IdSpecialty { get; set; }
    }
}
