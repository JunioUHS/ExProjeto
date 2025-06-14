using System.ComponentModel.DataAnnotations;

namespace ExProjetoAPI.DTOs
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; } = string.Empty;
    }
}