using System;
using UnityEngine;

namespace HBUnityGameCore
{
    [Serializable]
    [CreateAssetMenu(fileName = "Entity Class", menuName = "HBUnityGameCore//EntityClass")]
    public class EntityClass : BaseObjectScriptableObject
    {
        SerializableDictionary<string, Skill> _skills;
    }

}
