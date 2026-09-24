using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class G_MovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float smoothTime;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    private CharacterController _characterController;
    private G_PathFollowController _pathFollowController;
    private G_DetectionController _detectionController;

    private Vector3 _velocity;
    private Vector3 _movementTarget;
    private Transform _lookTarget;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _pathFollowController = GetComponent<G_PathFollowController>();
        _detectionController = GetComponentInChildren<G_DetectionController>();
    }

    private void OnEnable()
    {
        _pathFollowController.OnPathFollowPositionUpdated += HandlePathFollowPositionUpdated;

        _detectionController.OnPlayerDetected += HandlePlayerDetected;
        _detectionController.OnPlayerLost += HandlePlayerLost;
    }

    private void OnDisable()
    {
        _pathFollowController.OnPathFollowPositionUpdated -= HandlePathFollowPositionUpdated;

        _detectionController.OnPlayerDetected -= HandlePlayerDetected;
        _detectionController.OnPlayerLost -= HandlePlayerLost;
    }

    private void HandlePathFollowPositionUpdated(Vector3 pathFollowPosition)
    {
        _movementTarget = pathFollowPosition;
    }

    private void HandlePlayerDetected(Transform playerTransform)
    {
        _lookTarget = playerTransform;
    }

    private void HandlePlayerLost()
    {
        _lookTarget = null;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        HandleMovement(dt);
        HandleRotation(dt);
    }

    private void HandleMovement(float dt)
    {
        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = _movementTarget;

        targetPosition.y = currentPosition.y;

        Vector3 desiredPosition = Vector3.SmoothDamp(currentPosition, targetPosition, ref _velocity, smoothTime, maxMovementSpeed, dt);

        Vector3 movement = desiredPosition - currentPosition;

        _characterController.Move(movement);
    }

    private void HandleRotation(float dt)
    {
        Vector3 direction;

        if (_lookTarget != null)
        {
            direction = _lookTarget.position - transform.position;
        }
        else
        {
            direction = _velocity;
        }

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * dt);
    }
}