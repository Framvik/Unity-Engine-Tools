using UnityEngine;

namespace Framvik.EngineTools.Textures
{
    /// <summary>
    /// Functionality for merging texture channels into a single RenderTexture.
    /// </summary>
    public static class TextureMerger
    {
        /// <summary>
        /// Container class for arguments in the MergeTexturesRGBA functionality.
        /// </summary>
        public class TextureMergeSettingsRGBA
        {
            public Texture2D RedChannel;
            public Texture2D GreenChannel;
            public Texture2D BlueChannel;
            public Texture2D AlphaChannel;
            public bool InvertRed;
            public bool InvertGreen;
            public bool InvertBlue;
            public bool InvertAlpha;
        }

        /// <summary>
        /// Merge rgba channels from different textures into a single Texture2D.
        /// </summary>
        public static Texture2D MergeTexturesRGBA(TextureMergeSettingsRGBA settings)
        {
            var target = new RenderTexture(settings.RedChannel.width, settings.RedChannel.height, 0);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureMergerRGBA"));
            mat.SetTexture("_TexR", settings.RedChannel);
            mat.SetTexture("_TexG", settings.GreenChannel);
            mat.SetTexture("_TexB", settings.BlueChannel);
            mat.SetTexture("_TexA", settings.AlphaChannel);
            mat.SetVector("_Channels", 
                new Vector4(
                    settings.InvertRed ? -1 : 1,
                    settings.InvertGreen ? -1 : 1,
                    settings.InvertBlue ? -1 : 1,
                    settings.InvertAlpha ? -1 : 1)
                );
            var oldRt = RenderTexture.active;
            Graphics.Blit(null, target, mat, 0);
            RenderTexture.active = oldRt;
            var tex = TextureToFileUtility.RenderTextureToTexture2D(target);
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
        /// Merge rgb channels and alpha channel of 2 different Texture2D into a single Texture2D.
        /// </summary>
        public static Texture2D MergeTexturesAlpha(Texture2D rgbTexture, Texture2D alphaTexture, bool invertAlpha = false)
        {
            var target = new RenderTexture(rgbTexture.width, rgbTexture.height, 0);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureMergerAlpha"));
            mat.SetTexture("_TexRGB", rgbTexture);
            mat.SetTexture("_TexAlpha", alphaTexture);
            mat.SetFloat("_InvertAlpha", invertAlpha ? -1 : 1);
            var oldRt = RenderTexture.active;
            Graphics.Blit(null, target, mat, 0);
            RenderTexture.active = oldRt;
            var tex = TextureToFileUtility.RenderTextureToTexture2D(target);
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
        /// Merge rgb channels and alpha channel of 2 different Texture2D into a single Texture2D.
        /// </summary>
        public static Texture2D MergeTexturesAlphaGrayscale(Texture2D rgbTexture, Texture2D grayscaleTexture, bool invertAlpha = false)
        {
            var target = new RenderTexture(rgbTexture.width, rgbTexture.height, 0);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureMergerAlphaGrayscale"));
            mat.SetTexture("_TexRGB", rgbTexture);
            mat.SetTexture("_TexGrayscale", grayscaleTexture);
            mat.SetFloat("_InvertAlpha", invertAlpha ? -1 : 1);
            var oldRt = RenderTexture.active;
            Graphics.Blit(null, target, mat, 0);
            RenderTexture.active = oldRt;
            var tex = TextureToFileUtility.RenderTextureToTexture2D(target);
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
        /// Combines 2 Texture2D A and B by interpolating with a grayscale mask (only 3 channels).
        /// </summary>
        public static Texture2D MergeTexturesByMask(Texture2D textureA, Texture2D textureB, Texture2D mask, bool threeChannels = false)
        {
            var target = new RenderTexture(textureA.width, textureA.height, 0);
            Material mat;
            if (threeChannels)
                mat = new Material(Shader.Find("GraphicsBlit/TextureMergerMask3"));
            else
                mat = new Material(Shader.Find("GraphicsBlit/TextureMergerMask"));
            mat.SetTexture("_TexA", textureA);
            mat.SetTexture("_TexB", textureB);
            mat.SetTexture("_TexMask", mask);
            var oldRt = RenderTexture.active;
            Graphics.Blit(null, target, mat, 0);
            RenderTexture.active = oldRt;
            var tex = TextureToFileUtility.RenderTextureToTexture2D(target);
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
