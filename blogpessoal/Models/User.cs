using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blogpessoal.Models
{
    public class User
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public string? Foto { get; set; } = string.Empty;

        [JsonIgnore, InverseProperty("Usuario")]
        public virtual ICollection<Postagem>? Postagem { get; set; }
    }
}
