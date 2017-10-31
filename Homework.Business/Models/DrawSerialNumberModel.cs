using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homework.Business.Models
{
    public class DrawSerialNumberModel
    {
        [Required]
        [Index]
        public Guid Id { get; set; }
    }
}