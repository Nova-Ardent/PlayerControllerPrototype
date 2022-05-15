using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;

public static class SaveUtilities
{
    public static string SaveData(ISaveable saveable)
    {
        //var filePath = Path.Combine(Application.persistentDataPath, saveable.SavePath, saveable.SaveFile);
        var folderPath = Path.Combine(saveable.SavePath.Prepend(Application.persistentDataPath).ToArray());
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(saveable.SavePath.Prepend(Application.persistentDataPath).Append(saveable.SaveFile).ToArray());

        FileStream file = File.Create(filePath);
        foreach (var b in saveable.GetSavedData())
        {
            file.WriteByte(b);
        }
        file.Close();
        return filePath;
    }
}
