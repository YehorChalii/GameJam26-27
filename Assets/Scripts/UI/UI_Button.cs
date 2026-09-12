using System.Collections;
using UnityEngine;

public class UI_Button : MonoBehaviour
{
    [SerializeField] private float scaleMultiplier;
    [SerializeField] private float scaleTime;

    private Vector3 _originalScale;
    private Coroutine _beepCoroutine;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void Beep()
    {
        if (_beepCoroutine != null)
        {
            StopCoroutine(_beepCoroutine);
        }

        _beepCoroutine = StartCoroutine(BeepRoutine());
    }

    private IEnumerator BeepRoutine()
    {
        Vector3 targetScale = _originalScale * scaleMultiplier;

        yield return ScaleTo(_originalScale, targetScale);
        yield return ScaleTo(targetScale, _originalScale);

        _beepCoroutine = null;
    }

    private IEnumerator ScaleTo(Vector3 startScale, Vector3 targetScale)
    {
        float timer = 0f;

        while (timer < scaleTime)
        {
            timer += Time.deltaTime;
            float t = timer / scaleTime;

            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }

        transform.localScale = targetScale;
    }
}