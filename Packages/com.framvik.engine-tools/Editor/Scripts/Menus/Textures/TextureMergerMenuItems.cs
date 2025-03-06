using System.IO;
using UnityEngine;
using UnityEditor;
using Framvik.EngineTools.Textures;
using Framvik.DotNetLib.Extensions;

namespace Framvik.EditorTools.Menus
{
    /// <summary>
    /// Collection of MenuItem methods for populating context menus with TextureMerger shortcuts.
    /// </summary>
    public static class TextureMergerMenuItems
    {
        static bool ValidateActiveTexture() => Selection.activeObject is Texture2D;

        static Texture2D FindMainColorTextureInFolder(string mainPath)
        {
            Texture2D mainTexture = null;
            foreach (var path in Directory.EnumerateFiles(mainPath, "*", SearchOption.TopDirectoryOnly))
            {
                if (path.EndsWithAnyCaseVariants(".png", ".jpg", ".jpeg", ".tga"))
                {
                    if (Path.GetFileNameWithoutExtension(path).EndsWithAnyCaseVariants("Color", "Albedo", "Diffuse"))
                    {
                        mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                        break;
                    }
                }
            }
            return mainTexture;
        }

        static Texture2D FindColorTextureNeighborTexture(Texture2D mainTexture, params string[] search)
        {
            Texture2D texture = null;

            var mainName = mainTexture.name
                .ReplaceLast("Color", "").ReplaceLast("color", "")
                .ReplaceLast("Albedo", "").ReplaceLast("albedo", "")
                .ReplaceLast("Diffuse", "").ReplaceLast("diffuse", "");

            var filePath = AssetDatabase.GetAssetPath(mainTexture);
            var directoryPath = Path.GetDirectoryName(filePath);
            foreach (var path in Directory.EnumerateFiles(directoryPath, "*", SearchOption.TopDirectoryOnly))
            {
                if (path.Contains(mainName) && path != filePath)
                {
                    if (Path.GetFileNameWithoutExtension(path).EndsWithAnyCaseVariants(search))
                    {
                        texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                        break;
                    }
                }
            }
            return texture;
        }

        static void SaveNeighbourTexture(Texture2D mainTexture, Texture2D neighbourTexture, string saveName, SaveTextureFileFormat saveFormat)
        {
            var filePath = AssetDatabase.GetAssetPath(mainTexture);
            var directoryPath = Path.GetDirectoryName(filePath);
            var savePath = Path.Combine(directoryPath, saveName);
            if (Path.HasExtension(savePath))
                savePath = Path.ChangeExtension(savePath, null);
            TextureToFileUtility.SaveTexture2DToFile(neighbourTexture, savePath, saveFormat);
            AssetDatabase.Refresh();
        }

        static void CopyColorSmoothnessFromMetalSpecAlpha(Texture2D mainTexture)
        {
            if (mainTexture != null)
            {
                Texture2D alphaTexture = FindColorTextureNeighborTexture(mainTexture, "Metallic", "_MS", "Spec", "Specular", "_SS");

                if (alphaTexture != null)
                {
                    var tex = TextureMerger.MergeTexturesRGBA(new TextureMerger.TextureMergeSettingsRGBA
                    {
                        RedChannel = mainTexture,
                        GreenChannel = mainTexture,
                        BlueChannel = mainTexture,
                        AlphaChannel = alphaTexture
                    });
                    var mainExt = Path.GetExtension(AssetDatabase.GetAssetPath(mainTexture));
                    var alphaExt = Path.GetExtension(AssetDatabase.GetAssetPath(alphaTexture));
                    SaveNeighbourTexture(mainTexture, tex, mainTexture.name + "Smoothness", 
                        TextureToFileUtility.FileExtToSaveTextureFileFormat(mainExt, alphaExt));
                }
            }
        }


        [MenuItem(MenuPathsConstants.ASSET_TOOLS + "Textures/ColorSmoothness from Inverted Roughness Map", true, MenuPathsConstants.PRIO)]
        static bool SetAlphaSmoothnessFromInvRoughnessTextureValidate() => ValidateActiveTexture();

        [MenuItem(MenuPathsConstants.ASSET_TOOLS + "Textures/ColorSmoothness from Inverted Roughness Map", false, MenuPathsConstants.PRIO)]
        static void SetAlphaSmoothnessFromInvRoughnessTexture()
        {
            Texture2D mainTexture = Selection.activeObject as Texture2D;
            if (mainTexture != null)
            {
                Texture2D roughnessTexture = FindColorTextureNeighborTexture(mainTexture, "Roughness", "_R");

                if (roughnessTexture != null)
                {
                    var tex = TextureMerger.MergeTexturesAlphaGrayscale(mainTexture, roughnessTexture, true);
                    var mainExt = Path.GetExtension(AssetDatabase.GetAssetPath(mainTexture));
                    var texExt = Path.GetExtension(AssetDatabase.GetAssetPath(tex));
                    SaveNeighbourTexture(mainTexture, tex, mainTexture.name + "Smoothness", 
                        TextureToFileUtility.FileExtToSaveTextureFileFormat(mainExt, texExt));
                }
            }
        }

        [MenuItem(MenuPathsConstants.ASSET_TOOLS + "Textures/ColorSmoothness from MetalSpecAlpha", true, MenuPathsConstants.PRIO)]
        static bool SetAlphaSmoothnessFromMetalSpecAlphaValidate() => ValidateActiveTexture();

        [MenuItem(MenuPathsConstants.ASSET_TOOLS + "Textures/ColorSmoothness from MetalSpecAlpha", false, MenuPathsConstants.PRIO)]
        static void SetAlphaSmoothnessFromMetalSpecAlphaTexture()
        {
            CopyColorSmoothnessFromMetalSpecAlpha(Selection.activeObject as Texture2D);
        }

        [MenuItem(MenuPathsConstants.ASSET_TOOLS + "Textures/ColorSmoothness from MetalSpecAlpha (Multiple Folders)", false, MenuPathsConstants.PRIO)]
        static void SetAlphaSmoothnessFromMetalSpecAlphaTextureMULTIPLE()
        {
            var selections = Selection.instanceIDs;
            TextInputWindow.ShowWindow("", "Enter key name search main texture:", (string text) =>
            {
                foreach (var id in selections)
                {
                    var path = AssetDatabase.GetAssetPath(id);
                    if (Directory.Exists(path) && !File.Exists(path))
                    {
                        var mainTexture = FindMainColorTextureInFolder(path);
                        CopyColorSmoothnessFromMetalSpecAlpha(mainTexture);
                    }
                }
            });
        }
    }
}
