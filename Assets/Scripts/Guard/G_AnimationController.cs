using System;
using UnityEngine;

public class G_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Walking")]
    [SerializeField] private string isWalkingAnimBool;
    [SerializeField] private float minWalkingSpeed;

    [Header("Alert")]
    [SerializeField] private string playerSpottedAnimBool;

    private CharacterController _characterController;
    private G_AlertController _alertController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
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

    private void HandlePlayerSpotted() => animator.SetBool(playerSpottedAnimBool, true);

    private void Update()
    {
        HandleWalkingMovementAnimation();
    }

    private void HandleWalkingMovementAnimation()
    {
        Vector3 velocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);
        float currentSpeed = velocity.magnitude;

        bool isWalking = currentSpeed > minWalkingSpeed ? true : false;
        animator.SetBool(isWalkingAnimBool, isWalking);
    }
}
