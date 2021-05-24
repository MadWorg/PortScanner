using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortScanner
{
    interface IOutputAccess
    {
        string OutputText { get; set; }
    }
}
