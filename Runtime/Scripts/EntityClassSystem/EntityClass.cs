using System;
using System.Collections.Generic;
using UnityEngine;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Entity Class", menuName = "HBUnityGameCore/EntityClass")]
    public class EntityClass : BaseObjectScriptableObject
    {
        [Header("SKILLS")]
        [SerializeField, Tooltip("The skills that the entity can use.")]
        List<Skill> _skills;
    }
}
