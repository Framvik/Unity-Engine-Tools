using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Framvik.EngineTools.Textures
{
    /// <summary>
    /// Generic utility scripts for Texture objects.
    /// </summary>
    public static class TextureUtility
    {
        /// <summary>
        /// Create a linear color spaced 'editing' friendly version of this Texture2D.
        /// Used to edit and read/write with Graphics.Blit() when desiring to preserve original colors.
        /// </summary>
        public static Texture2D GetLinear(this Texture2D tex)
        {
            var texture = new Texture2D(tex.width, tex.height, tex.format, false, true);
            texture.LoadRawTextureData(tex.GetRawTextureData());
            texture.Apply();
            return texture;
        }
    }
}