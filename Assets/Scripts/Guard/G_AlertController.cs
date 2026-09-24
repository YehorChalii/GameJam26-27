using System;
using UnityEngine;

public class G_AlertController : MonoBehaviour
{
    public Action<float> OnAlertLevelChanged;
    public Action OnPlayerSpotted;

    [Header("Alert")]
    [SerializeField] private float maxAlertTime;
    [SerializeField] private float alertAcceleration;
    [SerializeField] private float alertDeceleration;

    private float _currentAlertTime = 0f;
    private float _currentAlertRate = 0f;

    private bool _playerDetected;

    private G_DetectionController _detectionController;

    private void Awake()
    {
        _detectionController = GetComponentInChildren<G_DetectionController>();
    }

    private void OnEnable()
    {
        _detectionController.OnPlayerDetected += HandlePlayerDetected;
        _detectionController.OnPlayerLost += HandlePlayerLost;
    }

    private void OnDisable()
    {
        _detectionController.OnPlayerDetected -= HandlePlayerDetected;
        _detectionController.OnPlayerLost -= HandlePlayerLost;
    }

    private void HandlePlayerDetected(Transform playerTransform) => _playerDetected = true;
    private void HandlePlayerLost()=> _playerDetected = false;

    private void Update()
    {
        float dt = Time.deltaTime;

        HandleAlertLevel(dt);

        float normalizedAlertLevel = _currentAlertTime / maxAlertTime;
        OnAlertLevelChanged?.Invoke(normalizedAlertLevel);
    }

    void HandleAlertLevel(float dt)
    {
        float targetAlertLevel = _playerDetected ? 1f : -1f;
        float alertRate = _playerDetected ? alertAcceleration : alertDeceleration;

        _currentAlertRate = Mathf.MoveTowards(_currentAlertRate, targetAlertLevel, alertRate * dt);
        _currentAlertTime = Mathf.Clamp(_currentAlertTime + _currentAlertRate * dt, 0f, maxAlertTime);

        if (_playerDetected && _currentAlertTime >= maxAlertTime)
        {
            GameEventsBus.RaisePlayerSpotted();
            OnPlayerSpotted?.Invoke();

            _currentAlertTime = maxAlertTime;
            _currentAlertRate = 0f;

            this.enabled = false;
        }
    }
}