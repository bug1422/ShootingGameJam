using UnityEngine;

[CreateAssetMenu(fileName = "Shotgun", menuName = "ScriptableObjects/Shotgun")]
public class ShotgunData : GunData
{
    public float SpreadAngle;
    public int SpreadLine;
}

