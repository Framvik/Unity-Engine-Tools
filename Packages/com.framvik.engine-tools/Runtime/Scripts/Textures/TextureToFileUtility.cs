using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

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
            Texture2D tex = RenderTextureUtility.RenderTextureToTexture2D(renderTexture);
            SaveTexture2DToFile(tex, filePath, fileFormat, jpgQuality);
            if (Application.isPlaying)
                Object.Destroy(tex);
            else
                Object.DestroyImmediate(tex);

        }

        /// <summary>
        /// Saves a Texture2D to disk with the specified filename and image format
        /// </summary>
        public static void SaveTexture2DToFile(Texture2D tex, string filePath, SaveTextureFileFormat fileFormat = SaveTextureFileFormat.PNG, int jpgQuality = 95)
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

        public static SaveTextureFileFormat FileExtToSaveTextureFileFormat(params string[] exts)
        {
            if (exts.Contains("exr"))
                return SaveTextureFileFormat.EXR;
            else if (exts.Contains("tga"))
                return SaveTextureFileFormat.TGA;
            return SaveTextureFileFormat.PNG;
        }
    }
}
