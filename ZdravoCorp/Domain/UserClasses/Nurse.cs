namespace ZdravoCorp.Domain.UserClasses
{
    public class Nurse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }  
        
        public string Role { get; }


        public Nurse(int id, string firstName, string lastName, string username, string password, string role = "nurse")
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Password = password;
            Role = role;
        }
    }

}
