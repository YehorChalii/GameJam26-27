using UnityEngine;

public class G_BehaviorController : MonoBehaviour
{
    private G_PathFollowController _pathFollowController;
    private G_MovementController _movementController;
    private G_AlertController _alertController;

    private void Awake()
    {
        _pathFollowController = GetComponent<G_PathFollowController>();
        _movementController = GetComponent<G_MovementController>();
        _alertController = GetComponent<G_AlertController>();
    }

    private void OnEnable()
    {
        _alertController.OnPlayerSpotted += HandlePlayerSpotted;
    }

    private void OnDisable()
    {
        _alertController.OnPlayerSpotted -= HandlePlayerSpotted;
    }

    private void Update()
    {
        _movementController.SetMovementTarget(_pathFollowController.FollowPosition);
    }

    public void OnPlayerEnter(P_Controller player)
    {
        _pathFollowController.Stop = true;
        _movementController.SetLookTarget(player.transform);

        _alertController.PlayerDetected = true;
    }

    public void OnPlayerExit()
    {
        _pathFollowController.Stop = false;
        _movementController.ClearLookTarget();

        _alertController.PlayerDetected = false;
    }

    private void HandlePlayerSpotted()
    {
        GameEventsBus.RaisePlayerSpotted();

        _pathFollowController.Stop = true;

        _pathFollowController.enabled = false;
        _movementController.enabled = false;
        _alertController.enabled = false;
        this.enabled = false;
    }
}
