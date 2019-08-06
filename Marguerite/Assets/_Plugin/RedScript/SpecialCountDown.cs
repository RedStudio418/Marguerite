using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCountDown {

    float _startTime;
    float _duration;
    bool _useRealTime;
    bool _working;

    /// <summary>
    /// Progression between 0 and 1
    /// </summary>
    public float Progress => _working ? Math.Min(((_useRealTime ? Time.realtimeSinceStartup:Time.fixedTime) - _startTime) / (_duration), 1f) : -1;
    public bool isDone => _working ? Mathf.Approximately(Progress, 1f):false;

    public bool isWorking => _working;
    public float ConsumedTime => _working ? (_useRealTime?Time.realtimeSinceStartup:Time.fixedTime - _startTime) : -1;
    public float RemainingTime => _working ? (_useRealTime?Time.realtimeSinceStartup:_duration - ConsumedTime) : -1;

    public SpecialCountDown(float duration, bool autoStart=true, bool useRealTime=false)
    {
        if (duration < 0) throw new ArgumentException("Must be positiv", nameof(duration));

        _useRealTime = useRealTime;
        _duration = duration;
        _startTime = autoStart ? (_useRealTime ? Time.realtimeSinceStartup : Time.fixedTime) : -1f;
        _working = autoStart;
    }

    public void Launch()
    {
        _startTime = _useRealTime ? Time.realtimeSinceStartup:Time.fixedTime;
        _working = true;
    }

}
