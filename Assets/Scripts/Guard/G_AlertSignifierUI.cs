using UnityEngine;
using UnityEngine.UI;

public class G_AlertSignifierUI : MonoBehaviour
{
    [SerializeField] private Image alertFill;
    [SerializeField] private Image alertBG;

    bool _playerDetected;

    private void Awake()
    {
        alertFill.fillAmount = 0;
        alertBG.enabled = false;
    }

    public void UpdateAlertBar(float normalizedAlertLevel)
    {
        if(!_playerDetected && normalizedAlertLevel > 0)
        {
            alertBG.enabled = true;
            _playerDetected = true;
        }
        else if(_playerDetected && normalizedAlertLevel <= 0)
        {
            alertBG.enabled = false;
            _playerDetected = false;
        }

        alertFill.fillAmount = normalizedAlertLevel;
    }
}