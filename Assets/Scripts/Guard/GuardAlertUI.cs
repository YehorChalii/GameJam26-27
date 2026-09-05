using UnityEngine;
using UnityEngine.UI;

public class GuardAlertUI : MonoBehaviour
{
    [SerializeField] private GuardBehaviorController behaviorController;
    private Image _alertBarUI;

    private void Start()
    {
        _alertBarUI = GetComponent<Image>();
    }

    private void Update()
    {
        _alertBarUI.fillAmount = behaviorController.GetNormalizedAlertLevel();
    }
}