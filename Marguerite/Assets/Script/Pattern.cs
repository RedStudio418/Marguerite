using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a container for all point in a pattern
/// </summary>
public class Pattern : MonoBehaviour
{
    [SerializeField] Transform[] _pattern;

    internal IEnumerable<Transform> Points => _pattern;
}
