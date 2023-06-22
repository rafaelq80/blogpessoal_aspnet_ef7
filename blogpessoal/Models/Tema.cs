using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace blogpessoal.Models
{
    public class Tema
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Descricao { get; set; } = string.Empty;

        [JsonIgnore, InverseProperty("Tema")]
        public virtual ICollection<Postagem>? Postagem { get; set; }

    }
}
