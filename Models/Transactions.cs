using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankAccount.Models
{
    public class Transactions : BaseEntity
    {
        [Key]
        public int transaction_id {get;set;}
        public float amount {get;set;} 
        public DateTime created_at {get;set;}
        public DateTime updated_at {get;set;}
        public int user_id {get;set;} 

        public User User{get;set;}
    }
}

