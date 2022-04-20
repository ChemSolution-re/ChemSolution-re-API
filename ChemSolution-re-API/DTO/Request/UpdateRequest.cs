﻿using ChemSolution_re_API.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.DTO.Request
{
    public class UpdateRequest
    {
        public Guid Id { set; get; }
        public string Theme { set; get; } = string.Empty;
        public string Text { set; get; } = string.Empty;
        [Required]
        [EnumDataType(typeof(Status))]
        public string Status { get; set; } = string.Empty;
    }
}
