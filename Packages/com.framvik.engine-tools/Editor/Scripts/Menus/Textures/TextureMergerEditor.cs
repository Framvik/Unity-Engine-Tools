using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEditor;
using Framvik.EngineTools.Textures;

namespace Framvik.EditorTools.Menus
{
    /// <summary>
    /// Early editor window for advanced TextureMerger UI tools.
    /// TODO: Fixup and comment.
    /// </summary>
    public class TextureMergerEditor : EditorWindow
    {
        private readonly List<Texture2D> m_Textures = new ();

        private int m_ChannelIndexR = 0;
        private int m_ChannelIndexG = 0;
        private int m_ChannelIndexB = 0;
        private int m_ChannelIndexA = 1;

        private bool m_InvertChannelR = false;
        private bool m_InvertChannelG = false;
        private bool m_InvertChannelB = false;
        private bool m_InvertChannelA = true;

        private Texture2D m_RGBTexture = null;
        private Texture2D m_AlphaTexture = null;
        private bool m_InvertAlpha = true;
        private bool m_GrayscaleAlpha = true;

        private Texture2D m_TextureA = null;
        private Texture2D m_TextureB = null;
        private Texture2D m_TextureMask = null;

        private Texture2D m_MergedTexture = null;

        private int m_TabIndex = 0;
        private string m_DefaultDirectory = "";
        private string m_DefaultName = "";
        private SaveTextureFileFormat m_SaveFormat;

        [MenuItem(MenuPathsConstants.MAIN_MENU_WINDOWS+ "Open Texture Merger Window")]
        public static void ShowWindow()
        {
            GetWindow<TextureMergerEditor>("Texture Merger").Show();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Mask Merge"))
                m_TabIndex = 0;
            if (GUILayout.Button("Alpha Merge"))
                m_TabIndex = 1;
            if (GUILayout.Button("RGBA Merge"))
                m_TabIndex = 2;
            EditorGUILayout.EndHorizontal();

            if (m_TabIndex == 0)
                DrawMaskGUI();
            else if (m_TabIndex == 1)
                DrawAlphaGUI();
            else if (m_TabIndex == 2)
                DrawRGBAGUI();

            if (m_MergedTexture != null)
            {
                if (GUILayout.Button("Export Texture"))
                {
                    var path = EditorUtility.SaveFilePanel("Select save file location", m_DefaultDirectory, m_DefaultName, 
                        TextureToFileUtility.SaveTextureFileFormatToFileExt(m_SaveFormat));
                    TextureToFileUtility.SaveTexture2DToFile(m_MergedTexture, path, m_SaveFormat);
                }
                var r = GUILayoutUtility.GetLastRect();
                var w = this.position.width / 2;
                EditorGUI.DrawPreviewTexture(new Rect(r.position.x, r.position.y + r.height, w, w), m_MergedTexture);
                EditorGUI.DrawTextureAlpha(new Rect(r.position.x + w, r.position.y + r.height, w, w), m_MergedTexture);
            }
        }

        private void DrawMaskGUI()
        {
            m_TextureA = (Texture2D)EditorGUILayout.ObjectField("Texture A ", m_TextureA, typeof(Texture2D), false);
            m_TextureB = (Texture2D)EditorGUILayout.ObjectField("Texture B ", m_TextureB, typeof(Texture2D), false);
            m_TextureMask = (Texture2D)EditorGUILayout.ObjectField("Texture Mask ", m_TextureMask, typeof(Texture2D), false);

            if (MaskTexturesValid())
            {
                if (GUILayout.Button("Merge Textures"))
                {
                    MergeTexturesMask();
                }
            }
            else
            {
                GUILayout.Label("Make sure all textures are chosen!");
            }
        }

        private bool MaskTexturesValid()
        {
            return m_TextureA != null && m_TextureB != null && m_TextureMask != null;
        }

        private void MergeTexturesMask()
        {
            var textureChannels = (int)GraphicsFormatUtility.GetComponentCount(m_TextureA.graphicsFormat);
            Debug.Log((int)GraphicsFormatUtility.GetComponentCount(m_TextureA.graphicsFormat));
            Debug.Log((int)GraphicsFormatUtility.GetComponentCount(m_TextureB.graphicsFormat));
            Debug.Log((int)GraphicsFormatUtility.GetComponentCount(m_TextureMask.graphicsFormat));
            m_MergedTexture = TextureMerger.MergeTexturesByMask(m_TextureA, m_TextureB, m_TextureMask, textureChannels <= 3);
            m_DefaultDirectory = AssetDatabase.GetAssetPath(m_TextureA);
            m_DefaultName = m_TextureA.name;
            m_SaveFormat = textureChannels <= 3 ? SaveTextureFileFormat.JPG : SaveTextureFileFormat.PNG;
        }

