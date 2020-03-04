using System.Collections.Generic;

namespace LedWallBackend.Controllers
{
    public class Picture
    {
        private Picture(IEnumerable<IEnumerable<LedColor>> leds, bool wasApproved)
        {
            Leds = leds;
            WasApproved = wasApproved;
        }

        public static Picture Create(IEnumerable<IEnumerable<LedColor>> leds)
        {
            return new Picture(leds, false);
        }
        public IEnumerable<IEnumerable<LedColor>> Leds { get; }
        public bool WasApproved { get; private set; }

        public void Approve()
        {
            WasApproved = true;
        }
    }
}