using System;
using UnityEngine;

public class G_AlertController : MonoBehaviour
{
    public event Action<float> OnAlertLevelChanged;

    [Header("Alert")]
    [SerializeField] private float maxAlertTime;
    [SerializeField] private float alertAcceleration;
    [SerializeField] private float alertDeceleration;

    private float _currentAlertTime = 0f;
    private float _currentAlertRate = 0f;

    [HideInInspector] public bool PlayerDetected;

    private void Update()
    {
        float dt = Time.deltaTime;

        HandleAlertLevel(dt);

        float normalizedAlertLevel = _currentAlertTime / maxAlertTime;
        OnAlertLevelChanged?.Invoke(normalizedAlertLevel);
    }

    void HandleAlertLevel(float dt)
    {
        float targetAlertLevel = PlayerDetected ? 1f : -1f;
        float alertRate = PlayerDetected ? alertAcceleration : alertDeceleration;

        _currentAlertRate = Mathf.MoveTowards(_currentAlertRate, targetAlertLevel, alertRate * dt);
        _currentAlertTime = Mathf.Clamp(_currentAlertTime + _currentAlertRate * dt, 0f, maxAlertTime);

        if (PlayerDetected && _currentAlertTime >= maxAlertTime)
        {
            _currentAlertTime = 0f;
            _currentAlertRate = 0f;
        }
    }
}