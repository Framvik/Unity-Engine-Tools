using UnityEngine;

namespace Framvik.EngineTools.Textures
{
    /// <summary>
    /// File format definitions for SaveTextureToFileUtility.
    /// </summary>
    public enum SaveTextureFileFormat
    {
        EXR, JPG, PNG, TGA
    };

    /// <summary>
    /// Functionality for saving Texture objects to a file.
    /// </summary>
    public static class TextureToFileUtility
    {
        /// <summary>
        /// Saves a RenderTexture to disk with the specified filename and image format
        /// </summary>
        public static void SaveRenderTextureToFile(RenderTexture renderTexture, string filePath, SaveTextureFileFormat fileFormat = SaveTextureFileFormat.PNG, int jpgQuality = 95)
        {
            Texture2D tex = RenderTextureToTexture2D(renderTexture, fileFormat);
            SaveTexture2DToFile(tex, filePath, fileFormat, jpgQuality);
            if (Application.isPlaying)
                Object.Destroy(tex);
            else
                Object.DestroyImmediate(tex);

        }

        /// <summary>
        /// Saves a Texture2D to disk with the specified filename and image format
        /// </summary>
        public static void SaveTexture2DToFile(Texture2D tex, string filePath, SaveTextureFileFormat fileFormat, int jpgQuality = 95)
        {
            switch (fileFormat)
            {
                case SaveTextureFileFormat.EXR:
                    System.IO.File.WriteAllBytes(filePath + ".exr", tex.EncodeToEXR());
                    break;
                case SaveTextureFileFormat.JPG:
                    System.IO.File.WriteAllBytes(filePath + ".jpg", tex.EncodeToJPG(jpgQuality));
                    break;
                case SaveTextureFileFormat.PNG:
                    System.IO.File.WriteAllBytes(filePath + ".png", tex.EncodeToPNG());
                    break;
                case SaveTextureFileFormat.TGA:
                    System.IO.File.WriteAllBytes(filePath + ".tga", tex.EncodeToTGA());
                    break;
            }
        }

        /// <summary>
        /// Returns file extension for the given SaveTextureFileFormat (without period, ex: 'png').
        /// </summary>
        public static string SaveTextureFileFormatToFileExt(SaveTextureFileFormat fileFormat)
        {
            switch (fileFormat)
            {
                case SaveTextureFileFormat.EXR:
                    return "exr";
                case SaveTextureFileFormat.JPG:
                    return "jpg";
                case SaveTextureFileFormat.PNG:
                    return "png";
                case SaveTextureFileFormat.TGA:
                    return "tga";
            }
            return "png";
        }

        /// <summary>
        /// Returns SaveTextureFileFormat for the given file extension (including period, ex: '.png').
        /// </summary>
        public static SaveTextureFileFormat FileExtToSaveTextureFileFormat(string ext)
        {
            switch (ext)
            {
                case ".exr":
                    return SaveTextureFileFormat.EXR;
                case ".jpg":
                    return SaveTextureFileFormat.JPG;
                case ".png":
                    return SaveTextureFileFormat.PNG;
                case ".tga":
                    return SaveTextureFileFormat.TGA;
            }
            return SaveTextureFileFormat.PNG;
        }

        /// <summary>
        /// Returns file extension for the given SaveTextureFileFormat (without period, ex: 'png').
        /// </summary>
        public static SaveTextureFileFormat TextureFormatToSaveTextureFileFormat(TextureFormat textureFormat)
        {
            if (textureFormat == TextureFormat.ARGB32 || textureFormat == TextureFormat.ARGB4444)
                return SaveTextureFileFormat.EXR;
            else if (
                textureFormat == TextureFormat.RGB24 ||
                textureFormat == TextureFormat.RGB48 ||
                textureFormat == TextureFormat.RGB565 ||
                textureFormat == TextureFormat.RGB9e5Float)
                return SaveTextureFileFormat.JPG;
            else
                return SaveTextureFileFormat.PNG;
        }

        /// <summary>
        /// Converts a RenderTexture to a Texture2D with given texture file format.
        /// </summary>
        public static Texture2D RenderTextureToTexture2D(RenderTexture renderTexture, SaveTextureFileFormat fileFormat = SaveTextureFileFormat.PNG)
        {
            Texture2D tex;
            if (fileFormat != SaveTextureFileFormat.EXR)
                tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false, false);
            else
                tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBAFloat, false, true);
            var oldRt = RenderTexture.active;
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = oldRt;
            return tex;
        }
    }
}
