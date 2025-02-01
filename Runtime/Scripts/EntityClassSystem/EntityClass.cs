using System;
using System.Collections.Generic;
using UnityEngine;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Entity Class", menuName = "HBUnityGameCore/EntityClass")]
    public class EntityClass : BaseObjectScriptableObject
    {
        // Similar to dungeon & dragons classes
        [Header("Abilities")]
        [SerializeField, Tooltip("These are the basic skills that the class can use.")]
        List<Skill> _skills;

        [SerializeField, Tooltip("The spells that the class can use.")]
        List<Spell> _spells;
    }
}
