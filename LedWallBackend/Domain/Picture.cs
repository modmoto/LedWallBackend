using System;
using System.Collections.Generic;

namespace LedWallBackend.Domain
{
    public class Picture
    {
        private Picture(IEnumerable<IEnumerable<LedColor>> leds, bool wasApproved, Guid id)
        {
            Leds = leds;
            WasApproved = wasApproved;
            Id = id;
        }

        public static Picture Create(IEnumerable<IEnumerable<LedColor>> leds)
        {
            return new Picture(leds, false, Guid.NewGuid());
        }
        public IEnumerable<IEnumerable<LedColor>> Leds { get; }
        public bool WasApproved { get; private set; }
        public Guid Id { get; }

        public void Approve()
        {
            WasApproved = true;
        }
    }
}