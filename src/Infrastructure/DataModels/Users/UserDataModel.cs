using Domain.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.DataModels.Users
{
    public class UserDataModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(maximumLength: Username.MaxUsernameLength)]
        public string Name { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RegisteredDateTime { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDateTime { get; set; }
        
        [Required]
        public int Status { get; set; }
    }
}
