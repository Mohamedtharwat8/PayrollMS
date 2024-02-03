using System.ComponentModel.DataAnnotations;

namespace PayrollMS.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

    }
}
