#nullable enable

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float ClampAngle(float angle, float from, float to)
    {
        if (angle < 0f) angle = 360 + angle;
        if (angle > 180f) return Mathf.Max(angle, 360 + from);
        return Mathf.Min(angle, to);
    }

    public static bool DegreeInMinRange(float angle, float endPoint1, float endPoint2)
    {
        angle = angle % 360;
        endPoint1 = endPoint1 % 360;
        endPoint2 = endPoint2 % 360;

        if (angle < 0) angle += 360;
        if (endPoint1 < 0) endPoint1 += 360;
        if (endPoint2 < 0) endPoint2 += 360;

        if (Mathf.Abs(endPoint2 - endPoint1) < 180)
        {
            return InRange(angle, endPoint1, endPoint2);
        }
        else
        {
            return !InRange(angle, endPoint1, endPoint2);
        }
    }

    public static Vector2 DegreeToVector2(float degree)
    {
        return RadToVector2(degree * Mathf.Deg2Rad);
    }

    public static T? FirstOrDefault<T>(this IEnumerable<T> source, Func<T, bool> predicate, T? defaultValue) where T : class
    {
        foreach (var value in source)
        {
            if (predicate(value))
            {
                return value;
            }
        }
        return defaultValue;
    }

    public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
    {
        var enumType = value.GetType();
        var name = Enum.GetName(enumType, value);
        return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
    }

    public static IEnumerable<Enum> GetEnums(Type type)
    {
        foreach (var e in Enum.GetValues(type))
        {
            Enum? ret = e as Enum;
            if (ret == null)
                continue;

            yield return ret;
        }
    }

    public static Vector2 GetMouseFromCenter()
    {
        var mousePos = Input.mousePosition;
        return new Vector2
            ( mousePos.x - Screen.width / 2
            , mousePos.y - Screen.height / 2
            );;
    }

    public static bool InRange(float value, float e1, float e2)
    {
        if (e1 > e2)
        {
            return e1 > value && e2 < value;
        }
        return e2 > value && e1 < value;
    }

    public static string Localize(this Enum value)
    {
        return Localized.Instance.GetDefinition(value);
    }

    public static Vector2 RadToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
}
