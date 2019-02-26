using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCClub.Models
{
    public class MemberDetails
    {

        private string _club;
        private string _OpenDate;
        private string _Location;
        private int _NoofMember;


        public int NoofMember
        {
            set { _NoofMember = value; }
            get { return _NoofMember; }
        }

        public string club
        {
            set { _club = value; }
            get { return _club; }
        }


        public string OpenDate
        {
            set { _OpenDate = value; }
            get { return _OpenDate; }
        }

        public string Location
        {
            set { _Location = value; }
            get { return _Location; }
        }

    }
}