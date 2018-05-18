using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService1.MySQL.DAL;

namespace WindowsService1.MySQL.BLL
{
    public class Ana_BLL
    {
        public IEnumerable<string> GetPumproom()
        {
            return new Ana_DAL().GetPumproom();
        }
    }
}
