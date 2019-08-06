using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] Text _text;

    Coroutine _timerRoutine;
    SpecialCountDown _scd;

    public event Action OnTimeOut;

    public float Duration => _scd.RemainingTime;

    public void StartTimer(float duration)
    {
        _timerRoutine = StartCoroutine(TimerRoutine());
        IEnumerator TimerRoutine()
        {
            _scd = new SpecialCountDown(duration);
            while (!_scd.isDone)
            {
                int seconds = (int)_scd.RemainingTime;
                if(seconds >= 60)
                    _text.text = $"{(seconds/60<0?"0": string.Empty)}{seconds/60} : {(seconds % 60 < 10 ? "0" : string.Empty)}{seconds % 60}";
                else if(seconds >= 10)
                    _text.text = $"{seconds % 60}";
                else 
                    _text.text = $"0{seconds % 60}";
                yield return null;
            }

            OnTimeOut?.Invoke();
            yield break;
        }

    }


}
