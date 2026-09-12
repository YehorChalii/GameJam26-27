using UnityEngine;

[RequireComponent(typeof(Camera))]
public class P_CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float movementSmoothTime;

    private Vector3 _positionOffset;
    private Quaternion _initialRotation;
    private Vector3 _velocity;

    private bool _isFollowingPlayer = true;

    private Vector3 _transitionStartPosition;
    private Quaternion _transitionStartRotation;

    private Vector3 _transitionTargetPosition;
    private Quaternion _transitionTargetRotation;

    private float _transitionTimer;
    private float _transitionTime;
    private bool _isTransitioning;

    private void Awake()
    {
        _positionOffset = transform.position - playerTransform.position;
        _initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        HandleMovement();
    }

    public void SetTarget(Transform target, float transitionTime)
    {
        _transitionStartPosition = transform.position;
        _transitionStartRotation = transform.rotation;

        if (target != null)
        {
            _transitionTargetPosition = target.position;
            _transitionTargetRotation = target.rotation;

            _isFollowingPlayer = false;
        }
        else
        {
            _transitionTargetPosition = playerTransform.position + _positionOffset;
            _transitionTargetRotation = _initialRotation;

            _isFollowingPlayer = true;
        }

        _transitionTimer = 0f;
        _transitionTime = transitionTime;
        _isTransitioning = true;
    }

    private void HandleMovement()
    {
        if (_isTransitioning)
        {
            HandleTransition();
            return;
        }

        if (_isFollowingPlayer)
        {
            HandlePlayerFollow();
            return;
        }
    }

    private void HandlePlayerFollow()
    {
        Vector3 targetPosition = playerTransform.position + _positionOffset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, movementSmoothTime);
        transform.rotation = _initialRotation;
    }

    private void HandleTransition()
    {
        _transitionTimer += Time.deltaTime;

        float t = Mathf.Clamp01(_transitionTimer / _transitionTime);

        transform.position = Vector3.Lerp(_transitionStartPosition, _transitionTargetPosition, t);
        transform.rotation = Quaternion.Slerp(_transitionStartRotation, _transitionTargetRotation, t);

        if (t >= 1f)
        {
            _isTransitioning = false;
        }
    }
}