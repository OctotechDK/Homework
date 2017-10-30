using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homework.Models
{
    public class DrawSerialNumberModel
    {
        [Required]
        [Index]
        public Guid Id { get; set; }
    }
}