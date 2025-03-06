using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Framvik.EngineTools.Textures;

namespace Framvik.EditorTools.Menus
{
    /// <summary>
    /// Collection of MenuItem methods for populating context menus with TextureModifier shortcuts.
    /// </summary>
    public static class TextureModifierMenuItems
    {
        [MenuItem(MenuPathsConstants.ASSET_TOOLS + "Textures/Make Grayscale", true, MenuPathsConstants.PRIO)]
        static bool MakeGrayscaleValidate()
        {
            return Selection.activeObject is Texture2D;
        }

        [MenuItem(MenuPathsConstants.ASSET_TOOLS + "Textures/Make Grayscale", false, MenuPathsConstants.PRIO)]
        static void MakeGrayscale()
        {
            Texture2D mainTexture = Selection.activeObject as Texture2D;
            if (mainTexture != null)
            {
                var filePath = AssetDatabase.GetAssetPath(mainTexture);
                var directoryPath = Path.GetDirectoryName(filePath);
                var savePath = Path.Combine(directoryPath, mainTexture.name + "Grayscale");
                Texture2D grayTexture = TextureModifier.MakeGrayscaleTexture(mainTexture, 1);
                TextureToFileUtility.SaveTexture2DToFile(grayTexture, savePath, SaveTextureFileFormat.JPG);
                AssetDatabase.Refresh();
            }
        }
    }
}
