using System;
using System.Collections.Generic;

namespace ExpertConnection.Entities;

public partial class Chat
{
    public string Id { get; set; } = null!;

    public string AdviseId { get; set; } = null!;

    public string FromAcc { get; set; } = null!;

    public string ToAc { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Advise? Advise { get; set; }
}
