using Application.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Interfaces
{
    public interface ICertificateService
    {
        bool ManualGenerateAccoladeCertificate(CertificateVM model);
    }
}
