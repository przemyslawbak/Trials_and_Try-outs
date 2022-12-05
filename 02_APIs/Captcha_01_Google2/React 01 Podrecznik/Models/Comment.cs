using React_01_Podrecznik.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace React_01_Podrecznik.Models
{
    public class Comment : GoogleReCaptchaModelBase
    {
        [Required]
        public String Title { get; set; }

        [Required]
        public String Content { get; set; }
    }
}
