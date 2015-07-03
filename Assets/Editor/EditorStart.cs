using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class Startup 
{
    static Startup()
    {		
        Debug.Log("Loading Item Database...");
		string path = Application.dataPath + "/Resources/" + GameplayItemManager.BIN_FILE_NAME + GameplayItemManager.BIN_EXT;
        string err = "";// GameplayItemManager.Singleton.LoadFromBinary(path);
        if(err != string.Empty)
		{
			Debug.LogError(err);
		}
		Debug.Log("Success");
    }
}
