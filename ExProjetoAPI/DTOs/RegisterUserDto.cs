using System.ComponentModel.DataAnnotations;

namespace ExProjetoAPI.DTOs
{
    public class RegisterUserDto
    {
        [Required(ErrorMessage = "O nome de usuário é obrigatório.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome de usuário deve ter entre 3 e 50 caracteres.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage ="O nome completo é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome completo deve até 150 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter pelo menos 8 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória.")]
        [Compare("Password", ErrorMessage = "A confirmação de senha não corresponde à senha.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}