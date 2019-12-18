using System;
using System.Collections.Generic;
using System.Linq;
using UniKid;
using UniKid.Core;
using UniKid.Core.Model;
using UnityEngine;

public static class Utils
{
    private static readonly Dictionary<tk2dSpriteAnimationClip, tk2dSpriteAnimationClip> reverseTable = new Dictionary<tk2dSpriteAnimationClip, tk2dSpriteAnimationClip>();

    public static double UtcNowAO { get { return DateTime.UtcNow.ToOADate(); } }

    public static tk2dSpriteAnimationClip Reverse(this tk2dSpriteAnimationClip clip)
    {
        reverseTable.Clear();
        tk2dSpriteAnimationClip reversed = null;

        if (!reverseTable.TryGetValue(clip, out reversed))
        {
            reversed = new tk2dSpriteAnimationClip();
            reversed.CopyFrom(clip);
            Array.Reverse(reversed.frames);
            reverseTable.Add(clip, reversed);
            reverseTable.Add(reversed, clip);
        }

        return reversed;
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        var rng = new System.Random();
        var n = list.Count;

        while (n > 1)
        {
            n--;
            var k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    

    public static List<string> SplitSequence(this string charSequenceString, bool toUpper = false)
    {
        if (string.IsNullOrEmpty(charSequenceString)) return new List<string>();

        if (toUpper) charSequenceString = charSequenceString.ToUpper();

        return charSequenceString.Split(new[] { Const.SEPARATE_CHAR }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    
    public static Vector2 GetWorldPos(this Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        // we solve for intersection with z = 0 plane
        float t = -ray.origin.z / ray.direction.z;

        return ray.GetPoint(t);
    }


    #region Char Sprite Info

    public static string GetCharSpriteName(string charLibraryNameSequence, char charName)
    {
        return GetCharSpriteName(charLibraryNameSequence, charName.ToString());
    }

    public static string GetCharSpriteName(string charLibraryNameSequence, string charName)
    {
        var libName = string.Empty;
        var charIndex = -1;

        var charLiraryNameList = charLibraryNameSequence.SplitSequence(true).ToList();
        var charLiraryList = CoreContext.Settings.CharLibraryArray.Where(x => charLiraryNameList.Contains(x.Name.ToUpper()));

        DetectCharInfo(charLiraryList, charName, ref libName, ref charIndex);

        return string.Format("char_{0}_{1}", libName, charIndex.ToString("00"));

    }

    #endregion

    #region Helpers

    private static void DetectCharInfo(IEnumerable<CharLibrary> charLiraryList, string charName, ref string libName, ref int charIndex)
    {
        var charLibraries = charLiraryList as CharLibrary[] ?? charLiraryList.ToArray();

        foreach (var lib in charLibraries)
        {
            var index = Utils.SplitSequence(lib.Sequence, true).ToList().IndexOf(charName.ToUpper());

            if (index < 0) continue;

            libName = lib.Name;
            charIndex = index;
            return;
        }

        if (charName.Equals(Const.UNKNOWN_CHAR.ToString()))
        {
            DetectCharInfo(charLibraries, Const.UNKNOWN_SYMBOL_NAME, ref libName, ref charIndex);
        }

    }

    #endregion


    public static float FindMax<T>(this IEnumerable<T> list, Converter<T, float> projection)
	{
        if (!list.Any()) throw new InvalidOperationException("Empty list");
	    
        var maxValue = float.MinValue;

	    foreach (T item in list)
	    {
	        var value = projection(item);
	        if (value > maxValue) maxValue = value;
	    }

	    return maxValue;
	}
	
	public static int FindMax<T>(this IEnumerable<T> list, Converter<T, int> projection)
	{
        if (!list.Any()) throw new InvalidOperationException("Empty list");

        var maxValue = int.MinValue;

        foreach (T item in list)
        {
            var value = projection(item);
            if (value > maxValue) maxValue = value;
        }

        return maxValue;
	}


    public static T[] AddRange<T>(this T[] sequence, T[] items)
    {
        return (sequence ?? Enumerable.Empty<T>()).Concat(items).ToArray();
    }

    public static T[] Add<T>(this T[] sequence, T item)
    {
        return (sequence ?? Enumerable.Empty<T>()).Concat(new[] { item }).ToArray();
    }
	
    public static void ForEach<T>(this T[] sequence, Action<T> action)
    {
        Array.ForEach(sequence, action);
    }

    public static void Sort<T>(this T[] sequence, Comparison<T> comparison)
    {
        Array.Sort(sequence, comparison);
    }

    public static T[] Clear<T>(this T[] sequence)
    {
        return new T[0];
    }
}
