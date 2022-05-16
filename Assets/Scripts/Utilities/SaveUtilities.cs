using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

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
        foreach (var version in BitConverter.GetBytes(saveable.Version))
        {
            file.WriteByte(version);
        }

        foreach (var b in saveable.GetSavedData())
        {
            file.WriteByte(b);
        }
        file.Close();
        return filePath;
    }

    public static void LoadData(ISaveable saveable)
    {
        var filePath = Path.Combine(saveable.SavePath.Prepend(Application.persistentDataPath).Append(saveable.SaveFile).ToArray());
        if (!File.Exists(filePath))
        {
            if (!saveable.IgnoreFileNotFound)
            {
                Debug.LogWarning($"couldn't find worldData for {filePath}");
            }
            return;
        }

        FileStream file = File.OpenRead(filePath);
        BinaryReader binaryReader = new BinaryReader(file);
        
        byte[] lastRead = binaryReader.ReadBytes(sizeof(int));
        int version = BitConverter.ToInt32(lastRead, 0);

        int readNext;
        while ((readNext = saveable.SetSavedData(lastRead, version)) != 0 && binaryReader.BaseStream.Position != binaryReader.BaseStream.Length)
        {
            lastRead = binaryReader.ReadBytes(readNext);
        }

        saveable.OnStreamEnd();
        file.Close();
    }
}
