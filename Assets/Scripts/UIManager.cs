using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private Image fillPanel;

    private Coroutine _beepCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        fillPanel.enabled = false;
    }

    public void Beep(float waitingTime, float duration)
    {
        if (_beepCoroutine != null)
        {
            StopCoroutine(_beepCoroutine);
        }

        _beepCoroutine = StartCoroutine(BeepRoutine(waitingTime, duration));
    }

    private IEnumerator BeepRoutine(float waitingTime, float duration)
    {
        fillPanel.enabled = false;

        yield return new WaitForSeconds(waitingTime);

        fillPanel.enabled = true;

        yield return new WaitForSeconds(duration);

        fillPanel.enabled = false;

        _beepCoroutine = null;
    }
}