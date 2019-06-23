namespace SB.Core.Context
{
    public interface IDbContextFactory
    {
        IDbContext Create();
    }
}
