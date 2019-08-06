using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Show the right ending animation
/// </summary>
public class ShowEnd : MonoBehaviour
{
    #region InternalType
    [System.Serializable] class End
    {
        public int Value;
        public CanvasGroup CanvasGroup;
        public Dialogue Dialogue;
    }
    #endregion

    [Header("Managers")]
    [SerializeField] DialogueManager _dialogueManager;
    [SerializeField] TextPetal _currentPetal;

    [Header("Internal UI")]
    [SerializeField] CanvasGroup _background;
    [SerializeField] CanvasGroup _credit;
    [SerializeField] Text _score;
    [SerializeField] Button _endingButton;
    [SerializeField] Button _finishButton;

    [Header("Ending Conf")]
    [SerializeField] List<End> _ends;

    public IEnumerator EndRoutine(float score)
    {
        gameObject.SetActive(true);
        _score.text = ((int)(score * 1000)).ToString();

        // Fade to black
        float fadeDuration = 1f;
        SpecialCountDown scd = new SpecialCountDown(fadeDuration);
        while(!scd.isDone)
        {
            _background.alpha = scd.Progress;
            yield return null;
        }

        // Fade right image
        var endPicked = _ends.Where(i=>i.Value == _currentPetal.CurrentIndex).FirstOrDefault() ?? _ends.First();
        float endingDuration = 3f;
        SpecialCountDown scd2 = new SpecialCountDown(endingDuration);
        while (!scd2.isDone)
        {
            endPicked.CanvasGroup.alpha = scd2.Progress;
            yield return null;
        }
        // Show dialogue message
        _dialogueManager.gameObject.SetActive(true);
        _dialogueManager.PlayDialogue(endPicked.Dialogue);

        // Wait click
        var nextEnding = false;
        _endingButton.onClick.AddListener(() => nextEnding = true);
        yield return new WaitWhile(() => !nextEnding);
        _endingButton.onClick.RemoveAllListeners();

        // Show Credi Animation
        _dialogueManager.gameObject.SetActive(false);
        float creditDuration = 1f;
        _credit.blocksRaycasts = true;
        SpecialCountDown scd3 = new SpecialCountDown(creditDuration);
        while (!scd3.isDone)
        {
            _credit.alpha = scd3.Progress;
            yield return null;
        }

        // Wait click
        bool next = false;
        _finishButton.onClick.AddListener(() => next = true);
        yield return new WaitWhile(() => !next);

        yield break;
    }



}
