using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public string[] SavePath { get; } 
    public string SaveFile { get; }
    public int Version { get; }
    public IEnumerable<byte> GetSavedData();
}
