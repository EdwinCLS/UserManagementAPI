

using System.ComponentModel.DataAnnotations;

namespace UserManagementAPI.Models
{
  public class User
  {
   public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
    public string Name { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido")]
    public string Email { get; set; }

  }
}