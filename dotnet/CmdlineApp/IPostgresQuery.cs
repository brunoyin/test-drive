using System;
using System.Threading.Tasks;

namespace CmdlineApp
{
    public interface IPostgresQuery
    {
        string ConnectionString{get; set;}
        Task<int> Run(string val, bool queryByName);

        Task<int> Run(string id);
    }
}