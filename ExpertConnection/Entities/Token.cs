﻿using System;
using System.Collections.Generic;

namespace ExpertConnection.Entities;

public partial class Token
{
    public string Id { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string AccId { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Account Acc { get; set; } = null!;
}
