// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DontDestroy.cs" company="Temporalis">
//    Copyright (c) 2018, Lars-Kristian Svenøy. All rights reserved
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Utilities
{
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///     Keeps gameObjects this is attached to persistent throughout scenes
    /// </summary>
    public class DontDestroy : MonoBehaviour
    {
        public bool DestroyOriginal;

        public bool IsUnique;

        public void SetDoNotDestroy()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Awake()
        {
            if (IsUnique)
            {
                var gameObjects = FindObjectsOfType<GameObject>().Where(x => x.name == gameObject.name).ToArray();
                if (gameObjects.Length > 1)
                    if (!DestroyOriginal)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    else
                    {
                        for (var i = 0; i < gameObjects.Length; i++)
                        {
                            var otherObj = gameObjects[i];
                            if (gameObject != otherObj) Destroy(otherObj);
                        }
                    }
            }

            DontDestroyOnLoad(gameObject);
        }
    }
}