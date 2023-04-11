using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore;

namespace Test.models

{

    public class Comentario

    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Titulo { get; set; }

        [Required]
        public String Creador { get; set; }

        [Required]
        public string Texto { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }
    }
}
