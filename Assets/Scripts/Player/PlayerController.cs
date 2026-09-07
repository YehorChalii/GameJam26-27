using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxMovementSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed;

    [Header("Animation")]
    [SerializeField] private float minWalkingSpeed;

    private CharacterController _characterController;
    private Animator _animator;

    private Vector2 _inputVector;
    private Vector3 _currentVelocity;

    private InputActions _inputActions;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _inputActions = new InputActions();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        _inputActions.Player.Move.performed += HandleInputVector;
        _inputActions.Player.Move.canceled += HandleInputVector;
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= HandleInputVector;
        _inputActions.Player.Move.canceled -= HandleInputVector;
        _inputActions.Disable();
    }

    private void HandleInputVector(InputAction.CallbackContext ctx)
    {
        _inputVector = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();

        HandleAnimations();
    }

    void HandleMovement()
    {
        Vector3 targetDirection = new Vector3(_inputVector.x, 0f, _inputVector.y).normalized;
        Vector3 targetVelocity = targetDirection * maxMovementSpeed;

        float rate = _inputVector.sqrMagnitude > 0.01f ? acceleration : deceleration;

        _currentVelocity = Vector3.MoveTowards(_currentVelocity, targetVelocity, rate * Time.deltaTime);

        _characterController.Move(_currentVelocity * Time.deltaTime);
    }

    void HandleRotation()
    {
        Vector3 horizontalVelocity = new Vector3(_currentVelocity.x, 0f, _currentVelocity.z);

        if (horizontalVelocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleAnimations()
    {
        Vector3 horizontalVelocity = new Vector3(_currentVelocity.x, 0f, _currentVelocity.z);
        float currentSpeed = horizontalVelocity.magnitude;

        _animator.speed = currentSpeed > minWalkingSpeed ? 1f : 0f;
    }
}