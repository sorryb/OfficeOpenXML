using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelToDbfConvertor.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "eMail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Campul {0} trebuie sa fie de cel putin {2} si de maxim {1} caractere ca lungime.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmare parola")]
        [Compare("Password", ErrorMessage = "Cele doua parole trebuie sa fie identice.")]
        public string ConfirmPassword { get; set; }
    }
}
