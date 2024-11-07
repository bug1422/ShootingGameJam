using UnityEngine;
public class SetWeapon : MonoBehaviour
{
    [SerializeField] private AnimatorOverrideController[] overrideControllers;
    [SerializeField] private AnimatorOverride overrider;
    public void Set(int value)
    {
        overrider.SetAnimation(overrideControllers[value]);
    }
    
}
