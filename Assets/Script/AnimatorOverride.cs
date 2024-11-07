using UnityEngine;
public class AnimatorOverride : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void SetAnimation(AnimatorOverrideController animatorOverrideController)
    {
        _animator.runtimeAnimatorController = animatorOverrideController;
    }
}
