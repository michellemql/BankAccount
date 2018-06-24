using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace BankAccount.Models
{
    public abstract class BaseEntity{}

    public class User : BaseEntity
    {
        [Key]
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; } 
        public double balance {get;set;}
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public List<Transactions> transactions {get;set;}
        public User()
        {
            transactions = new List<Transactions>();
        }
    }    
}