using GetStartedApp.SqlSugar.IServices;
using GetStartedApp.SqlSugar.Repositories;
using GetStartedApp.SqlSugar.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.SqlSugar.Services
{
    public class SysMenuService:ISysMenuService
    {
        private readonly ISqlSugarRepository<SysMenu> _repository;

        public SysMenuService(ISqlSugarRepository<SysMenu> repository)
        {
            _repository = repository;
        }

        public async Task<List<SysMenu>> GetMenuTreeAsync()
        {
            var allMenus = await _repository.Context.Queryable<SysMenu>().OrderBy(m => m.Id).ToListAsync();
            return BuildTree(allMenus, 0);
        }

        private List<SysMenu> BuildTree(List<SysMenu> allMenus, int parentId)
        {
            return allMenus
                .Where(m => m.ParentId == parentId)
                .Select(m => {
                    m.Menus = BuildTree(allMenus, m.Id);
                    return m;
                }).ToList();
        }
    }
}
