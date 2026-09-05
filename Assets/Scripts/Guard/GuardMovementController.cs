using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GuardMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    [Header("Waypoints")]
    [SerializeField] private WaypointsPath waypointsPath;
    [SerializeField] private float waypointReachThreshold;
    [SerializeField] private float waypointDecelerationDistance;
    [SerializeField] private float waitTimeAtEnd;

    private List<Transform> _waypoints;
    private CharacterController _characterController;

    private int _currentWaypointIndex = 0;
    private int _direction = 1;
    private float _waitTimer = 0f;

    private Vector3 _currentVelocity;
    private float _currentSpeed;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        _waypoints = waypointsPath.Waypoints.ToList();
    }

    public void UpdateLogic(float dt)
    {
        if (_waypoints == null || _waypoints.Count == 0) return;

        if (_waitTimer > 0f)
        {
            _waitTimer -= dt;
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, 0f, deceleration * dt);
            _currentVelocity = transform.forward * _currentSpeed;
            _characterController.Move(_currentVelocity * dt);
            return;
        }

        Transform targetWaypoint = _waypoints[_currentWaypointIndex];
        Vector3 directionToTarget = targetWaypoint.position - transform.position;
        directionToTarget.y = 0f;

        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget <= waypointReachThreshold)
        {
            AdvanceToNextWaypoint();
            return;
        }

        HandleRotation(dt, directionToTarget);
        HandleMovement(dt, directionToTarget, distanceToTarget);
    }

    private void HandleRotation(float dt, Vector3 directionToTarget)
    {
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * dt);
        }
    }

    private void HandleMovement(float dt, Vector3 directionToTarget, float distanceToTarget)
    {
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        float turnSpeedMultiplier = Mathf.Clamp01(1f - (angleToTarget / 120f));
        float arrivalMultiplier = Mathf.Clamp01(distanceToTarget / waypointDecelerationDistance);

        float targetSpeed = maxMovementSpeed * turnSpeedMultiplier * arrivalMultiplier;

        float accelRate = (_currentSpeed < targetSpeed) ? acceleration : deceleration;
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, accelRate * dt);
        _currentVelocity = transform.forward * _currentSpeed;

        _characterController.Move(_currentVelocity * dt);
    }

    private void AdvanceToNextWaypoint()
    {
        if (_waypoints.Count <= 1) return;

        int nextIndex = _currentWaypointIndex + _direction;

        if (nextIndex >= _waypoints.Count || nextIndex < 0)
        {
            _direction *= -1;
            _currentWaypointIndex += _direction;
            _waitTimer = waitTimeAtEnd;
        }
        else
        {
            _currentWaypointIndex = nextIndex;
        }
    }
}