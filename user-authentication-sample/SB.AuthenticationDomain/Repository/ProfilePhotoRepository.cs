using SB.AuthenticationDomain.Repository.Interface;
using SB.Core.Repository;
using SB.Core.UnitOfWork.Interface;
using SB.Model.Entity.Authentication;

namespace SB.AuthenticationDomain.Repository
{
    public class ProfilePhotoRepository : AuditableRepository<ProfilePhoto>, IProfilePhotoRepository
    {
        public ProfilePhotoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
