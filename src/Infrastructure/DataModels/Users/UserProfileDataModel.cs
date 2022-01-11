using System.ComponentModel.DataAnnotations;
using Domain.Models.Users;
using Domain.Models.Users.UserProfiles;

namespace Infrastructure.DataModels.Users
{
    public class UserProfileDataModel
    {
        [Key]
        public string UserId { get; set; }

        [Required]
        [StringLength(maximumLength: UserNickname.MaxNicknameLength)]
        public string Nickname { get; set; }
    }
}