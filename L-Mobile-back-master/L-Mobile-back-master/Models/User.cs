using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class User : IdentityUser
{


    // Navigation properties
    public virtual ICollection<Article> Articles { get; set; }
    public virtual ICollection<ServiceOrder> ServiceOrders { get; set; }


}
