using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ExcelHelper
{
    public class UploadStudentDTO
    {
        public Guid Id { get; set; }
        public string AdmissionNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddeleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Class { get; set; }
        public string ParentName { get; set; }
        public string FatherPhone { get; set; }
        public string MotherPhone { get; set; }
        public string MotherEmail { get; set; }
        public string FatherEmail { get; set; }

    }

    public enum CollegeClass
    {
        Year_7A,
        Year_7B,
        Year_7C,
        Year_7D
    }
}
