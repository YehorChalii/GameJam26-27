using UnityEngine;

public class GuardBehaviorController : MonoBehaviour
{
    private enum GuardState
    {
        Patrol,
        Alert
    }

    private GuardState _guardState = GuardState.Patrol;

    private GuardMovementController _movementController;
    private GuardAlertController _alertController;
    private GuardViewController _viewController;

    private void Awake()
    {
        _movementController = GetComponent<GuardMovementController>();
        _alertController = GetComponent<GuardAlertController>();
        _viewController = GetComponent<GuardViewController>();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        _alertController.UpdateLogic(dt);

        switch (_guardState) 
        {
            case GuardState.Patrol:
                _movementController.UpdateLogic(dt);
                break;
            case GuardState.Alert:
                break;
        }
    }

    public void OnPlayerEnter(PlayerController player)
    {
        _guardState = GuardState.Alert;
        _alertController.SetPlayer(player);
    }

    public void OnPlayerExit()
    {
        _guardState = GuardState.Patrol;
        _alertController.SetPlayer(null);
    }
}
