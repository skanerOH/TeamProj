using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PersonalBlog.DAL.Entities
{
    public class UserWithIdentity : IdentityUser
    {
        [Required]
        [MaxLength(128)]
        public string FullName { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }

}
