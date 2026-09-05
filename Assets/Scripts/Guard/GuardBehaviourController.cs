using UnityEngine;

public class GuardBehaviorController : MonoBehaviour
{
    private enum GuardState
    {
        Patrolling,
        Alerted
    }

    private GuardState _guardState = GuardState.Patrolling;

    [Header("Alert")]
    [SerializeField] private float maxAlertTime;
    [SerializeField] private float alertAcceleration;
    [SerializeField] private float alertDeceleration;

    private float _currentAlertTime = 0f;
    private float _currentAlertRate = 0f;

    private GuardMovementController _movementController;
    private GuardDetectionController _detectionController;
    private GuardAlertController _alertController;

    private PlayerController _player;

    private void Start()
    {
        _movementController = GetComponent<GuardMovementController>();
        _detectionController = GetComponentInChildren<GuardDetectionController>();
        _alertController = GetComponent<GuardAlertController>();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        switch (_guardState) 
        {
            case GuardState.Patrolling:
                _movementController.UpdateLogic(dt);
                break;
            case GuardState.Alerted:
                _alertController.UpdateLogic(dt);
                break;
        }

        HandleAlertLevel(dt);
    }

    void HandleAlertLevel(float dt)
    {
        bool playerInSight = _player != null;

        float targetRate = playerInSight ? 1f : -1f;
        float rateChange = playerInSight ? alertAcceleration : alertDeceleration;

        _currentAlertRate = Mathf.MoveTowards(_currentAlertRate, targetRate, rateChange * dt);

        _currentAlertTime = Mathf.Clamp(_currentAlertTime + _currentAlertRate * dt, 0f, maxAlertTime);

        Debug.Log($"Alert Level: {GetNormalizedAlertLevel()}");

        if (playerInSight && _currentAlertTime >= maxAlertTime)
        {
            Destroy(_player.gameObject);
            _player = null;
            _currentAlertTime = 0f;
            _currentAlertRate = 0f;

            _guardState = GuardState.Patrolling;
        }
    }

    public float GetNormalizedAlertLevel() => _currentAlertTime / maxAlertTime;

    public void OnPlayerEnter(PlayerController player)
    {
        _guardState = GuardState.Alerted;
        _player = player;
        _alertController.SetPlayer(player);
    }

    public void OnPlayerExit()
    {
        _guardState = GuardState.Patrolling;
        _player = null;
    }
}
