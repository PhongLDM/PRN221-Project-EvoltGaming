using System;
using System.Collections.Generic;

namespace EvoltingStore.Entity
{
    public partial class GameRequirement
    {
        public int GameId { get; set; }
        public string Type { get; set; } = null!;
        public string Os { get; set; } = null!;
        public string Processor { get; set; } = null!;
        public double Memory { get; set; }
        public double Storage { get; set; }
        public int? DirectX { get; set; }
        public string? Graphic { get; set; }
        public string? Other { get; set; }

        public virtual Game Game { get; set; } = null!;
    }
}
