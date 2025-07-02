using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Repositories;
using GetStartedApp.SqlSugar.Tables;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Services
{
    public class BaseService<TTable> : IBaseService<TTable> where TTable : EntityBase, new()
    {
        private readonly ISqlSugarRepository<TTable> _repository;

        public BaseService(ISqlSugarRepository<TTable> repository)
        {
            _repository = repository;
        }

        public virtual int DeletedById(int id, bool utterly = false)
        {
            if (utterly)
            {
                return _repository.Delete(id);
            }
            else
            {
                return _repository.Context.Updateable<TTable>()
                    .SetColumns(x => x.IsDeleted == "Y")
                    .Where(x => x.Id == id)
                    .ExecuteCommand();
            }
        }

        public virtual ICollection<TTable> GetAll()
        {
            return _repository
                .ToList();
        }

        public virtual ICollection<TTable> GetAllPage(ref int totalNum, int pageIndex, int pageItems = 35)
        {
            var total = 0;
            var page = _repository.Context.Queryable<TTable>()
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return page;
        }

        public virtual ICollection<TTable> GetAllPageDesc(ref int totalNum, int pageIndex, int pageItems = 35)
        {
            var total = 0;
            var page = _repository.Context.Queryable<TTable>()
                .OrderBy(x => x.CreatedTime, OrderByType.Desc)
                .ToPageList(pageIndex, pageItems, ref total);
            totalNum = total;
            return page;
        }

        public virtual int InsertOrUpdate(TTable table)
        {
            if (table.Id == 0)
            {
                //新增
                return _repository.Context.CopyNew().Insertable(table).ExecuteReturnIdentity();
            }
            else
            {
                _repository.Context.CopyNew().Updateable(table).ExecuteCommand();
                return table.Id;
            }
            //return _repository.Context.Storageable(table).ExecuteCommand();
        }
    }
}
