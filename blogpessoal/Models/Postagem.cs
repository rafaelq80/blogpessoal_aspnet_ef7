using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogpessoal.Models
{
    public class Postagem: Auditable
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Texto { get; set; } = string.Empty;

        public virtual Tema? Tema { get; set; }

        public virtual User? Usuario { get; set; }

        

    }
}
