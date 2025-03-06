using System;
using UnityEditor;
using UnityEngine;

namespace Framvik.EditorTools.Menus
{
    /// <summary>
    /// Collection of GUI functionality.
    /// </summary>
    public static class EditorGUIHelper
    {
        /// <summary>
        /// Creates a very compact EditorGUILayout.LabelField.
        /// </summary>
        public static void DrawCompactLabel(string label)
        {
            var l = new GUIContent(label);
            EditorGUILayout.LabelField(label, GUILayout.Width(GUI.skin.label.CalcSize(l).x));
        }

        /// <summary>
        /// Executes some GUI drawing callback with the GUI tinted with a color.
        /// </summary>
        public static void DrawWithColorTint(Color color, Action drawAction)
        {
            var oldC = GUI.color;
            GUI.color = color;
            drawAction();
            GUI.color = oldC;
        }

        /// <summary>
        /// Executes some GUI drawing callback with the GUI background and content tinted with different colors.
        /// </summary>
        public static void DrawWithColorTint(Color contentColor, Color backgroundColor, Action drawAction)
        {
            var oldBC = GUI.backgroundColor;
            var oldCC = GUI.contentColor;
            GUI.backgroundColor = backgroundColor;
            GUI.contentColor = contentColor;
            drawAction();
            GUI.backgroundColor = oldBC;
            GUI.contentColor = oldCC;
        }

        /// <summary>
        /// Executes some GUI drawing callback with the background tinted with a color.
        /// </summary>
        public static void DrawWithBackgroundColorTint(Color color, Action drawAction)
        {
            var oldC = GUI.backgroundColor;
            GUI.backgroundColor = color;
            drawAction();
            GUI.backgroundColor = oldC;
        }

        /// <summary>
        /// Executes some GUI drawing callback with the content tinted with a color.
        /// </summary>
        public static void DrawWithContentColorTint(Color color, Action drawAction)
        {
            var oldC = GUI.contentColor;
            GUI.contentColor = color;
            drawAction();
            GUI.contentColor = oldC;
        }
    }
}
