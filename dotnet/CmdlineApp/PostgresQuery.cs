using System;
using System.Threading.Tasks;

namespace CmdlineApp
{
    public abstract class PostgresQuery : IPostgresQuery{
        public string ConnectionString {get; set; }

        public abstract Task<int> Run(string val, bool queryByName);

        public Task<int> Run(string id){
            return Run(id, false);
        }
    }
}