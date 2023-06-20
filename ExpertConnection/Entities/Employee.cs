using System;
using System.Collections.Generic;

namespace ExpertConnection.Entities;

public partial class Employee
{
    public string Id { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string AcId { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual Account Ac { get; set; } = null!;
}
