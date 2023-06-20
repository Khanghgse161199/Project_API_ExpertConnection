using System;
using System.Collections.Generic;

namespace ExpertConnection.Entities;

public partial class Expert
{
    public string Id { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string CerfificateLink { get; set; } = null!;

    public string Introduction { get; set; } = null!;

    public double RatingSummary { get; set; }

    public string WorlkRole { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public bool EmailConfirm { get; set; }

    public bool ExpertConfirm { get; set; }

    public string AcId { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual Account Ac { get; set; } = null!;

    public virtual ICollection<CategoryMapping> CategoryMappings { get; set; } = new List<CategoryMapping>();
}
