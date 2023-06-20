using System;
using System.Collections.Generic;

namespace DatabaseConection.Entities;

public partial class Rating
{
    public string Id { get; set; } = null!;

    public double RatingPoint { get; set; }

    public string Comment { get; set; } = null!;

    public bool IsActice { get; set; }

    public string UserId { get; set; } = null!;

    public virtual ICollection<Advise> Advises { get; set; } = new List<Advise>();
}
