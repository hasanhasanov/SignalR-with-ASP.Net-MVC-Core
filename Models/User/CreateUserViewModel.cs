using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Chat.Models.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunlu!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Min 3 ve Max 50 karakter olmalı!")]
        [Description("UserName")]
        public string UserName { get; set; }
    }
}