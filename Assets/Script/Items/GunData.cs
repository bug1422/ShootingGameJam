using UnityEngine;

public class GunData : ScriptableObject
{
    public string Name;

    public bool UnlimitedAmmo;
    public bool IsRightHand;
    public int Magazine;
    public int TotalAmmo;
    public int Damage;
    public float ReloadTime;
    public float CooldownPerShot;
    public float Price;
    public float BulletSpeed;
    public AnimatorOverrideController BulletAnimation;
}
