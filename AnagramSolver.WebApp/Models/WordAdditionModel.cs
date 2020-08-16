using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramSolver.WebApp.Models
{
    public class WordAdditionModel
    {
        [StringLength(25, MinimumLength = 2)]
        [Required]
        [Index(IsUnique = true)]
        public string DictionaryWord { get; set; }
        [Required]
        [RegularExpression(@"(^dll$|^rom\.sk$|^dkt$|^tikr\.dkt2|^įst$|^įv$|tikr\.dkt$|^sktv$|išt$|^sutr$prl$|^būdn$|^vksm$|^prv$|^bdv$|^jng$|^akronim$)")]
        public string DictionaryCategory { get; set; }
    }
}

