using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PersonalBlog.DAL.Entities
{
    [Table("Blogs")]
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Title { get; set; }

        [MaxLength(2048)]
        public string Description { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public DateTimeOffset ModifiedAt { get; set; }

        [Required]
        public string UserWithIdentityId { get; set; }

        [ForeignKey("UserWithIdentityId")]
        public virtual UserWithIdentity UserWithIdentity { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }

}
