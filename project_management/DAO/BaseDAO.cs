using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DAO
{
    public interface BaseDAO<T>
    {
        bool create(T obj);
        T read(int ID);
        bool update(T obj);
        List<T> list();
        T delete(int ID);
 

    }
}
