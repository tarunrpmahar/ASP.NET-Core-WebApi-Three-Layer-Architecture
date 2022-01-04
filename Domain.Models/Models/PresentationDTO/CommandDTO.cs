using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Models.PresentationDTO
{
    public class CommandDTO
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "HowTo length can't be more than 20.")]
        [Required]
        [RegularExpression(".*[a-zA-Z]+.*",ErrorMessage ="Only numeric not allowed")]
        public string HowTo { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Line length can't be more than 20.")]
        public string Line { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Platform length can't be more than 20.")]
        public string Platform { get; set; }
    }
}
