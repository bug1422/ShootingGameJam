using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunListPlaceholder : MonoBehaviour
{
    [SerializeField]
    public List<WeaponInfo> weaponInfos = new List<WeaponInfo>();

    private void Awake()
    {
        if(GunList.getList() != null) GunList.setList(weaponInfos);
    }
}

[Serializable]
public struct WeaponInfo
{
    public string Name;
    public Vector2 BulletPosition;
    public Sprite BulletSprite;
    public Sprite ArmSprite;
    public Sprite GunSprite;
    public GunData GunData;
}