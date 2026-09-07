using UnityEngine;
using UnityEngine.UI;

public class GuardAlertUI : MonoBehaviour
{
    [SerializeField] private GuardAlertController alertController;
    private Image _alertBarUI;

    private void Start()
    {
        _alertBarUI = GetComponent<Image>();
    }

    private void Update()
    {
        _alertBarUI.fillAmount = alertController.GetNormalizedAlertLevel();
    }
}