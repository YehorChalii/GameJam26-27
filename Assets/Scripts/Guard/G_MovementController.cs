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

    private Vector3 _velocity;

    private Vector3 _movementTarget;
    private bool _hasMovementTarget;

    private Transform _lookTarget;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        HandleMovement(dt);
        HandleRotation(dt);
    }

    private void HandleMovement(float dt)
    {
        if (!_hasMovementTarget)
        {
            _velocity = Vector3.zero;
            return;
        }

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

    public void SetMovementTarget(Vector3 target)
    {
        _movementTarget = target;
        _hasMovementTarget = true;
    }


    public void SetLookTarget(Transform target)
    {
        _lookTarget = target;
    }

    public void ClearLookTarget()
    {
        _lookTarget = null;
    }
}