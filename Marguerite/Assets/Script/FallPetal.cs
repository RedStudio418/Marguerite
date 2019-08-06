using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FallPetal : MonoBehaviour
{
    [Header("Internal UI")]
    [SerializeField] Transform _flowerHeart;

    [Header("Petal movement")]
    [SerializeField] AnimationCurve _transformAnimation;
    [SerializeField] AnimationCurve _opacityAnimation;

    [Header("Petals conf")]
    [SerializeField] Transform[] _petal;

    [Header("UI")]
    [SerializeField] Button _buttonPedal;
    [SerializeField] CanvasGroup _introductionText;

    [Header("Random Generation")]
    [SerializeField] bool _useScriptedGeneration = false;
    [SerializeField] int[] _randomScript;

    Queue<Transform> _currentPetals;
    IEnumerator<int> _randomGeneration;

    public UnityEvent OnPick;
    public event Action OnAllPedalPicked;

    float TimeOfPetalFall => _transformAnimation.keys.Last().time;

    IEnumerable<int> RandomNumbers()
    {
        while(true)
        {
            yield return UnityEngine.Random.Range(1, 3);
        }
    }
    IEnumerable<int> ScriptedNumbers()
    {
        IEnumerator<int> enumerator =  _randomScript.Cast<int>().GetEnumerator();

        while (enumerator.MoveNext())
            yield return enumerator.Current;

        yield break;
    }

    private void Awake()
    {
        // Prepare internal data structure
        _randomGeneration = (_useScriptedGeneration ? ScriptedNumbers() : RandomNumbers()).GetEnumerator();
        var tmp = _petal.ToList();  // Use ToList in order to use Shuffle algo because cannot easily shuffle in Queue or Array
        tmp.Shuffle();
        _currentPetals = new Queue<Transform>(tmp);
    }

    /// <summary>
    /// The FallPetal Routine containing : "Il m'aime" animation and click to fall petal
    /// </summary>
    /// <returns></returns>
    internal IEnumerator Run()
    {
        gameObject.SetActive(true);
        bool finished = false;

        // Animation "Il m'aime ..."
        _introductionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        SpecialCountDown scd = new SpecialCountDown(2f);
        while(!scd.isDone)
        {
            _introductionText.alpha = 1 - scd.Progress;
            yield return null;
        }
        _introductionText.gameObject.SetActive(false);

        // Prepare button Listener
        _buttonPedal.onClick.AddListener(PickPetal);
        OnAllPedalPicked += () => finished = true;

        // Wait for the petal to be empty
        yield return new WaitWhile(() => !finished);

        // Cleanup
        _buttonPedal.onClick.RemoveListener(PickPetal);
        _buttonPedal.gameObject.SetActive(false);
        yield break;
    }

    /// <summary>
    /// Public method to pick a random number of petal
    /// </summary>
    public void PickPetal()
    {
        // Some check
        if (_currentPetals.Count == 0) return;
        if (!_randomGeneration.MoveNext()) return;

        var nbOfPetal = _randomGeneration.Current;
        OnPick?.Invoke();    // In order to launch FX or SFX

        for (var i=0; i<nbOfPetal; i++)
        {
            if(_currentPetals.Count>0)
            {
                Fall(_currentPetals.Dequeue());
            }
        }

        // Trigg empty flower
        if(_currentPetals.Count<=0)
        {
            OnAllPedalPicked?.Invoke();
        }
    }

    /// <summary>
    /// The fall petal animation
    /// </summary>
    /// <param name="petal">Which petal must me animated</param>
    public void Fall(Transform petal)
    {
        StartCoroutine(FallAnimation());
        IEnumerator FallAnimation()
        {
            Vector3 fallDirection = (petal.position - _flowerHeart.position).normalized;
            Vector3 initialPosition = petal.position;
            CanvasGroup opacityController = petal.GetComponent<CanvasGroup>();
            SpecialCountDown scd = new SpecialCountDown(TimeOfPetalFall);
            
            // Control movement and opacity through code
            while(!scd.isDone)
            {
                petal.position = initialPosition + fallDirection * (_transformAnimation.Evaluate(scd.Progress));
                opacityController.alpha =  _opacityAnimation.Evaluate(scd.Progress);

                yield return null;
            }

            petal.gameObject.SetActive(false);
            yield break;
        }

    }

}
