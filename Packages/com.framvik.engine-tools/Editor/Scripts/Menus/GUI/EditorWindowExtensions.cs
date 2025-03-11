using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Framvik.EditorTools.Menus
{
    public static class EditorWindowExtensions
    {
        public static void DockWindowTo<T>(EditorWindow targetWindow) where T : EditorWindow
        {
            EditorWindow.GetWindow<T>(targetWindow.GetType());
        }

        public static void DockWindowTo<T>(this T window, EditorWindow targetWindow) where T : EditorWindow
        {
            EditorWindow.GetWindow<T>(targetWindow.GetType());
        }

        public static void DockWindowTo<T1, T2>(this T1 window, T2 targetWindow) where T1 : EditorWindow where T2 : EditorWindow
        {
            EditorWindow.GetWindow<T1>(targetWindow);
        }

        public static void DockWindowTo<T1, T2>() where T1 : EditorWindow where T2 : EditorWindow
        {
            EditorWindow.GetWindow<T1>(typeof(T2));
        }
    }
}