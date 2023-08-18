using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Card
{
    public class DataSaveLoader 
    {
        public void SaveData(string _data)
        {
            string filePath = Application.persistentDataPath + "/Data.json";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using FileStream stream = File.Create(filePath);
            stream.Close();
            File.WriteAllText(filePath, _data);
        }

        public string LoadData()
        {
            string filePath = Application.persistentDataPath + "/Data.json";
            if (File.Exists(filePath) == false)
            {
                return "";
            }
            else
            {
                string data = File.ReadAllText(filePath);
                return data;
            }
        }
    }
}
