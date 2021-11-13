using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PersonalBlog.DAL.Entities
{
    [Table("Comments")]
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(1024)]
        public string Text { get; set; }

        [Required]
        public bool IsBanned { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public DateTimeOffset ModifiedAt { get; set; }

        [Required]
        public string UserWithIdentityId { get; set; }

        [ForeignKey("UserWithIdentityId")]
        public virtual UserWithIdentity UserWithIdentity { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }

}
