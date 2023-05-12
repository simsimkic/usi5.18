namespace ZdravoCorp.Domain.UserClasses
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialty { get; set; }
        public string Role { get; }

        public Doctor(int id, string username, string password, string firstName, string lastName, string specialty, string role="doctor")
        {
            Id = id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Specialty = specialty;
            Role = role;
        }
    }
}
