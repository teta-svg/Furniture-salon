using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class Invoice
{
    public int IdInvoice { get; set; }

    public int IdOrder { get; set; }

    public DateOnly InvoiceDate { get; set; }

    public decimal InvoiceAmount { get; set; }

    public string InvoiceStatus { get; set; } = null!;

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
