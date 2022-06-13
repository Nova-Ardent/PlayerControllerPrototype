using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public abstract class CSharpFileGenerator : FileGenerator
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

    protected IDisposable Class(string name, Accessibility accessibility = Accessibility.Private, ClassUtility classUtility = ClassUtility.None, string? extends = null)
    {
        fileText = fileText + Scope();
        switch (accessibility)
        {
            case Accessibility.Private: fileText = fileText + "private "; break;
            case Accessibility.Protected: fileText = fileText + "protected "; break;
            case Accessibility.Public: fileText = fileText + "public "; break;
        }

        switch (classUtility)
        {
            default:
            case ClassUtility.None:
                break;
            case ClassUtility.Static: fileText = fileText + "static "; break;
            case ClassUtility.Sealed: fileText = fileText + "sealed "; break;
            case ClassUtility.Abstract: fileText = fileText + "abstract "; break;
        }

        if (string.IsNullOrEmpty(extends))
        {
            fileText = fileText + "class " + name + "\n";
        }
        else
        {
            fileText = fileText + "class " + name + $" : {extends}\n";
        }
        return Stack();
    }

    protected IDisposable Struct(string name, Accessibility accessibility = Accessibility.Private)
    {
        fileText = fileText + Scope();
        switch (accessibility)
        {
            case Accessibility.Private: fileText = fileText + "private "; break;
            case Accessibility.Protected: fileText = fileText + "protected "; break;
            case Accessibility.Public: fileText = fileText + "public "; break;
        }

        fileText = fileText + "struct " + name + "\n";
        return Stack();
    }

    protected IDisposable Enum(string name, Accessibility accessibility = Accessibility.Private)
    {
        fileText = fileText + Scope();
        switch (accessibility)
        {
            case Accessibility.Private: fileText = fileText + "private "; break;
            case Accessibility.Protected: fileText = fileText + "protected "; break;
            case Accessibility.Public: fileText = fileText + "public "; break;
        }

        fileText = fileText + "enum " + name + "\n";
        return Stack();
    }

    protected IDisposable Function(string name, Accessibility accessibility = Accessibility.Private, string? ret = null, params string[] args)
    {
        fileText = fileText + Scope();
        switch (accessibility)
        {
            case Accessibility.Private: fileText = fileText + "private "; break;
            case Accessibility.Protected: fileText = fileText + "protected "; break;
            case Accessibility.Public: fileText = fileText + "public "; break;
        }

        if (!string.IsNullOrEmpty(ret))
        {
            fileText = fileText + ret + " ";
        }
        fileText = fileText + name + "(";
        for (int i = 0; i < args.Length; i++)
        {
            fileText = fileText + args[i];
            if (i < args.Length - 1)
            {
                fileText = fileText + ",";
            }
            else
            {
                fileText = fileText + ")\n";
            }
        }

        return Stack();
    }

    protected IDisposable Switch(string arg)
    {
        fileText = fileText + Scope();

        fileText = fileText + "switch( " + arg + ")\n";
        return Stack();
    }

    protected IDisposable NameSpace(string name)
    {
        fileText = fileText + Scope();

        fileText = fileText + "namespace " + name + "\n";
        fileText = fileText + OpenBrace();
        scope++;

        return new DisposableAction(() =>
        {
            scope--;
            fileText = fileText + CloseBrace();
        });
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
