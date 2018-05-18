using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WindowsService1.MySQL.DAL
{
    public class Ana_DAL
    {
        public IEnumerable<string> GetPumproom()
        {
            List<string> list = new List<string>();
            DataTable dt = MySQLHelper.ExecuteDataTable("select 编号 from stations where IsDeleted='0';");
            foreach(DataRow dr in dt.Rows)
            {
                string s = "pumproom" + dr["编号"].ToString();
                list.Add(s);
            }
            return list;
        }
       
    }
}
