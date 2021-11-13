using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PersonalBlog.DAL.Entities
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

    }

}
