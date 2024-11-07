using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunList : MonoBehaviour
{
    static List<WeaponInfo> list = new List<WeaponInfo>();
    static int index = 0;
    public static void setList(List<WeaponInfo> data)
    {
        list = data;
    }
    public static List<WeaponInfo> getList()
    {
        return list;
    }
    public static WeaponInfo getInfo(int pos)
    {
        index = pos;
        return list[pos];
    }
    public static Sprite getSprite()
    {
        return list[index].GunSprite;
    }
}
