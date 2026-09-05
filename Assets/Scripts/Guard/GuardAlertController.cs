using UnityEngine;

public class GuardAlertController : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 10f;

    private Transform _playerTransform;

    public void SetPlayer(PlayerController player)
    {
        if (_playerTransform == null && player != null)
        {
            _playerTransform = player.transform;
        }
    }

    public void UpdateLogic(float dt)
    {
        HandleLookAtPlayer(dt);
    }

    private void HandleLookAtPlayer(float dt)
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