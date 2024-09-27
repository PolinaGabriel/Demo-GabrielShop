using System;
using System.Collections.Generic;

namespace GabrielTestExam.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public int OrderDeliver { get; set; }

    public string OrderPoint { get; set; } = null!;

    public string OrderCode { get; set; } = null!;

    public float OrderPrice { get; set; }

    public virtual ICollection<OrderAndProduct> OrderAndProducts { get; set; } = new List<OrderAndProduct>();
}
