using System;
using System.Collections.Generic;

namespace FurnitureSalon.Core.Models;

public partial class Payment
{
    public int IdPayment { get; set; }

    public int IdOrder { get; set; }

    public int? IdInvoice { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    public string PaymentType { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentDocNumber { get; set; }

    public virtual Invoice? IdInvoiceNavigation { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;
}
