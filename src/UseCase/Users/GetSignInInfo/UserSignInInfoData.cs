namespace UseCase.Users.GetSignInInfo
{
    public class UserSignInInfoData
    {
        public string Id { get; }
        public string Name { get; }
        public string Nickname { get; }

        public UserSignInInfoData(
            string id, 
            string name, 
            string nickname)
        {
            Id = id;
            Name = name;
            Nickname = nickname;
        }
    }
}