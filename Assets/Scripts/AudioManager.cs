using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource secondaryAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip bgAudioClip;

    [Header("Volume")]
    [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float bgVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float secondaryVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 1f;

    private Coroutine _bgFadeCoroutine;
    private Coroutine _secondaryFadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayBackground(AudioClip clip, float fadeTime = 1f)
    {
        if (clip == null || bgAudioSource.clip == clip) return;

        if (_bgFadeCoroutine != null)
        {
            StopCoroutine(_bgFadeCoroutine);
        }

        _bgFadeCoroutine = StartCoroutine(ChangeBackgroundRoutine(clip, fadeTime));
    }

    public void StopBackground(float fadeTime = 1f)
    {
        if (_bgFadeCoroutine != null)
        {
            StopCoroutine(_bgFadeCoroutine);
        }

        _bgFadeCoroutine = StartCoroutine(FadeBackgroundRoutine(0f, fadeTime, true));
    }

    public void ReturnToMainBackground(float fadeTime = 1f)
    {
        if (_bgFadeCoroutine != null)
        {
            StopCoroutine(_bgFadeCoroutine);
        }

        _bgFadeCoroutine = StartCoroutine(ChangeBackgroundRoutine(bgAudioClip, fadeTime));
    }

    private IEnumerator ChangeBackgroundRoutine(AudioClip newClip, float fadeTime)
    {
        if (bgAudioSource.isPlaying)
        {
            yield return FadeAudioSource(bgAudioSource, 0f, fadeTime);
        }

        bgAudioSource.clip = newClip;
        bgAudioSource.loop = true;
        bgAudioSource.Play();

        yield return FadeAudioSource(bgAudioSource, bgVolume, fadeTime);

        _bgFadeCoroutine = null;
    }

    private IEnumerator FadeBackgroundRoutine(float targetVolume, float fadeTime, bool stopAfter)
    {
        yield return FadeAudioSource(bgAudioSource, targetVolume, fadeTime);

        if (stopAfter) bgAudioSource.Stop();

        _bgFadeCoroutine = null;
    }

    public void PlaySecondary(AudioClip clip, float fadeTime = 1f, bool loop = true)
    {
        if (clip == null || secondaryAudioSource.clip == clip) return;

        if (_secondaryFadeCoroutine != null)
        {
            StopCoroutine(_secondaryFadeCoroutine);
        }

        _secondaryFadeCoroutine = StartCoroutine(PlaySecondaryRoutine(clip, fadeTime, loop));
    }

    public void StopSecondary(float fadeTime = 1f)
    {
        if (_secondaryFadeCoroutine != null)
        {
            StopCoroutine(_secondaryFadeCoroutine);
        }

        _secondaryFadeCoroutine = StartCoroutine(StopSecondaryRoutine(fadeTime));
    }

    private IEnumerator PlaySecondaryRoutine(AudioClip clip, float fadeTime, bool loop)
    {
        if (secondaryAudioSource.isPlaying)
        {
            yield return FadeAudioSource(secondaryAudioSource, 0f, fadeTime);
        }

        secondaryAudioSource.clip = clip;
        secondaryAudioSource.loop = loop;
        secondaryAudioSource.Play();

        yield return FadeAudioSource(secondaryAudioSource, secondaryVolume, fadeTime);

        _secondaryFadeCoroutine = null;
    }

    private IEnumerator StopSecondaryRoutine(float fadeTime)
    {
        yield return FadeAudioSource(secondaryAudioSource, 0f, fadeTime);

        secondaryAudioSource.Stop();
        secondaryAudioSource.clip = null;

        _secondaryFadeCoroutine = null;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxAudioSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    private IEnumerator FadeAudioSource(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = source.volume;

        if (duration <= 0f)
        {
            source.volume = targetVolume;
            yield break;
        }

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = timer / duration;

            source.volume = Mathf.Lerp(startVolume, targetVolume * masterVolume, t);

            yield return null;
        }

        source.volume = targetVolume * masterVolume;
    }
}