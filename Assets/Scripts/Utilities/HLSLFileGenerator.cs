using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public abstract class HLSLFileGenerator : FileGenerator
{
    protected enum Accessibility
    {
        Private,
        Protected,
        Public,
    }

    protected enum ClassUtility
    {
        None,
        Static,
        Sealed,
        Abstract,
    }

    protected IDisposable Stack(char? suffix = null)
    {
        fileText = fileText + OpenBrace();
        scope++;

        return new DisposableAction(() =>
        {
            scope--;
            fileText = fileText + CloseBrace(suffix);
        });
    }

    protected void Line(string text = "")
    {
        fileText = fileText + Scope() + text + "\n";
    }

    private string OpenBrace()
    {
        return Scope() + "{\n";
    }

    private string CloseBrace(char? suffix = null)
    {
        if (suffix == null)
        {
            return Scope() + "}\n";
        }
        return Scope() + $"}}{suffix}\n";
    }

    private string Scope()
    {
        return new string(' ', scope * 4);
    }

}
