
using System;
using System.Collections.Generic;

namespace DemoADO.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public User(string name, string email, string address, Guid id)
        {
            Name = name;
            Email = email;
            Address = address;
            Id = id;
        }
        public override string ToString()
        {
            return "Name: " + Name + "\nEmail:" + Email + "\nAddress: " + Address + "\nGUID: " + Id;
        }
    }

}
