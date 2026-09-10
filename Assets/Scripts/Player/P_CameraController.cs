using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerForwardTransform;
    [SerializeField] private float movementSmoothing;

    private Vector3 _positionOffset;
    private Vector3 _velocity;

    private void Awake()
    {
        SetPositionOffset();
    }

    void SetPositionOffset()
    {
        _positionOffset = transform.position - playerForwardTransform.position;
    }

    private void LateUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (playerForwardTransform == null) return;
        transform.position = Vector3.SmoothDamp(transform.position, playerForwardTransform.position + _positionOffset, ref _velocity, movementSmoothing);
    }
}
