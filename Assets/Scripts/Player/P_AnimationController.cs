using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class P_AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float minWalkingSpeed;
    [SerializeField] private float animationChangeSpeed;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        animator.speed = 0f;
    }

    private void Update()
    {
        Vector3 velocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);
        float currentSpeed = velocity.magnitude;

        float targetSpeed = currentSpeed > minWalkingSpeed ? 1f : 0f;
        animator.speed = Mathf.MoveTowards(animator.speed, targetSpeed, animationChangeSpeed * Time.deltaTime);
    }
}
