using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.Models
{
    public class CertificateViewModel
    {
        [Display(Name = "Student Name")]
        public string StudentName { get; set; }

        [Display(Name = "Award Name")]
        public string AwardName { get; set; }

        [Display(Name = "Award Month")]
        public string  AwardMonth { get; set; }

        [Display(Name = "Subject")]
        public string  AwardSubject { get; set; }

        [Display(Name = "Issue Date")]
        public DateTimeOffset IssueDate { get; set; }
       
    }
}
