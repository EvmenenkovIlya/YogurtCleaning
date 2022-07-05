﻿namespace YogurtCleaning.DataLayer.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal Rating { get; set; }
        public List<CleaningObject> Addresses { get; set; }
    }
}
