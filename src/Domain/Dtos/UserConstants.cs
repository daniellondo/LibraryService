namespace Domain.Dtos
{
    using System.Collections.Generic;
    public class UserConstants
    {
        public static List<UserModel> Users = new()
            {
                    new UserModel(){ Username="admin",Password="admin",Role="Admin"}
            };
    }
}
