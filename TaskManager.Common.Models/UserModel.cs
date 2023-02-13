﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Common.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public byte[] Photo { get; set; }
        public UserStatus Status { get; set; }

        public UserModel() { }

        public UserModel(
            string fname,
            string lname,
            string email,
            string password,
            UserStatus status /*= UserStatus.User*/,
            string phone/* = null*/
/*            byte[] photo = null*/)
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            Phone = phone;
            //Photo = photo;
            RegistrationDate = DateTime.Now;
            Status = status;
        }
    }
}
