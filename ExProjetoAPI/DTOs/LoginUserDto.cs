using System.ComponentModel.DataAnnotations;

namespace ExProjetoAPI.DTOs
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "O nome de usu�rio � obrigat�rio.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha � obrigat�ria.")]
        public string Password { get; set; } = string.Empty;
    }
}