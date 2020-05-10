using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class IOUtil
{
    public static void Write (string key , object data )
    {
        string path = Application.persistentDataPath + key + ".json";
        var fileStream = File.Open(path, FileMode.OpenOrCreate);
        byte[] bytes = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
        fileStream.Flush();
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
    }

    public static T Read<T>(string key) where T : class
    {
        string path = Application.persistentDataPath + key + ".json";
        if (File.Exists(path))
        {
            var fileStream = File.Open(path, FileMode.Open);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            return JsonUtility.FromJson<T>(Encoding.UTF8.GetString(bytes));
        }
        return null;
    }
}
