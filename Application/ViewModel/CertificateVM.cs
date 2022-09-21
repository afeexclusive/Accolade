using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ViewModel
{
    public class CertificateVM
    {
        public string StudentName { get; set; }

        public string AwardName { get; set; }

        public string AwardMonth { get; set; }

        public string AwardSubject { get; set; }

        public DateTimeOffset IssueDate { get; set; }

    }
    
}
