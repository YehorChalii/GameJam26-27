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

    private void Update()
    {
        _movementController.SetMovementTarget(_pathFollowController.FollowPosition);
    }

    public void OnPlayerEnter(PlayerController player)
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
}
