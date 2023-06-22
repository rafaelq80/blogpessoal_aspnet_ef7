using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogpessoal.Models
{
    public class Postagem
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Texto { get; set; } = string.Empty;

        public DateTime Data { get; set; }

        public virtual Tema? Tema { get; set; }

        public virtual User? User { get; set; }

        public Postagem()
        {
            Data = DateTime.Now;
        }

    }
}
