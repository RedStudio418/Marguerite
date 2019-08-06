using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragMecaniq : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] TouchTracker _touch;

    [Header("UI")]
    [SerializeField] Timer _timer;
    [SerializeField] float _gameOverTime;

    [Header("All pattern in game")]
    [SerializeField] Pattern[] _patterns;

    [Header("Change Alphabet to Numbers Conf")]
    [SerializeField] Sprite[] _numbers;

    [Header("Animations")]
    [SerializeField] Animation _tutoAnimation;
    [SerializeField] AnimationClip _tutoClip;

    Transform _reachedPoint;

    public event Action OnGameOver;

    /// <summary>
    /// Cheat in Editor
    /// </summary>
    public void EditorLaunch()
    {
        gameObject.SetActive(true);
        StartCoroutine(DragMecanique(null));
    }

    public IEnumerator DragMecanique(Action<float> scoring)
    {
        // Prepare UI
        gameObject.SetActive(true);
        _timer.StartTimer(_gameOverTime);

        // Prepare Cursor Events
        _touch.OnTriggInPatternPoint += UpdateReachedPoint;
        void UpdateReachedPoint(Transform t) {
            _reachedPoint = t;
            Debug.Log($"Reached Point : {_reachedPoint.name}");
        }

        // Switch number to Alphabet
        if(UnityEngine.Random.Range(0,2) == 1)
        {
            int idx = 0;
            foreach(var el in _patterns)
            {
                foreach(var point in el.Points)
                {
                    point.GetComponent<Image>().sprite = _numbers[idx];
                    if (++idx >= _numbers.Length) idx = 0;
                }
            }
        }

        // Launch the pattern routine
        Coroutine _patternRoutine = StartCoroutine(PatternRoutine());
        IEnumerator PatternRoutine()
        {
            bool firstPattern = false;
            foreach(var el in _patterns)
            {
                if(!firstPattern)
                {
                    _tutoAnimation.Play(_tutoClip.name);
                    firstPattern = true;
                    yield return PlayPattern(el, true);

                    _tutoAnimation.gameObject.SetActive(false);
                }
                else
                {
                    yield return PlayPattern(el, false);
                }
            }
        }

        // Prepare for GameOver
        _timer.OnTimeOut += GameOver;
        void GameOver()
        {
            StopCoroutine(_patternRoutine);
            foreach (var el in _patterns) el.gameObject.SetActive(false);
            OnGameOver?.Invoke();
        }

        // Wait the right PatternRoutine
        yield return _patternRoutine;
        
        // Inject final score
        var score = _timer.Duration;
        scoring?.Invoke(score);

        // Cleanup
        _touch.OnTriggInPatternPoint -= UpdateReachedPoint;
        gameObject.SetActive(false);
        yield break;
    }

    /// <summary>
    /// Play given pattern. Must collide with all points
    /// </summary>
    /// <param name="p">current Pattern</param>
    /// <param name="showAll">must show all point or as you go</param>
    /// <returns></returns>
    IEnumerator PlayPattern(Pattern p, bool showAll=false)
    {
        // Prepare UI
        p.gameObject.SetActive(true);
        foreach (var el in p.Points) el.gameObject.SetActive(showAll);
        
        // Gameplay mecaniq
        foreach (var el in p.Points)
        {
            Debug.Log($"Waiting for {el.name}");
            el.gameObject.SetActive(true);
            yield return new WaitWhile(() => _reachedPoint != el);
            el.GetChild(1).gameObject.SetActive(true);  // In real world don't use GetChild
            Debug.Log($"Reached");
        }

        // Clean up
        foreach (var el in p.Points) el.gameObject.SetActive(false);
        p.gameObject.SetActive(false);
    }

}
