// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonoSingletonCreateIfNull.cs" company="Temporalis">
//    Copyright (c) 2018, Lars-Kristian Svenøy. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Utilities
{
    using UnityEngine;

    /// <summary>
    ///     Any class inheriting MonoSingletonCreateIfNull will automatically become a MonoBehaviour Singleton,
    ///     and will be automatically instantiated if it does not currently exit.
    ///     WARNING: Before you inherit from this class, PLEASE consider the possible memory/reference leak situation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingletonCreateIfNull<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        protected static T instance;

        /**
       Returns the instance of this singleton.
    */
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (instance == null)
                    {
                        var instanceObj = new GameObject(typeof(T).Name, typeof(T));
                        instance = instanceObj.GetComponent<T>();
                    }
                }

                return instance;
            }
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}