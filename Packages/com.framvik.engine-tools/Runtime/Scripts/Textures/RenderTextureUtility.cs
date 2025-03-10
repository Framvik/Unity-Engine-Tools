using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
namespace Framvik.EngineTools.Textures
{
    public static class RenderTextureUtility
    {
        /// <summary>
        /// Creates a RenderTexture fit for Grpahics.Blit() editing based on given Texture2D dimensions.
        /// </summary>
        public static RenderTexture CreateBlitRenderTexture(Texture2D texture)
        {
            var readWrite = RenderTextureReadWrite.Linear;
            var format = RenderTextureFormat.ARGB32;
            var renderTexture = new RenderTexture(texture.width, texture.height, 0, format, readWrite);
            renderTexture.Create();
            return renderTexture;
        }

        /// <summary>
        /// Converts a RenderTexture to a Texture2D making the correct color corrections.
        /// </summary>
        public static Texture2D RenderTextureToTexture2D(RenderTexture renderTexture)
        {
            var format = TextureFormat.ARGB32;
            Texture2D tex = new(renderTexture.width, renderTexture.height, format, false, false);
            var oldRt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = oldRt;
            return tex;
        }
    }
}