using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoicesSystem.API.Models.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id_user")]
        public int IdUser { get; set; }

        [Required]
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = null!;

        [Required]
        [Column("password_hash")]
        [StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [Required]
        [Column("first_name")]
        [StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required]
        [Column("last_name")]
        [StringLength(100)]
        public string LastName { get; set; } = null!;

        [Column("role")]
        [StringLength(50)]
        public string Role { get; set; } = "User"; // Admin, User, etc.

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}