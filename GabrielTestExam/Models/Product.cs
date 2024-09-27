using System;
using System.Collections.Generic;

namespace GabrielTestExam.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public string ProductPhoto { get; set; } = null!;

    public string ProductManufacturer { get; set; } = null!;

    public float ProductPrice { get; set; }

    public float? ProductDiscount { get; set; }

    public virtual ICollection<OrderAndProduct> OrderAndProducts { get; set; } = new List<OrderAndProduct>();
}
