﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Pustok.Business.ViewModels;

public class BookCreateVM
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [StringLength(450)]
    public string Description { get; set; }
    public int AuthorId { get; set; }
    public int GenreId { get; set; }
    public double CostPrice { get; set; }
    public double SalePrice { get; set; }
    public int DiscountPercent { get; set; }
    public bool IsAvailable { get; set; }
    public int StockCount { get; set; }
    [Required]
    [StringLength(10)]
    public string ProductCode { get; set; }

    public IFormFile PosterImage { get; set; }
    public IFormFile HoverImage { get; set; }
    public List<IFormFile>? Images { get; set; }

}
