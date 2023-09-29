﻿namespace YBS.Data.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }    
        public string Role { get; set; }
        public DateTime CreationDate { get; set; }
        public Company Company { get; set; }
        public Member Member { get; set; }
        public string Status { get; set; }
    }
}
