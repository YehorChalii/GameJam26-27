using UnityEngine;

public class NPC_AnimationController : MonoBehaviour
{
    private enum NPCAnimation
    {
        Dead,
        Eaten,
        Sitting,
        Biting,
        LookAround
    }

    [SerializeField] private NPCAnimation animation;
    [SerializeField] private string deadAnimBool;
    [SerializeField] private string eatenAnimBool;
    [SerializeField] private string sittingAnimBool;
    [SerializeField] private string bitingAnimBool;
    [SerializeField] private string lookAroundAnimBool;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        switch(animation)
        {
            case NPCAnimation.Dead:
                SetAnimation(deadAnimBool);
                break;
            case NPCAnimation.Eaten:
                SetAnimation(eatenAnimBool);
                break;
            case NPCAnimation.Sitting:
                SetAnimation(sittingAnimBool);
                break;
            case NPCAnimation.Biting:
                SetAnimation(bitingAnimBool);
                break;
            case NPCAnimation.LookAround:
                SetAnimation(lookAroundAnimBool);
                break;
        }
    }
    private void SetAnimation(string animationBoolName)
    {
        _animator.SetBool(animationBoolName, true);
    }
}
