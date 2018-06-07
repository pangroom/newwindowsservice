using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsService1.MySQL.DAL;

namespace WindowsService1.MySQL.BLL
{
    public class Ana_BLL
    {
        //从station中获取pumproom
        public IEnumerable<string> GetPumproom()
        {
            return new Ana_DAL().GetPumproom();
        }
        //从数据库中获取pumproom
        public IEnumerable<string> ReadPumpRoom()
        {
            return new Ana_DAL().ReadPumpRoom();
        }
        //读取数据库中所有pumproom的状态返回dic
        public Dictionary<string, string> ReadAlarm()
        {
            return new Ana_DAL().ReadAlarm();
        }
        //从gispros中读取泵表编号和和对应的站点名称，返回dic
        public Dictionary<string, string[]> ReadPumproomNum()
        {
            return new Ana_DAL().ReadPumproomNum();
        }
        public void WriteAlarm(string msg)
        {
            new Ana_DAL().WriteAlarm(msg);
        }
        public void InsertAlarm(string msg)
        {
            new Ana_DAL().InsertAlarm(msg);
        }
        public void UpdateMessage(string name, string msg)
        {
            new Ana_DAL().UpdateMessage(name, msg);
        }
        public void SetSafe()
        {
            new Ana_DAL().SetSafe();
        }
        //逻辑操作，首先查询所有的pumproom的状态然后将状态信息，然后查询station中的泵表编号然后根据站点名称写入报警信息中然后。
        public void CircleDetect()
        {
            //获取所有的pumproom 的状态
            Dictionary<string,string> dicalarm= ReadAlarm();
            Dictionary<string, string[]> dicgis = ReadPumproomNum();//看那个泵站中包括这个表
            StringBuilder alarmcontext = new StringBuilder();
            int flag = 0;
            //对每一个pumproom的状态进行分析
            foreach(var alarm in dicalarm)
            { 
                
                //如果出现报警信息 ，需要得到是哪一个的站点            
                if (dicalarm[alarm.Key] !="一切正常")
                {
                    flag = 1;
                    string s = "";
                    foreach(var gis in dicgis)
                    {
                        //如果对应的这个dic中包含这个泵房的表的时候，然后就直接生成对应的报警信息。
                        if(dicgis[gis.Key].Contains(alarm.Key))
                        {
                            //alarmkey是对应的表的名称，可以从这里得出泵站的那个泵出现异常。
                            //此处的gis.key就是泵站的名称。
                            s = gis.Key + dicalarm[alarm.Key];
                            UpdateMessage(gis.Key, "出现异常");
                            //根据泵站的名称来更新对应的状态信息。
                            break;
                        }
                    }
                    alarmcontext.Append(s);
                }

                
            }
            //如果没有任何的报警信息的话，赋值一切正常
            if(flag==0)
            {
                alarmcontext.Append("一切正常");
                SetSafe();

            }
            if (flag==1)
            {
                //往alarm中插入记录
                InsertAlarm(alarmcontext.ToString());
                //需要将对应的报警的信息写到相应的，位置

            }
            //往数据库中对应的字段中写入。
            WriteAlarm(alarmcontext.ToString());
        }
    }
}
