using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ParkyApi.Models
{
#pragma warning disable CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
    public class NationalPark
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string State { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
    }
#pragma warning restore CS1591 // Falta el comentario XML para el tipo o miembro visible públicamente
}