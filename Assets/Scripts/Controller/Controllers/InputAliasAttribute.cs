using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class InputAliasAttribute : Attribute
{
    public string alias { get; private set; }

    public InputAliasAttribute(string alias)
    {
        this.alias = alias;
    }
}
