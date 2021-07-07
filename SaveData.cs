using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

public class SaveData
{
    private static SaveDataBase savedatabase = null;

    private static SaveDataBase Savedatabase
    {
        get
        {
            if (savedatabase == null)
            {
                string path;
                string fileName = "Save.json";
                #if UNITY_EDITOR
                    path = Directory.GetCurrentDirectory() + "/Save/";
                #else
                    path = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "/Save/";
                #endif
                savedatabase = new SaveDataBase(path, fileName);
            }
            return savedatabase;
        }
    }

#region Public Static Methods
        
    public static void SetInt(string key, int value)
    {
        Savedatabase.SetInt(key, value);
    }

    public static void SetFloat(string key, float value)
    {
        Savedatabase.SetFloat(key, value);
    }

    public static void SetBool(string key, float value)
    {
        Savedatabase.SetBool(key, value);
    }

    public static void SetString(string key, string value)
    {
        Savedatabase.SetString(key, value);
    }

    public static void SetClass<T>(string key, T obj) where T : class, new()
    {
        Savedatabase.SetClass<T>(key, obj);
    }

    public static void SetList<T>(string key, List<T> list)
    {
        Savedatabase.SetList<T>(key, list);
    }



    public static int GetInt(string key, int _default = default)
    {
        return Savedatabase.GetInt(key, _default);
    }

    public static float GetFloat(string key, float _default = default)
    {
        return Savedatabase.GetFloat(key, _default);
    }

    public static bool GetBool(string key, bool _default = default)
    {
        return Savedatabase.GetBool(key, _default);
    }

    public static string GetString(string key, string _default = default)
    {
        return Savedatabase.GetString(key, _default);
    }

    public static T GetClass<T>(string key, T _default = default) where T : class, new()
    {
        return Savedatabase.GetClass(key, _default);
    }
    
    public static List<T> GetList<T>(string key, List<T> _default = default)
    {
        return Savedatabase.GetList<T>(key, _default);
    }

    public static void Clear()
    {
        Savedatabase.Clear();
    }

    public static void Remove(string key)
    {
        Savedatabase.Remove(key);
    }

    public static bool ContainsKey(string _key)
    {
        return Savedatabase.ContainsKey(_key);
    }

    public static List<string> Keys()
    {
        return Savedatabase.Keys();
    }

    public static void Save()
    {
        Savedatabase.Save();
    }

#endregion

#region SaveDatabase Class

    [Serializable]
    private class SaveDataBase
    {
        #region Fields

        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private Dictionary<string, string> saveDictionary;

#endregion

#region Constructor&Destructor

        public SaveDataBase(string _path, string _fileName)
        {
            path = _path;
            fileName = _fileName;
            saveDictionary = new Dictionary<string, string>();
            Load();

        }

        //~SaveDataBase()
        //{
        //    Save();
        //}

        #endregion

        #region Public Methods

        public void SetInt(string key, int value)
        {
            KeyCheck(key);
            saveDictionary[key] = value.ToString();
        }

        public void SetFloat(string key, float value)
        {
            KeyCheck(key);
            saveDictionary[key] = value.ToString();
        }

        public void SetBool(string key, float value)
        {
            KeyCheck(key);
            saveDictionary[key] = value.ToString();
        }

        public void SetString(string key, string value)
        {
            KeyCheck(key);
            saveDictionary[key] = value;
        }

        public void SetClass<T>(string key, T obj) where T : class, new()
        {
            KeyCheck(key);
            string json = JsonUtility.ToJson(obj);
            saveDictionary[key] = json;
        }

        public void SetList<T>(string key, List<T> list)
        {
            KeyCheck(key);
            var serializableList = new Serialization<T>(list);
            string json = JsonUtility.ToJson(serializableList);
            saveDictionary[key] = json;
        }



