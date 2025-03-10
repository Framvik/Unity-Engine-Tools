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
        public static Texture2D MakeGrayscaleTexture(Texture2D sourceTexture, float multiply, bool invert = false)
        {
            var texture = sourceTexture.GetLinear();
            var target = RenderTextureUtility.CreateBlitRenderTexture(sourceTexture);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureModifierMakeGrayscale"));
            mat.SetTexture("_Texture", texture);
            mat.SetFloat("_Multiply", multiply);
            mat.SetFloat("_Invert", invert ? -1 : 1);
            var oldRt = RenderTexture.active;
            Graphics.Blit(null, target, mat, 0);
            RenderTexture.active = oldRt;
            var tex = RenderTextureUtility.RenderTextureToTexture2D(target);
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

        /// <summary>
        /// Inverts color of texture
        /// </summary>
        public static Texture2D InvertColors(Texture2D sourceTexture)
        {
            var texture = sourceTexture.GetLinear();
            var target = RenderTextureUtility.CreateBlitRenderTexture(sourceTexture);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureModifierMakeGrayscale"));
            mat.SetTexture("_Texture", texture);
            mat.SetFloat("_Multiply", 1);
            mat.SetFloat("_Invert", -1);
            var oldRt = RenderTexture.active;
            Graphics.Blit(null, target, mat, 0);
            RenderTexture.active = oldRt;
            var tex = RenderTextureUtility.RenderTextureToTexture2D(target);
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
