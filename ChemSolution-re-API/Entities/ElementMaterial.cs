﻿using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.Entities
{
    public class ElementMaterial
    {
        public Guid MaterialId { get; set; }
        public Material Material { get; set; } = new();
        public int ElementId { get; set; }
        public Element Element { get; set; } = new();
        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
    }
}
