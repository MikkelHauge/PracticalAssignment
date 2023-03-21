namespace DemoADO.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public User(string name, string email, string address)
        {
            Name = name;
            Email = email;
            Address = address;
        }

        public override string ToString()
        {
            return "Name: " + Name + "\nEmail:" + Email + "\nAddress: " + Address;
        }
    }
}