        public int GetInt(string key, int _default)
        {
            KeyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                return _default;
            int ret;
            if (!int.TryParse(saveDictionary[key], out ret))
            {
                ret = 0;
            }
            return ret;
        }

        public float GetFloat(string key, float _default)
        {
            float ret;
            KeyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                ret = _default;

            if (!float.TryParse(saveDictionary[key], out ret))
            {
                ret = 0.0f;
            }
            return ret;
        }

        public bool GetBool(string key, bool _default)
        {
            bool ret;
            KeyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                ret = default;

            if (!bool.TryParse(saveDictionary[key], out ret))
            {
                ret = default;
            }

            return ret;
        }

        public string GetString(string key, string _default)
        {
            KeyCheck(key);

            if (!saveDictionary.ContainsKey(key))
                return _default;
            return saveDictionary[key];
        }

        public T GetClass<T>(string key, T _default) where T : class, new()
        {
            KeyCheck(key);
            if (!saveDictionary.ContainsKey(key))
                return _default;

            string json = saveDictionary[key];
            T obj = JsonUtility.FromJson<T>(json);
            return obj;

        }

        public List<T> GetList<T>(string key, List<T> _default)
        {
            KeyCheck(key);
            if (!saveDictionary.ContainsKey(key))
            {
                return _default;
            }
            string json = saveDictionary[key];
            Serialization<T> deserializeList = JsonUtility.FromJson<Serialization<T>>(json);

            return deserializeList.ToList();
        }

        public void Clear()
        {
            saveDictionary.Clear();
        }

        public void Remove(string key)
        {
            KeyCheck(key);
            if (saveDictionary.ContainsKey(key))
            {
                saveDictionary.Remove(key);
            }
        }

        public bool ContainsKey(string _key)
        {
            return saveDictionary.ContainsKey(_key);
        }

        public List<string> Keys()
        {
            return saveDictionary.Keys.ToList<string>();
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(path + fileName, false, Encoding.GetEncoding("utf-8")))
            {
                var serialDict = new Serialization<string, string>(saveDictionary);
                serialDict.OnBeforeSerialize();
                string dictJsonString = JsonUtility.ToJson(serialDict);
                writer.WriteLine(dictJsonString);
            }
        }

        public void Load()
        {
            if (File.Exists(path + fileName))
            {
                using (StreamReader sr = new StreamReader(path + fileName, Encoding.GetEncoding("utf-8")))
                {
                    if (saveDictionary != null)
                    {
                        var sDict = JsonUtility.FromJson<Serialization<string, string>>(sr.ReadToEnd());
                        sDict.OnAfterDeserialize();
                        saveDictionary = sDict.ToDictionary();
                    }
                }
            }
            else { saveDictionary = new Dictionary<string, string>(); }
        }

        public string GetJsonString(string key)
        {
            KeyCheck(key);
            if (saveDictionary.ContainsKey(key))
            {
                return saveDictionary[key];
            }
            else
            {
                return null;
            }
        }

#endregion

#region Private Methods
        
        private void KeyCheck(string _key)
        {
            if (string.IsNullOrEmpty(_key))
            {
                throw new ArgumentException("invalid key!!");
            }
        }

#endregion
    }

#endregion

#region Serialization Class
    
    [Serializable]
    private class Serialization<T>
    {
        public List<T> target;

        public List<T> ToList()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(List<T> target)
        {
            this.target = target;
        }
    }

    [Serializable]
    private class Serialization<TKey, TValue>
    {
        public List<TKey> keys;
        public List<TValue> values;
        private Dictionary<TKey, TValue> target;

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return target;
        }

        public Serialization()
        {
        }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            this.target = target;
        }

        public void OnBeforeSerialize()
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public void OnAfterDeserialize()
        {
            int count = Math.Min(keys.Count, values.Count);
            target = new Dictionary<TKey, TValue>(count);
            Enumerable.Range(0, count).ToList().ForEach(i => target.Add(keys[i], values[i]));
        }
    }

#endregion
}