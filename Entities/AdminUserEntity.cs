using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheStartupBuddy.Entities
{
    public class AdminUserEntity
    {
        public AdminUserEntity()
        {
        }

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(8000)")]
        public string ProfilePhoto { get; set; }

        [Required]
        [Column(TypeName = "varchar(6)")]
        public string Gender { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [Column(TypeName = "varchar(8000)")]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Position { get; set; }
    }
}
