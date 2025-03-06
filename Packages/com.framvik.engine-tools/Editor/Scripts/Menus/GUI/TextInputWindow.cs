using UnityEngine;
using UnityEditor;
using System;

namespace Framvik.EditorTools.Menus
{
    /// <summary>
    /// An editor window that lets the user accept or cancel.
    /// </summary>
    public class TextInputWindow : EditorWindow
    {
        string m_Message;
        string m_Text;
        Action<string> m_OnAccept;
        Action m_OnCancel;

        public static void ShowWindow(string title, string message, Action<string> onAccept, Action onCancel = null)
        {
            var win = GetWindow<TextInputWindow>(true, title, true);
            win.m_Message = message;
            win.m_OnAccept = onAccept;
            win.m_OnCancel = onCancel;
            win.ShowModalUtility();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField(m_Message);
            m_Text = EditorGUILayout.TextField(m_Text);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Accept"))
                Accept();
            if (GUILayout.Button("Cancel"))
                Cancel();
            EditorGUILayout.EndHorizontal();
        }

        private void Cancel()
        {
            m_OnCancel?.Invoke();
            this.Close();
        }

        private void Accept()
        {
            m_OnAccept?.Invoke(m_Text);
            this.Close();
        }
    }
}
