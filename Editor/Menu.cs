using System.IO;
using UnityEditor;
using UnityEngine;

public static class Menu
{
    public const string MENU_PATH = @"Tools/Saving System/";

    [MenuItem(MENU_PATH + "Clear save data", false, 100)]
    static void ClearSaveData()
    {
        string path = GetPath();
        File.Delete(path);
        Debug.Log("Game data cleared...");
    }

    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "SaveData.sav"); 
    }
}
