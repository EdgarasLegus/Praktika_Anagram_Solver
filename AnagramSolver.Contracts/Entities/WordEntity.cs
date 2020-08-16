//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AnagramSolver.Contracts.Entities
{
    public partial class WordEntity
    {
        public WordEntity()
        {
            CachedWord = new HashSet<CachedWordEntity>();
        }

        public int Id { get; set; }
        [StringLength(50, MinimumLength = 1)]
        [Required]
        [Index(IsUnique = true)]
        public string Word1 { get; set; }
        [Required]
        [RegularExpression(@"(^dll$|^rom\.sk$|^dkt$|^tikr\.dkt2|^įst$|^įv$|tikr\.dkt$|^sktv$|išt$|^sutr$prl$|^būdn$|^vksm$|^prv$|^bdv$|^jng$|^akronim$)")]
        public string Category { get; set; }

        public virtual ICollection<CachedWordEntity> CachedWord { get; set; }
    }

    //public class WordEntity
    //{
    //    public int Id { get; set; }
    //    public string Word { get; set; }
    //    public string Category { get; set; }
    //}
}
