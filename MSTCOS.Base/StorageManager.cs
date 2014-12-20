using System;
using System.Collections.Generic;

using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MSTCOS.Base
{
    /// <summary>
    /// 管理序列化存储的类
    /// </summary>
    public class StorageManager
    {
        public static void SaveData(Object target, string file)
        {
            
            FileStream fs = new FileStream(file, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fs, target);
            fs.Close();
        }
        public static T LoadData<T>(string file)
        {
            
                T target;
                FileStream fs = null;
                try
                {

                    fs = new FileStream(file, FileMode.Open);
                    BinaryFormatter formatter = new BinaryFormatter();
                    target = (T)formatter.Deserialize(fs);
                    fs.Close();
                    return target;

                }
                catch
                {
                    if (fs != null)
                    {

                        fs.Close();
                    }
                    throw;
                }
               

        }
    }
}
