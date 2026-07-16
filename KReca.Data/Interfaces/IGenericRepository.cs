using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KReca.Data.Interfaces
{
    public interface IGenericRepository <T> where T : class
    {
        Task<List<T>> HepsiniGetirAsync();
        Task<T?> IdIleGetirAsync(int id);
        Task<T> EkleAsync(T entity);
        Task<T> GuncelleAsync(T entity);
        Task SilAsync(int id);
    }
}
