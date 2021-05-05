using System;
using JetBrains.Annotations;
using UnityEngine;


namespace Veganimus.NovaStar
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    [CreateAssetMenu(menuName =("Specialized Events/ Pool GameObject Request"))]
    public class PoolGORequest : ScriptableObject
    {
        public Func<GameObject> OnGameObjectRequested;
        public Func<int, GameObject> OnGameObjectIntRequested;
        public GameObject RequestGameObject()
        {
            if (OnGameObjectRequested != null)
            {
                GameObject obj = OnGameObjectRequested.Invoke();
                return obj;
            }
            else
                return null;
        }
        public GameObject RequestGameObjectInt(int parameter)
        {
            if (OnGameObjectIntRequested != null)
            {
                GameObject obj = OnGameObjectIntRequested.Invoke(parameter);
                return obj;
            }
            else
                return null;
        }
    }
}
