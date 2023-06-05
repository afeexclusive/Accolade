using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Accolades.ViewModels
{
    public class ConnectionModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Domain { get; set; } = "";
        public int Port { get; set; } = 0;
    }
}
