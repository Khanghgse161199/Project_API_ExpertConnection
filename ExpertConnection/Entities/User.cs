using System;
using System.Collections.Generic;

namespace ExpertConnection.Entities;

public partial class User
{
    public string Id { get; set; } = null!;

    public string AcountId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public string Address { get; set; } = null!;

    public string Introduction { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool EmailActivated { get; set; }

    public bool UserConfirm { get; set; }

    public virtual Account Acount { get; set; } = null!;
}
