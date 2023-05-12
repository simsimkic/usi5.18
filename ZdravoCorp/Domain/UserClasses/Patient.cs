using System;
using System.Collections.Generic;

namespace ZdravoCorp.Domain.UserClasses
{
    public class Patient
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Queue<DateTime> UpdateCancel { get; set; }
        public Queue<DateTime> Create { get; set; }
        public bool IsBlocked { get; set; }

        public string Role { get; }
        public Patient(int id, string username, string password, string firstName, string lastName, string role="patient")
        {
            Id = id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            UpdateCancel = new Queue<DateTime>();
            Create = new Queue<DateTime>();
            IsBlocked = false;
            Role = role;
        }
    }
}
