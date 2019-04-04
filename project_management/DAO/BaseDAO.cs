using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_management.DAO
{
    public interface BaseDAO<T>
    {
        bool Create(T obj);
        T Read(int ID);
        bool Update(T obj);
        List<T> List();
        bool Delete(int ID);
 

    }
}
