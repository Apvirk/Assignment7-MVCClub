﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCClub.Models
{
    public class StudentDetails
    {

        public int StudentID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string  RegistrationDate { get; set; }
    }
}