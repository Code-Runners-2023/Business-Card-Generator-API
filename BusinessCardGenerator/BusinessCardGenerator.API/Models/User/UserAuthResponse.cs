using UserClass = BusinessCardGenerator.API.Data.User;
namespace BusinessCardGenerator.API.Models.User
{
    public class UserAuthResponse : UserCompressedInfoModel
    {
        public UserAuthResponse(UserClass user, string jwtToken) : base(user)
        {
            JwtToken = jwtToken;
        }

        public string JwtToken { get; set; }
    }
}
