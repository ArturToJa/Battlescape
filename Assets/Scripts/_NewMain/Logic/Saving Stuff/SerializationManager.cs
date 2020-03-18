using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BattlescapeLogic
{
    public class SerializationManager
    {
        static SerializationManager _instance;
        public static SerializationManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SerializationManager();
                }
                return _instance;
            }
        }

        public bool Save(string name, string directory, object saveData)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }
            string path = directory + "/" + name + ".save";
            FileStream file = File.Create(path);
            binaryFormatter.Serialize(file, saveData);
            file.Close();
            return true;
        }

        public object Load(string path)
        {
            if (File.Exists(path) == false)
            {
                return null;
            }
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            try
            {
                object save = binaryFormatter.Deserialize(file);
                file.Close();
                return save;
            }
            catch
            {
                Debug.LogError("Failed to load: " + path);
                file.Close();
                return null;
            }
        }

    }
}
