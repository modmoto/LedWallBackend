using System;

namespace LedWallBackend.Domain
{
    public class Picture
    {
        public Picture()
        {
        }

        private Picture(ApprovalState state, Guid id, string asBase64, Pixel[][] pixels)
        {
            State = state;
            Id = id;
            AsBase64 = asBase64;
            Pixels = pixels;
        }

        public static Picture Create(Pixel[][] pixels, string asBase64)
        {
            return new Picture(ApprovalState.Undecided, Guid.NewGuid(), asBase64, pixels);
        }

        public ApprovalState State { get; set; }
        public Guid Id { get; set; }
        public string AsBase64 { get; set; }
        public Pixel[][] Pixels { get; set; }

        public void MakeDecision(ApprovalState state)
        {
            State = state;
        }
    }

    public enum ApprovalState
    {
        Undecided, Approved, Rejected
    }
}