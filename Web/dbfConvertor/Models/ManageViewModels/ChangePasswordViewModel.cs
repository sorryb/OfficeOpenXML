using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelToDbfConvertor.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Parola curenta")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Campul {0} trebuie sa fie de cel putin {2} si de maxim {1} caractere ca lungime.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Noua parola")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma noua parola")]
        [Compare("NewPassword", ErrorMessage = "Cele doua parole trebuie sa fie identice.")]
        public string ConfirmPassword { get; set; }
    }
}
