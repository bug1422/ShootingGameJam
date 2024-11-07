using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponType
{
    Shotgun,Rifle,Pistol
}
public abstract class Weapon
{
    protected WeaponType Type;
    public GunData data;
    public Weapon() { }

    public virtual void Shoot() { }
}
public class Pistol : Weapon
{
    public Pistol(PistolData data) { Type = WeaponType.Pistol; this.data = data; }
}

public class Shotgun : Weapon
{
    public Shotgun(ShotgunData data) { Type = WeaponType.Shotgun; this.data = data; }
}

public class Rifle : Weapon
{
    public Rifle(RifleData data) { Type = WeaponType.Rifle; this.data = data; }
}