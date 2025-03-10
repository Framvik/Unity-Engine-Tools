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
            public bool GeR => GreenChannel == RedChannel;
            public bool BeG => BlueChannel == GreenChannel;
            public bool BeR => BlueChannel == RedChannel;
            public bool AeR => AlphaChannel == RedChannel;
            public bool AeG => AlphaChannel == GreenChannel;
            public bool AeB => AlphaChannel == BlueChannel;

        }

        /// <summary>
        /// Merge rgba channels from different textures into a single Texture2D.
        /// </summary>
        public static Texture2D MergeTexturesRGBA(TextureMergeSettingsRGBA settings)
        {
            var rc = settings.RedChannel.GetLinear();
            var gc = settings.GeR ? rc : settings.GreenChannel.GetLinear();
            var bc = settings.BeR ? rc : settings.BeG ? gc : settings.BlueChannel.GetLinear();
            var ac = settings.AeR ? rc : settings.AeG ? gc : settings.AeB ? bc : settings.AlphaChannel.GetLinear();

            var target = RenderTextureUtility.CreateBlitRenderTexture(settings.RedChannel);

            var mat = new Material(Shader.Find("GraphicsBlit/TextureMergerRGBA"));
            mat.SetTexture("_TexR", rc);
            mat.SetTexture("_TexG", gc);
            mat.SetTexture("_TexB", bc);
            mat.SetTexture("_TexA", ac);
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
        /// Merge rgb channels and alpha channel of 2 different Texture2D into a single Texture2D.
        /// </summary>
        public static Texture2D MergeTexturesAlpha(Texture2D rgbTexture, Texture2D alphaTexture, bool invertAlpha = false)
        {
            var rgbTex = rgbTexture.GetLinear();
            var alphaTex = rgbTexture == alphaTexture ? rgbTex : alphaTexture.GetLinear();
            var target = RenderTextureUtility.CreateBlitRenderTexture(rgbTexture);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureMergerAlpha"));
            mat.SetTexture("_TexRGB", rgbTex);
            mat.SetTexture("_TexAlpha", alphaTex);
            mat.SetFloat("_InvertAlpha", invertAlpha ? -1 : 1);
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
        /// Merge rgb channels and alpha channel of 2 different Texture2D into a single Texture2D.
        /// </summary>
        public static Texture2D MergeTexturesAlphaGrayscale(Texture2D rgbTexture, Texture2D grayscaleTexture, bool invertAlpha = false)
        {
            var rgbTex = rgbTexture.GetLinear();
            var grayTex = rgbTexture == grayscaleTexture ? rgbTex : grayscaleTexture.GetLinear();
            var target = RenderTextureUtility.CreateBlitRenderTexture(rgbTexture);
            var mat = new Material(Shader.Find("GraphicsBlit/TextureMergerAlphaGrayscale"));
            mat.SetTexture("_TexRGB", rgbTex);
            mat.SetTexture("_TexGrayscale", grayTex);
            mat.SetFloat("_InvertAlpha", invertAlpha ? -1 : 1);
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
        /// Combines 2 Texture2D A and B by interpolating with a grayscale mask (only 3 channels).
        /// </summary>
        public static Texture2D MergeTexturesByMask(Texture2D textureA, Texture2D textureB, Texture2D mask, bool threeChannels = false)
        {
            var aTex = textureA.GetLinear();
            var bTex = textureB.GetLinear();
            var mTex = mask.GetLinear();
            var target = RenderTextureUtility.CreateBlitRenderTexture(textureA);
            Material mat;
            if (threeChannels)
                mat = new Material(Shader.Find("GraphicsBlit/TextureMergerMask3"));
            else
                mat = new Material(Shader.Find("GraphicsBlit/TextureMergerMask"));
            mat.SetTexture("_TexA", aTex);
            mat.SetTexture("_TexB", bTex);
            mat.SetTexture("_TexMask", mTex);
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
