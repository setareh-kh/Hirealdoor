using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Hirealdoor.Models;
public class BaseRepository<TEntity> where TEntity:class,ISqlEntity
{
    protected readonly SqlContext dbContext;
    protected BaseRepository(SqlContext sqlContext)
    {
        dbContext=sqlContext;
    }
    


}
