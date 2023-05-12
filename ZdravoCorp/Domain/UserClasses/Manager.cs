namespace ZdravoCorp.Domain.UserClasses
{
    public class Manager
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; }

        public Manager(int id, string username, string password, string firstName, string lastName, string role="manager")
        {
            Id = id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
        }

        public override string ToString()
        {
            return "{Id: " + this.Id +  
                   ", Username: " + this.Username +    
                   ", Password: " + this.Password +    
                   ", FirstName: " + this.FirstName +    
                   ", LastName: " + this.LastName +    
                   ", Role: " + this.Role + "}";
        }
    }
}
