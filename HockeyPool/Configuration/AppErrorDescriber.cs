using Microsoft.AspNetCore.Identity;

namespace HockeyPool.Configuration
{
    public class AppErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = "Šis lietotāja vārds jau ir aizņemts ";
            return error;
        }

    }
}
