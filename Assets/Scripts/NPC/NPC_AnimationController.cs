using UnityEngine;

public class NPC_AnimationController : MonoBehaviour
{
    private enum NPCAnimation
    {
        Dead,
        Sitting
    }

    [SerializeField] private NPCAnimation animation;
    [SerializeField] private string deadAnimBool;
    [SerializeField] private string sittingAnimBool;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        switch(animation)
        {
            case NPCAnimation.Dead:
                SetAnimation(deadAnimBool);
                break;
            case NPCAnimation.Sitting:
                SetAnimation(sittingAnimBool);
                break;
        }
    }
    private void SetAnimation(string animationBoolName)
    {
        _animator.SetBool(animationBoolName, true);
    }
}
