using System;
using System.Collections.Generic;
using System.Text;

namespace AccoladesData.Entities
{
    public class CorpMember
    {
        public string StateCode { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string PhoneNumber { get; set; }
        public string LGA { get; set; }
        public string Month { get; set; }
    }

    public class CorpMemberDefualtData
    {
        public string StateCode { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string PhoneNumber { get; set; }
        public string LGA { get; set; }
        public string Months { get; set; }
    }
}
