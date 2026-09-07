using UnityEngine;

public class GuardViewController : MonoBehaviour
{
    [SerializeField] private GuardAlertBarUI alertUI;

    private GuardAlertController _alertController;

    private void Awake()
    {
        _alertController = GetComponent<GuardAlertController>();
    }

    private void OnEnable()
    {
        _alertController.OnAlertLevelChanged += UpdateAlertVisuals;
    }

    private void OnDisable()
    {
        _alertController.OnAlertLevelChanged -= UpdateAlertVisuals;
    }

    void UpdateAlertVisuals(float normalizedAlertLevel)
    {
        alertUI.UpdateAlertBar(normalizedAlertLevel);
    }
}
