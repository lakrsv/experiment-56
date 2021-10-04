﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonoSingleton.cs" company="Temporalis">
//    Copyright (c) 2018, Lars-Kristian Svenøy. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Utilities
{
    using UnityEngine;

    /// <summary>
    ///     Any class inheriting MonoSingleton will automatically become a MonoBehaviour Singleton
    ///     WARNING: Before you inherit from this class, PLEASE consider the possible memory/reference leak situation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour
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

                    //if (instance == null)
                        //Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
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