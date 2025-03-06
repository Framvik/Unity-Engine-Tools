using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framvik.EngineTools.Textures
{
    /// <summary>
    /// Functionality for simple modifying of textures.
    /// </summary>
    public static class TextureModifier
    {
        /// <summary>
        /// Makes texture into a grayscale image.
        /// </summary>
        public static Texture2D MakeGrayscaleTexture(Texture2D texture, float multiply, bool invert = false)
        {
            var target = new RenderTexture(texture.width, texture.height, 0);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureModifierMakeGrayscale"));
            mat.SetTexture("_Texture", texture);
            mat.SetFloat("_Multiply", multiply);
            mat.SetFloat("_Invert", invert ? -1 : 1);
            var oldRt = RenderTexture.active;
            Graphics.Blit(null, target, mat, 0);
            RenderTexture.active = oldRt;
            var tex = TextureToFileUtility.RenderTextureToTexture2D(target, SaveTextureFileFormat.JPG);
            if (Application.isPlaying)
            {
                Object.Destroy(mat);
                Object.Destroy(target);
            }
            else
            {
                Object.DestroyImmediate(mat);
                Object.DestroyImmediate(target);
            }
            return tex;
        }
    }
}
