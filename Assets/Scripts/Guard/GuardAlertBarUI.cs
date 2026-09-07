using UnityEngine;
using UnityEngine.UI;

public class GuardAlertBarUI : MonoBehaviour
{
    private Image _alertBarUI;

    private void Awake()
    {
        _alertBarUI = GetComponent<Image>();
    }

    public void UpdateAlertBar(float normalizedAlertLevel)
    {
        _alertBarUI.fillAmount = normalizedAlertLevel;
    }
}