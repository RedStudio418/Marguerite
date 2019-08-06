using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Button _nextButton;
    [SerializeField] Text _textContent;

    [Header("Animations")]
    [SerializeField] Animation _dialogueAnimation;
    [SerializeField] AnimationClip _openDialogue;
    [SerializeField] AnimationClip _closeDialogue;

    [SerializeField] AnimationClip _textFadeIn;
    [SerializeField] AnimationClip _textFadeOut;

    private void Awake()
    {
        // UI Setup
        _textContent.GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().alpha = 0;
        _nextButton.interactable = false;
    }

    public void PlayDialogue(Dialogue d) => StartCoroutine(DialogueAnimation(d));

    IEnumerator DialogueAnimation(Dialogue d)
    {
        // Prepare UI
        gameObject.SetActive(true);

        // Button Setup
        bool _next = false;
        void Next() => _next = true;
        _nextButton.onClick.AddListener(Next);
        _nextButton.interactable = false;

        // Launch Fade In animation
        yield return _dialogueAnimation.PlayAndWait(_openDialogue);
        _nextButton.interactable = true;
        
        // Show Text (with fade in & fade out animations)
        var dial = d.Dialog;
        foreach(var el in dial)
        {
            _textContent.text = el;
            yield return _dialogueAnimation.PlayAndWait(_textFadeIn);

            _next = false;
            yield return new WaitWhile(() => !_next);

            yield return _dialogueAnimation.PlayAndWait(_textFadeOut);
        }

        // Clean up
        _nextButton.interactable = false;
        _nextButton.onClick.RemoveListener(Next);

        // Fade Out animation
        yield return _dialogueAnimation.PlayAndWait(_closeDialogue);
        gameObject.SetActive(false);

        yield break;
    }
}
