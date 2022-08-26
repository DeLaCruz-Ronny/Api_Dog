using System.ComponentModel.DataAnnotations;

namespace Api_Dog.Entidades
{
    public class Dog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        public string Raza { get; set; }
        public int Edad { get; set; }
    }
}
