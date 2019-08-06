using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchTracker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Cursor _pointerFollow;
    [SerializeField] Camera _usedCamera;

    Coroutine _tracker;

    public event Action<Transform> OnTriggInPatternPoint { add => _pointerFollow.OnTriggInPatternPoint += value; remove => _pointerFollow.OnTriggInPatternPoint -= value; }

    IEnumerator Tracker(PointerEventData eventData)
    {
        while (true)
        {
            var tmp = _usedCamera.ScreenToWorldPoint(eventData.position);
            _pointerFollow.transform.position =  new Vector3(tmp.x,tmp.y, 0);

            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_tracker != null) return;
        _tracker = StartCoroutine(Tracker(eventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_tracker != null)
        {
            StopCoroutine(_tracker);
            _tracker = null;
        }
    }
}
