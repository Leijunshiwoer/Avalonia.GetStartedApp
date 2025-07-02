using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.IServices
{
    public interface IBaseService<TTable> where TTable : class, new()
    {
        ICollection<TTable> GetAll();
        ICollection<TTable> GetAllPage(ref int totalNum, int pageIndex, int pageItems = 35);
        ICollection<TTable> GetAllPageDesc(ref int totalNum, int pageIndex, int pageItems = 35);
        int InsertOrUpdate(TTable table);
        int DeletedById(int id, bool utterly = false);
    }
}
