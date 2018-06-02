using Newtonsoft.Json;
using System.IO;
using UnityEngine;


public class JsonIO
{
    public static void Save(string path, object obj)
    {        
        File.WriteAllText(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
        Debug.Log(string.Format("File saved at {0}", path));
    }
}
