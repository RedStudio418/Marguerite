using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class CoroutineExtension
{
    
    public static IEnumerator PlayAndWait(this Animation @this, AnimationClip clip) => @this.PlayAndWait(clip.name);
    public static IEnumerator PlayAndWait(this Animation @this, string name)
    {
        @this.Play(name);
        yield return new WaitWhile(() => @this.IsPlaying(name));
    }

}

public static class Extension {

    private readonly static Func<int, int, int> _classicUnityRandom = 
        (min, max) => UnityEngine.Random.Range(min, max);
    
    public static void Shuffle<T>(this List<T> @this, Func<int,int,int> randomProvider=null)
    {
        if (randomProvider == null) randomProvider = _classicUnityRandom;
        int n = @this.Count;
        while (n > 1)
        {
            n--;
            int k = randomProvider(0, n + 1);
            T value = @this[k];
            @this[k] = @this[n];
            @this[n] = value;
        }
    }

}

