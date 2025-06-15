using System;
using System.IO;
using UnityEngine;

[Serializable]
public class CamSettings
{
    public int width;
    public int height;
    public int frameRate;
}

[Serializable]
public class Settings
{
    public string horizontal;
    public string vertical;
    public CamSettings camSettings;
    public float enableTime;
    public float disableTime;
    public string[] videoUrl;

}

public class JsonSaver : MonoBehaviour
{
    public static JsonSaver instance;
    [NonSerialized] public Settings Settings;

    public string[] fixedUrl;
    
    private void Awake()
    {
        instance = this;
        
        Settings =  LoadJsonData<Settings>("settings.json");
        
        SetPath(Settings.videoUrl,out fixedUrl);
    }
    
    private T LoadJsonData<T>(string fileName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        filePath = filePath.Replace("\\", "/");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log("Loaded JSON: " + json); // JSON 문자열 출력
            return JsonUtility.FromJson<T>(json);
        }

        Debug.LogWarning("File does not exist!");
        return default;
    }
    
    private void SetPath(string[] path, out string[] fixedPath)
    {
        if (path == null)
        {
            fixedPath = null; // 길이가 0일 때는 null 반환
            return;
        }

        int length = path.Length;
        fixedPath = new string[length];

        //  Debug.Log("path Length : " + length);
        for (int i = 0; i < length; i++)
        {
            fixedPath[i] = Path.Combine(Application.streamingAssetsPath, path[i]);
            fixedPath[i] = fixedPath[i].Replace("\\", "/");
            //  Debug.Log("fixedPath : " + fixedPath[i]);
        }
    }
}
