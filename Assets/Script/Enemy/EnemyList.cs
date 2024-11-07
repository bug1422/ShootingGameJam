using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Enemy
{
    [CreateAssetMenu(fileName = "Enemies", menuName = "ScriptableObjects/EnemyRoundInfo")]
    public class EnemyList : ScriptableObject
    {
        public List<EnemyRoundInfo> enemies;
        public int waves;
        public float duration;
    }

    [Serializable]
    public class EnemyRoundInfo
    {
        public string path;
        public int amount;
    }
}
