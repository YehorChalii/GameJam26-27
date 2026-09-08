using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float minWalkingSpeed;
    [SerializeField] private float animationChangeSpeed;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator.speed = 0f;
    }

    private void Update()
    {
        Vector3 velocity = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
        float currentSpeed = velocity.magnitude;

        float targetSpeed = currentSpeed > minWalkingSpeed ? 1f : 0f;
        animator.speed = Mathf.MoveTowards(animator.speed, targetSpeed, animationChangeSpeed * Time.deltaTime);
    }
}
