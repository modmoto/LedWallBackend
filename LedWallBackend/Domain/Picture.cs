using System;
using MongoDB.Bson.Serialization.Attributes;

namespace LedWallBackend.Domain
{
    public class Picture
    {
        public Picture(Pixel[][] pixels, ApprovalState state, Guid id)
        {
            Pixels = pixels;
            State = state;
            Id = id;
        }

        public static Picture Create(Pixel[][] pixels)
        {
            return new Picture(pixels, ApprovalState.Undecided, Guid.NewGuid());
        }

        public Pixel[][]Pixels { get; }
        public ApprovalState State { get; private set; }
        public Guid Id { get; }

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