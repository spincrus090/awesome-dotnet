using SB.AuthenticationDomain.Repository.Interface;
using SB.Core.Repository;
using SB.Core.UnitOfWork.Interface;
using SB.Model.Entity.Authentication;
using System.Linq;

namespace SB.AuthenticationDomain.Repository
{
    public class UserRepository : AuditableRepository<User>, IUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User GetByUserName(string userName)
        {
            return FindBy(x => x.Username == userName).SingleOrDefault();
        }
    }
}
