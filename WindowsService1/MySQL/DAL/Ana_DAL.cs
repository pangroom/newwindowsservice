using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace WindowsService1.MySQL.DAL
{
    public class Ana_DAL
    {
        //获取station中的pumproom没什么用
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
        //获取所有的数据库中所有的pumproom
        public IEnumerable<string> ReadPumpRoom()
        {
            DataTable dt = MySQLHelper.ExecuteDataTable("select table_name from information_schema.tables where table_schema='jucheapcore' and table_name like 'pumproom%';");
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string s = dr["table_name"].ToString();
                list.Add(s);
            }
            return list;
        }
        //查询所有表中的某一字段，看是否有报警信息
        public Dictionary<string, string> ReadAlarm()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            
            List<string> list = (List<string>)ReadPumpRoom();
            foreach(var tablename in list )
            {
                //每个pumproom的状态不一样，所以新建sb
                StringBuilder sb = new StringBuilder();
                int flag = 0;
                DataTable dt = MySQLHelper.ExecuteDataTable("select Id,时间,系统运行状态,PLC故障状态,压力报警状态,水箱缺水状态,1变频器状态,2变频器状态,3变频器状态,4变频器状态,阀门开关状态,停机报警,泵房进水报警状态,停电来电报警状态,门禁报警状态,烟感报警状态 from "+ tablename + " order by 时间 desc limit 1;");
                //其实只有一行。
                foreach (DataRow dr in dt.Rows)
                {
                   if(dr["系统运行状态"].ToString()=="故障")
                    {
                        sb.Append(" 系统运行故障");
                        flag = 1;
                    }
                    if(dr["PLC故障状态"].ToString()=="故障")
                    {
                        sb.Append(" PLC故障");
                        flag = 1;
                    }
                    if(dr["压力报警状态"].ToString()=="超压报警")
                    {
                        sb.Append(" 超压报警");
                        flag = 1;
                    }
                    if(dr["压力报警状态"].ToString() == "低压报警")
                    {
                        sb.Append(" 低压报警");
                        flag = 1;
                    }
                    if(dr["水箱缺水状态"].ToString()=="缺水")
                    {
                        sb.Append(" 水箱缺水");
                        flag = 1;
                    }
                    if(dr["1变频器状态"].ToString()=="故障")
                    {
                        sb.Append(" 1变频器故障");
                        flag = 1;
                    }
                    if (dr["2变频器状态"].ToString() == "故障")
                    {
                        sb.Append(" 2变频器故障");
                        flag = 1;
                    }
                    if (dr["3变频器状态"].ToString() == "故障")
                    {
                        sb.Append(" 3变频器故障");
                        flag = 1;
                    }
                    if (dr["4变频器状态"].ToString() == "故障")
                    {
                        sb.Append(" 4变频器故障");
                        flag = 1;
                    }
                    if (dr["阀门开关状态"].ToString()=="故障")
                    {
                        sb.Append(" 阀门开关故障");
                        flag = 1;
                    }
                    if(dr["停机报警"].ToString()=="负压停机报警")
                    {
                        sb.Append(" 负压停机");
                        flag = 1;
                    }
                    if(dr["停机报警"].ToString()== "超压停机报警")
                    {
                        sb.Append(" 超压停机");
                        flag = 1;
                    }
                    if(dr["泵房进水报警状态"].ToString()=="报警")
                    {
                        sb.Append(" 泵房进水");
                        flag = 1;
                    }
                    if(dr["停电来电报警状态"].ToString()=="停电报警")
                    {
                        sb.Append(" 停电");
                        flag = 1;
                    }
                    if(dr["门禁报警状态"].ToString()=="报警")
                    {
                        sb.Append("门禁报警");
                        flag = 1;
                    }
                    if(dr["烟感报警状态"].ToString()=="报警")
                    {
                        sb.Append("烟感报警");
                        flag = 1;
                    }
                   if(flag==0)
                    {
                        sb.Append("一切正常");
                    }


                }
                dic.Add(tablename, sb.ToString());


            }
            return dic;
        }

        //读取gis上的泵表编号
        public Dictionary<string,string[]> ReadPumproomNum()
        {
            Dictionary<string, string[]> dic = new Dictionary<string, string[]>();
            DataTable dt = MySQLHelper.ExecuteDataTable("select 站点名称,泵表编号 from gispros where IsDeleted='0';");
            foreach(DataRow dr in dt.Rows)
            {
                string[] name = dr["泵表编号"].ToString().Split('-');
                for(int i=0;i<name.Count();i++)
                {
                    name[i] = "pumproom" + name[i];
                }
                dic.Add(dr["站点名称"].ToString(), name);
            }
            return dic;
        }
        //往alarms的表中写入报警信息,更新对应的报警信息表中的字段
        public void WriteAlarm(string msg)
        {
            MySQLHelper.ExecuteNonQuery("update alarms set alarmcontext=@text where Id=1;", new MySqlParameter("text", msg));
        }
        //往alarms的表中插入报警记录。方面以后的查阅
        public void InsertAlarm(string msg)
        {
            MySQLHelper.ExecuteNonQuery("insert into alarms(Time,Alarmcontext) value (@time,@context);", new MySqlParameter("time", DateTime.Now), new MySqlParameter("context", msg));
        }


        //根据站点的名称来更新报警信息。
        public void UpdateMessage(string name,string msg)
        {
            MySQLHelper.ExecuteNonQuery("update gispros set 报警信息=@msg where  站点名称=@name;", new MySqlParameter("msg", msg), new MySqlParameter("name", name));
        }

        public void SetSafe()
        {
            MySQLHelper.ExecuteNonQuery("update gispros set 报警信息=@msg where IsDeleted='0';",new MySqlParameter("msg","一切正常"));
        }

       
    }
}
