using SqlSugar;


namespace GetStartedApp.SqlSugar.Tables
{
    /// <summary>
    /// 自定义实体基类
    /// </summary>
    public abstract class EntityBase
    {
        public virtual int Id { get; set; }
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn( Length = 200, IsNullable = true)]
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable =true)]
        public virtual DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(IsNullable =true)]
        public virtual DateTime? UpdatedTime { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 16)]
        public virtual string? CreatedUserName { get; set; }

        /// <summary>
        /// 修改者名称
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 16)]
        public virtual string? UpdatedUserName { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        [SugarColumn(IsNullable =true, Length = 1)]
        public virtual string IsDeleted { get; set; }


        public virtual void Create()
        {
           // var userName = UserInfo.UserName;
            CreatedTime = DateTime.Now;
            
          //  CreatedUserName = userName;
        }

        public virtual void Modify()
        {
           // var userName = UserInfo.UserName;
            UpdatedTime = DateTime.Now;
            
            //UpdatedUserName = userName;
        }

        /// <summary>
        /// 更新信息列
        /// </summary>
        /// <returns></returns>
        public  string[] UpdateColumn()
        {
            var result = new[] { nameof(UpdatedUserName), nameof(UpdatedTime) };
            return result;
        }

        /// <summary>
        /// 假删除的列，包含更新信息
        /// </summary>
        /// <returns></returns>
        public string[] FalseDeleteColumn()
        {
            var updateColumn = UpdateColumn();
            var deleteColumn = new[] { nameof(IsDeleted) };
            var result = new string[updateColumn.Length + deleteColumn.Length];
            deleteColumn.CopyTo(result, 0);
            updateColumn.CopyTo(result, deleteColumn.Length);
            return result;
        }
    }

    /// <summary>
    /// 递增主键实体基类
    /// </summary>
    public abstract class AutoIncrementEntity : EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey =true, IsIdentity =true)]
        // 注意是在这里定义你的公共实体
        public override int Id { get; set; }
    }

    /// <summary>
    /// 递增主键实体基类
    /// </summary>
    public abstract class NotAutoIncrementEntity : EntityBase
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        // 注意是在这里定义你的公共实体
        public override int Id { get; set; }
    }
}
