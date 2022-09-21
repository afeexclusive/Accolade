using AccoladesData.Entities;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ExcelHelper
{
    public class StudentsMap : ClassMap<Student>
    {
        private readonly Student _student;

        public StudentsMap(Student student)
        {
            _student = student;

            Map(st => st.AdmissionNumber).Name(_student.AdmissionNumber);
            Map(st => st.FirstName).Name(_student.FirstName);
            Map(st => st.MiddeleName).Name(_student.MiddeleName);
            Map(st => st.LastName).Name(_student.LastName);
            Map(st => st.Gender).Name(_student.Gender);
            Map(st => st.Class).Name(_student.Class);
            Map(st => st.ParentName).Name(_student.ParentName);
            Map(st => st.FatherPhone).Name(_student.FatherPhone);
            Map(st => st.MotherPhone).Name(_student.MotherPhone);
            Map(st => st.FatherEmail).Name(_student.FatherEmail);
            Map(st => st.MotherEmail).Name(_student.FatherEmail);
            
        }
    }
}
