using UnityEngine;

public class G_ViewController : MonoBehaviour
{
    [SerializeField] private G_AlertSignifierUI alertSignifier;

    private G_AlertController _alertController;

    private void Awake()
    {
        _alertController = GetComponent<G_AlertController>();
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
        alertSignifier.UpdateAlertBar(normalizedAlertLevel);
    }
}
