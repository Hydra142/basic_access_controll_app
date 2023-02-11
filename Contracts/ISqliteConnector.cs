using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeMessenge.Contracts.Services;
public interface ISqliteConnector
{
    public Task<IList<T>> Read<T>(string sql, object options);
    public Task<int> Write(string sql, object options);
}
