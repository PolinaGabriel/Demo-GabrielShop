using System;
using System.Collections.Generic;

namespace GabrielTestExam.Models;

public partial class OrderAndProduct
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public float PriceWithoutDiscount { get; set; }

    public float PriceWithDiscount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
