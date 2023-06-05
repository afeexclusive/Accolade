using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.ViewModels
{
    public class ConnectionDetailsViewModel
    {
        public bool IsAuthenticated { get; set; }
        public string Domain { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
