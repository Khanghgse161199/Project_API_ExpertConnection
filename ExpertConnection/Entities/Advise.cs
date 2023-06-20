using System;
using System.Collections.Generic;

namespace ExpertConnection.Entities;

public partial class Advise
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string CategoryMappingId { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    public bool ExpertConfirm { get; set; }

    public bool UserConfirm { get; set; }

    public virtual CategoryMapping CategoryMapping { get; set; } = null!;

    public virtual Chat IdNavigation { get; set; } = null!;
}
