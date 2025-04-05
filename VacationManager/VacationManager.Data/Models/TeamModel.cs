using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManager.Data.Models
{
    public class TeamModel
    {
       [Required(ErrorMessage = "Моля, въведете името на екипа.")]
       [StringLength(100, ErrorMessage = "Името на екипа не може да бъде по-дълго от 100 знака.")]
        public string Name { get; set; }
    }
}
