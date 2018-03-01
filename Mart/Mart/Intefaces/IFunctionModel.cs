using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mart.Intefaces
{
    interface IFunctionModel<T>
    {
        void LoadData();
        bool Insert(T obj);
        bool Update(T obj);
        bool Delete(int id);
    }
}