        private void DrawAlphaGUI()
        {
            m_RGBTexture = (Texture2D)EditorGUILayout.ObjectField("RGB Texture ", m_RGBTexture, typeof(Texture2D), false);
            m_AlphaTexture = (Texture2D)EditorGUILayout.ObjectField("Alpha Texture", m_AlphaTexture, typeof(Texture2D), false);
            m_InvertAlpha = EditorGUILayout.Toggle("Invert Alpha", m_InvertAlpha);
            m_GrayscaleAlpha = EditorGUILayout.Toggle("Grayscale Alpha Texture", m_GrayscaleAlpha);

            if (AlphaTexturesValid())
            {
                if (GUILayout.Button("Merge Texture"))
                {
                    MergeTexturesAlpha();
                }
            }
            else
            {
                GUILayout.Label("Make sure all textures are chosen!");
            }
        }

        private bool AlphaTexturesValid()
        {
            return m_RGBTexture != null && m_AlphaTexture != null;
        }

        private void MergeTexturesAlpha()
        {
            m_MergedTexture = m_GrayscaleAlpha ?
                TextureMerger.MergeTexturesAlphaGrayscale(m_RGBTexture, m_AlphaTexture, m_InvertAlpha) :
                TextureMerger.MergeTexturesAlpha(m_RGBTexture, m_AlphaTexture, m_InvertAlpha);
        }

        private void DrawRGBAGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < m_Textures.Count; i++)
            {
                m_Textures[i] = (Texture2D)EditorGUILayout.ObjectField("Texture " + i, m_Textures[i], typeof(Texture2D), false);
            }
            if (GUILayout.Button("Add Texture"))
            {
                m_Textures.Add(null);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUIHelper.DrawCompactLabel("R Index");
            m_ChannelIndexR = EditorGUILayout.IntSlider(m_ChannelIndexR, 0, m_Textures.Count-1);
            m_InvertChannelR = EditorGUILayout.ToggleLeft("Invert", m_InvertChannelR);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUIHelper.DrawCompactLabel("G Index");
            m_ChannelIndexG = EditorGUILayout.IntSlider(m_ChannelIndexG, 0, m_Textures.Count-1);
            m_InvertChannelG = EditorGUILayout.ToggleLeft("Invert", m_InvertChannelG);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUIHelper.DrawCompactLabel("B Index");
            m_ChannelIndexB = EditorGUILayout.IntSlider(m_ChannelIndexB, 0, m_Textures.Count - 1);
            m_InvertChannelB = EditorGUILayout.ToggleLeft("Invert", m_InvertChannelB);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUIHelper.DrawCompactLabel("A Index");
            m_ChannelIndexA = EditorGUILayout.IntSlider(m_ChannelIndexA, 0, m_Textures.Count - 1);
            m_InvertChannelA = EditorGUILayout.ToggleLeft("Invert", m_InvertChannelA);
            EditorGUILayout.EndHorizontal();
            
            if (RGBATexturesValid())
            {
                if (GUILayout.Button("Merge Texture"))
                {
                    MergeTexturesRGBA();
                }
            }
            else
            {
                GUILayout.Label("Make sure all used textures are chosen!");
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();


        }

        private bool RGBATexturesValid()
        {
            if (m_Textures.Count <= 0)
                return false;
            if (m_ChannelIndexA < 0 || m_ChannelIndexA >= m_Textures.Count || m_Textures[m_ChannelIndexA] == null)
                return false;
            if (m_ChannelIndexR < 0 || m_ChannelIndexR >= m_Textures.Count || m_Textures[m_ChannelIndexR] == null)
                return false;
            if (m_ChannelIndexG < 0 || m_ChannelIndexG >= m_Textures.Count || m_Textures[m_ChannelIndexG] == null)
                return false;
            if (m_ChannelIndexB < 0 || m_ChannelIndexB >= m_Textures.Count || m_Textures[m_ChannelIndexB] == null)
                return false;
            return true;
        }

        private void MergeTexturesRGBA()
        {
            m_MergedTexture = TextureMerger.MergeTexturesRGBA(new TextureMerger.TextureMergeSettingsRGBA
            {
                RedChannel = m_Textures[m_ChannelIndexR],
                GreenChannel = m_Textures[m_ChannelIndexG],
                BlueChannel = m_Textures[m_ChannelIndexB],
                AlphaChannel = m_Textures[m_ChannelIndexA],
                InvertRed = m_InvertChannelR,
                InvertGreen = m_InvertChannelG,
                InvertBlue = m_InvertChannelB,
                InvertAlpha = m_InvertChannelA,
            });
        }
    }
}