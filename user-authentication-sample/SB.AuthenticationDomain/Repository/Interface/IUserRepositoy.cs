using SB.Core.Repository.Interface;
using SB.Model.Entity.Authentication;

namespace SB.AuthenticationDomain.Repository.Interface
{
    public interface IUserRepository : IAuditableRepository<User>
    {
        User GetByUserName(string userName);
    }
}
