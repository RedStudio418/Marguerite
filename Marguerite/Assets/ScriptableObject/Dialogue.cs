using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Marguerite/Dialog")]
public class Dialogue : ScriptableObject
{
    [SerializeField] LocalizedText[] _dialog;

    internal IEnumerable<string> Dialog => _dialog.Select(i => i.Get());

}
