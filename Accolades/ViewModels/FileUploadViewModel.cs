using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.ViewModels
{
    public class FileUploadViewModel
    {
        public IFormFile File { get; set; }
    }
}
