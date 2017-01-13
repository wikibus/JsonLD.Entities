// ReSharper disable InconsistentNaming
#pragma warning disable SA1300 // Element must begin with upper-case letter
using System;

namespace JsonLD.Entities.Tests.Entities
{
    public class AllPrimitives
    {
        public string @string { get; set; }

        public bool? @bool { get; set; }

        public double? @double { get; set; }

        public DateTime? date { get; set; }

        public DateTimeOffset? dateOff { get; set; }

        public decimal? @decimal { get; set; }

        public long? @long { get; set; }

        public ulong? @ulong { get; set; }

        public int? @int { get; set; }

        public uint? @uint { get; set; }

        public short? @short { get; set; }

        public ushort? @ushort { get; set; }

        public byte? @byte { get; set; }

        public sbyte? @sbyte { get; set; }

        public float? @float { get; set; }

        public TimeSpan? timeSpan { get; set; }
    }
}