using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    Vector3 _lastPosition = Vector3.zero;
    public event Action<Transform> OnTriggInPatternPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggInPatternPoint?.Invoke(collision.transform);
    }

    private void Update()
    {
        // Init case
        if (_lastPosition== Vector3.zero)
        {
            _lastPosition = transform.position;
            return;
        }

        // rotation update
        Vector3 diff = _lastPosition - transform.position;
        if (diff.magnitude > 0.1f)
        {
            diff.Normalize();
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
        }

        // Update internal data
        _lastPosition = transform.position;
        return;
    }

}
