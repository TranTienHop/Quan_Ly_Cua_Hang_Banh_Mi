using System;
using System.IO;
using PdfSharp.Fonts;

namespace QuanLyCuaHangBanhMi.Services.Reporting
{
    public class WindowsFontResolver : IFontResolver
    {
        private static bool _initialized = false;
        private static readonly object _lock = new object();

        public static void EnsureRegistered()
        {
            if (_initialized) return;
            lock (_lock)
            {
                if (!_initialized)
                {
                    try
                    {
                        if (GlobalFontSettings.FontResolver == null)
                        {
                            GlobalFontSettings.FontResolver = new WindowsFontResolver();
                        }
                    }
                    catch { }
                    _initialized = true;
                }
            }
        }

        public FontResolverInfo? ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            string cleanFamily = familyName.ToLowerInvariant();
            if (cleanFamily.Contains("segoe"))
            {
                if (isBold && isItalic) return new FontResolverInfo("segoeuiz.ttf");
                if (isBold) return new FontResolverInfo("segoeuib.ttf");
                if (isItalic) return new FontResolverInfo("segoeuii.ttf");
                return new FontResolverInfo("segoeui.ttf");
            }

            if (isBold && isItalic) return new FontResolverInfo("arialbi.ttf");
            if (isBold) return new FontResolverInfo("arialbd.ttf");
            if (isItalic) return new FontResolverInfo("ariali.ttf");
            return new FontResolverInfo("arial.ttf");
        }

        public byte[]? GetFont(string faceName)
        {
            try
            {
                string fontsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
                string path = Path.Combine(fontsFolder, faceName);
                if (File.Exists(path)) return File.ReadAllBytes(path);

                // Fallback to arial
                path = Path.Combine(fontsFolder, "arial.ttf");
                if (File.Exists(path)) return File.ReadAllBytes(path);
            }
            catch { }
            return null;
        }
    }
}
