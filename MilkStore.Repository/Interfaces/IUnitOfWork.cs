using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAcccountRepository AcccountRepository { get; }
        //IRoleRepository RoleRepository { get; }
        IImageRepository ImageRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
