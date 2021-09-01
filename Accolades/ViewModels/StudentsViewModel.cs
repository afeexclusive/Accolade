using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.ViewModels
{
    public class StudentsViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Admission Number")]
        public string AdmissionNumber { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Middle Name")]
        public string MiddeleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public string ParentName { get; set; }
        public string FatherPhone { get; set; }
        public string MotherPhone { get; set; }
        public string MotherEmail { get; set; }
        public string FatherEmail { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string FileName { get; set; }
        public List<string> Headings { get; set; }
    }
}
