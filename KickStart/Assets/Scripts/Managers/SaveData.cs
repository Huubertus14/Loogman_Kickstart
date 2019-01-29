using Newtonsoft.Json;
using System.Text;
using UnityEngine;


public class SaveData
{
    public int[] ReadData(string filename)
    {
        string path = string.Format("{0}/{1}.json", Application.persistentDataPath, filename);

        byte[] data = UnityEngine.Windows.File.ReadAllBytes(path);
        string json = Encoding.ASCII.GetString(data);

        int[] obj = JsonConvert.DeserializeObject<int[]>(json);
        Debug.Log("Data Loaded");
        return obj;
    }

    public void SaveScore(string filename, int[] obj)
    {
        string path = string.Format("{0}/{1}.json", Application.persistentDataPath, filename);

        string json = JsonConvert.SerializeObject(obj);
        byte[] data = Encoding.ASCII.GetBytes(json);

        UnityEngine.Windows.File.WriteAllBytes(path, data);
        Debug.Log("Saved");
    }
}
