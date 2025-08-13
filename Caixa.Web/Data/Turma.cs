using System.ComponentModel.DataAnnotations;

namespace Caixa.Web.Data
{
    public class Turma
    {
        [Display(Name = "Identificação da Turma")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "O nome da turma é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome da Turma deve conter no mínimo 3 caracteres")]
        [MaxLength(15, ErrorMessage = "O nome da Turma deve conter no máximo 15 caracteres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O nome do Professor é obrigatório")]
        [MinLength(3, ErrorMessage = "O nome do Professor deve conter no mínimo 3 caracteres")]
        public string Professor { get; set; }
    }
}
