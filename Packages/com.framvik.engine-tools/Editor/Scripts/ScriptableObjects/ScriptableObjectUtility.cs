using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framvik.EditorTools
{
    /// <summary>
    /// Generic utility class for ScriptableObjects.
    /// </summary>
    public static class ScriptableObjectUtility
    {
        /// <summary>
        /// Attempts to get all ScriptableObject assets instances of a given type.
        /// </summary>
        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }
            return a;
        }
    }
}

