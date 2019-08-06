using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SFXLauncher : MonoBehaviour
{
    [System.Serializable] class SFXLaunch
    {
        public string Key;
        public AudioSource Source;
    }

    /// <summary>
    /// Singleton because I was lazy. 
    /// </summary>
    public static SFXLauncher Instance;
    [SerializeField] SFXLaunch[] _sfxLauncher;

    private void Awake()
    {
        Instance = this;    // Didn't have time to manage the right dependency injection. Use Zenject.
    }

    
    public void Launch(string key)
    {
        // Key as string are often bad, use ScriptableObject instead when you have more time
        var result = _sfxLauncher.Where(i => i.Key == key).FirstOrDefault();
        result?.Source.Play();
    }


}
