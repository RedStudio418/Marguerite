using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager show right Text "Il m'aime, un peu, beaucoup etc."
/// </summary>
public class TextPetal : MonoBehaviour
{
    [Header("Conf")]
    [SerializeField] LocalizedText[] _textLoop;
    [SerializeField] AnimationCurve _opacityAnimation;

    [Header("UI")]        
    [SerializeField] Text _textToChange;
    [SerializeField] CanvasGroup _textCanvasGroup;

    Coroutine _textAnimation;

    public int CurrentIndex { get; private set; }

    /// <summary>
    /// Cheat in order to test every ending
    /// </summary>
    /// <param name="i"></param>
    public void _EditorForceIndex(int i) => CurrentIndex = i;

    private void Awake()
    {
        CurrentIndex = -1;
        _textCanvasGroup.alpha = 0;
    }

    /// <summary>
    /// Text animation
    /// </summary>
    public void NextText()
    {
        // We must have only one Coroutine running at the same time
        if (_textAnimation != null) StopCoroutine(_textAnimation);
            
        _textAnimation = StartCoroutine(ShowTextRoutine());
        IEnumerator ShowTextRoutine()
        {
            CurrentIndex++;
            if (CurrentIndex >= _textLoop.Length) CurrentIndex = 0;

            _textToChange.text = _textLoop[CurrentIndex].Get();

            SpecialCountDown scd = new SpecialCountDown(_opacityAnimation.keys.Last().time);
            while(!scd.isDone)
            {
                _textCanvasGroup.alpha = _opacityAnimation.Evaluate(scd.Progress);
                yield return null;
            }

            _textAnimation = null;
            yield break;
        }
    }



}


[System.Serializable]
public class LocalizedText
{
    [SerializeField] string FR;
    [SerializeField] string EN;
    public string Get()
    {
        return FR;
        /* 
         * Finally we don't localize the game because the game in english is "like me, like me not" which completely change the game
         */
#if false
        if (Application.systemLanguage == SystemLanguage.French)
            return FR;
        else
            return EN;
#endif
    }
}