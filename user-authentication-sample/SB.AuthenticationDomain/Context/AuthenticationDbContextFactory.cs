using SB.Core.Context;
using SB.Enums;

namespace SB.AuthenticationDomain.Context
{
    public class AuthenticationDbContextFactory : IDbContextFactory
    {
        public IDbContext Create()
        {
            return new AuthenticationContext(
                DatabaseProvider.PostgreSQL,
                    //@"Server=127.0.0.1,1433\\Enterprise;Integrated Security=False;User Id=sa;Password=Ankara.07;MultipleActiveResultSets=True;Initial Catalog=CleanArchitectureDemo;");
                    @"Username=postgres; Password=Ankara.07; Host=127.0.0.1; Port=5432;Database=postgreDb; Integrated Security=true; Pooling=true;");                  
        }
    }
}
