using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manage the GameOver animation
/// </summary>
public class GameOver : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] DragMecaniq _gameplay;

    [Header("External references")]
    [SerializeField] CanvasGroup _fade;
    [SerializeField] AudioSource _musicSource;

    private void Awake()
    {
        _gameplay.OnGameOver += LaunchGameOver;
    }

    /// <summary>
    /// The Game Over routine
    /// </summary>
    internal void LaunchGameOver()
    {
        StartCoroutine(GameOverRoutine());
        IEnumerator GameOverRoutine()
        {
            // Fade to black & Music
            _fade.gameObject.SetActive(true);
            var initialVolume = _musicSource.volume;
            SpecialCountDown scd = new SpecialCountDown(1);
            while(!scd.isDone)
            {
                _musicSource.volume = initialVolume - scd.Progress;
                _fade.alpha = scd.Progress;
                yield return null;
            }

            SceneManager.LoadScene(0);

            yield break;
        }

    }


}
