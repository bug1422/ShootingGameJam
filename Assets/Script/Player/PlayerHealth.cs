using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerHealth
{
    public static int maxHP = 100;
    static int HP = maxHP;
    public static bool invincible = false;
    public static bool isAlive = true;

    public delegate void InitHealthHUD();
    public static event InitHealthHUD onInit;



    public static void DeduceHealth(int point)
    {
        if (!invincible)
        {
            HP -= point;
            if (HP <= 0)
            {
                isAlive = false;
            }
        }
    }
    public static int GetHealth() => HP;
    public static void AddHealth(int point)
    {
        HP = point;
        HP = HP > maxHP ? maxHP : HP;
    }
    public static void Reset()
    {
        HP = maxHP;
        isAlive = true;
        onInit.Invoke();
    }
}
