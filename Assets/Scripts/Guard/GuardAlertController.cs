using System;
using UnityEngine;

public class GuardAlertController : MonoBehaviour
{
    public event Action<float> OnAlertLevelChanged;

    [Header("Alert")]
    [SerializeField] private float maxAlertTime;
    [SerializeField] private float alertAcceleration;
    [SerializeField] private float alertDeceleration;

    private float _currentAlertTime = 0f;
    private float _currentAlertRate = 0f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    private Transform _playerTransform;

    public void SetPlayer(PlayerController player)
    {        
        if(player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            _playerTransform = null;
        }
    }

    public void UpdateLogic(float dt)
    {
        HandleAlertLevel(dt);
        HandleLookAtPlayer(dt);

        float normalizedAlertLevel = _currentAlertTime / maxAlertTime;
        OnAlertLevelChanged?.Invoke(normalizedAlertLevel);
    }

    void HandleAlertLevel(float dt)
    {
        bool playerDetected = _playerTransform != null;

        float targetAlertLevel = playerDetected ? 1f : -1f;
        float alertRate = playerDetected ? alertAcceleration : alertDeceleration;

        _currentAlertRate = Mathf.MoveTowards(_currentAlertRate, targetAlertLevel, alertRate * dt);
        _currentAlertTime = Mathf.Clamp(_currentAlertTime + _currentAlertRate * dt, 0f, maxAlertTime);

        if (_playerTransform != null && _currentAlertTime >= maxAlertTime)
        {
            Destroy(_playerTransform.gameObject);
            _playerTransform = null;

            _currentAlertTime = 0f;
            _currentAlertRate = 0f;
        }
    }

    void HandleLookAtPlayer(float dt)
    {
        if (_playerTransform == null) return;

        Vector3 directionToPlayer = _playerTransform.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * dt);
        }
    }
}