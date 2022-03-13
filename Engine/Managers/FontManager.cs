using SFML.Graphics;

namespace Engine.Managers
{
    public class FontManager
    {
        
        public static Font LoadFontFromPath(string path)
            => new(path);
    }
}