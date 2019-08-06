using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Easy script to launch SFX on Enable
/// </summary>
public class LaunchSFX_OnStart : MonoBehaviour
{
    [SerializeField] string _key;
    private void OnEnable() => SFXLauncher.Instance.Launch(_key);
}
