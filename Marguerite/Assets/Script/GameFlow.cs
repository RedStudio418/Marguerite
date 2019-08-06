using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The GAME
public class GameFlow : MonoBehaviour
{
    [SerializeField] FallPetal _fallPetal;
    [SerializeField] TextPetal _textPetal;
    [SerializeField] DragMecaniq _gameplay;
    [SerializeField] ShowEnd _ending;
    [SerializeField] GameOver _gameOver;

    [SerializeField, BoxGroup("Musics")] AudioSource _music;
    [SerializeField, BoxGroup("Musics")] AudioSource _endingMusic;

    Coroutine _gameflow;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        _gameflow = StartCoroutine(GameFlow());
        IEnumerator GameFlow()
        {
            yield return _fallPetal.Run();

            float score = 0;
            yield return _gameplay.DragMecanique((s)=> score=s);

            FadeToBadEndingMusic(); // We can move this in EndRoutine logic but didn't had the time

             yield return _ending.EndRoutine(score);

            _gameOver.LaunchGameOver();

            yield break;
        }

    }

    /// <summary>
    /// Fade music only if we have the "pas du tout" ending
    /// </summary>
    void FadeToBadEndingMusic()
    {
        // Manage music changement on BadEnding
        if (_textPetal.CurrentIndex == 4)
        {
            StartCoroutine(MusicFade());
            IEnumerator MusicFade()
            {
                var destVolume = _endingMusic.volume;
                var startVolume = _music.volume;

                _endingMusic.volume = 0;
                _endingMusic.Play();
                SpecialCountDown scd = new SpecialCountDown(1);
                while (!scd.isDone)
                {
                    _music.volume = Mathf.Lerp(startVolume, 0, scd.Progress);
                    _endingMusic.volume = Mathf.Lerp(0, destVolume, scd.Progress);
                    yield return null;
                }

                yield break;
            }
        }
    }




}
