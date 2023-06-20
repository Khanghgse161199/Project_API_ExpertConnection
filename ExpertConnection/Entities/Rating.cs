using System;
using System.Collections.Generic;

namespace ExpertConnection.Entities;

public partial class Rating
{
    public string Id { get; set; } = null!;

    public string AdviseId { get; set; } = null!;

    public double Rating1 { get; set; }

    public string Comment { get; set; } = null!;

    public bool IsActice { get; set; }

    public string UserId { get; set; } = null!;
}
