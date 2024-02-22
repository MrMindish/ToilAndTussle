using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AB
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "AttackData/Attack", order = 1)]
    public class SpawnManagerScriptableObject : ScriptableObject
    {
        public string moveName;
        public string moveDescription;

        public bool isLight;
        public bool isHeavy;
        public bool isSpecial;

        public int damage;

        public string moveExecution;


    }
}