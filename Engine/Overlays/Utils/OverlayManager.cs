using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Abstract.Overlay;
using SFML.Window;

namespace Engine.Overlays.Utils
{
    public class OverlayManager
    {
        private readonly List<Overlay> _overlays = new();

        public ReadOnlyCollection<Overlay> Overlays => _overlays.AsReadOnly();

        public void AddOverlay(Overlay overlay)
        {
            _overlays.Add(overlay);
        }

        public void DrawAll()
        {
            foreach (var overlay in _overlays)
            {
                overlay.Draw();
            }
        }
        
        public void LoadContentAll()
        {
            foreach (var overlay in _overlays)
            {
                overlay.LoadContent();
            }
        }
    }
}