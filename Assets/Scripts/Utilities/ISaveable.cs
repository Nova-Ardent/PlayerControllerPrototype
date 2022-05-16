using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public string[] SavePath { get; } 
    public string SaveFile { get; }
    public int Version { get; }
    public bool IgnoreFileNotFound { get; }
    public IEnumerable<byte> GetSavedData();
    public int SetSavedData(byte[] nextBytes, int version);
    public void OnStreamEnd();
}
