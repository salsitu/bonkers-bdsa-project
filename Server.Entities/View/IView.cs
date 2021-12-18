using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Server.Entities
{
    internal interface IView
    {
        public int ProjectId { get; set; }
        public int StudentId { get; set; }
    }
}
