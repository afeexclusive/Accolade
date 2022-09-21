using AccoladesData.Entities;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ExcelHelper
{
    public class StudentEmailMap : ClassMap<Student>
    {
        private readonly Student _student;

        public StudentEmailMap(Student student)
        {
            _student = student;

            Map(st => st.Email).Name(_student.Email);
            Map(st => st.DateOfBirth).Name(_student.DateOfBirth);
            Map(st => st.FirstName).Name(_student.FirstName);
            Map(st => st.LastName).Name(_student.LastName);

        }
    }
}
