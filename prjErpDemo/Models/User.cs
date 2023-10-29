using System;
using System.Collections.Generic;

namespace prjErpDemo.Models;

public partial class User
{
    public int UserID { get; set; }

    public string AccountName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Permission { get; set; }
}
