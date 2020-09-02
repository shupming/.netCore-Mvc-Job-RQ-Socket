using HospitalReport.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HospitalReport.Service.Common
{
    public class Repository<T, TPrimaryKey> : IRepository<T, TPrimaryKey> where T: class, new()
    {
       private readonly IHospitalReportDbContext _hospitalReportDbContext;
        public SqlSugarClient DbClient { get; }//IHospitalReportDbContext hospitalReportDbContext
        public Repository(IHospitalReportDbContext hospitalReportDbContext)
        {
            _hospitalReportDbContext = hospitalReportDbContext;
            DbClient = _hospitalReportDbContext.GetDbClient();
        }
        public T GetById(TPrimaryKey id)
        {
            return DbClient.Queryable<T>().InSingle(id);
        }
        public T Get(Expression<Func<T, bool>> whereExpression, string orderFileds = null)
        {
            return DbClient.Queryable<T>().Where(whereExpression).OrderByIF(orderFileds != null, orderFileds).First();
        }
        public T Get(string orderFileds)
        {
            return DbClient.Queryable<T>().OrderBy( orderFileds).First();
        }
        public List<T> GetList()
        {
            return DbClient.Queryable<T>().ToList();
        }

        public ISugarQueryable<T> GetQueryable(Expression<Func<T, bool>> whereExpression = null, string orderFileds = null)
        {
            return DbClient.Queryable<T>().WhereIF(whereExpression != null, whereExpression).OrderByIF(orderFileds != null, orderFileds);
        }
        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return DbClient.Queryable<T>().Where(whereExpression).ToList();
        }
        public T GetSingle(Expression<Func<T, bool>> whereExpression)
        {
            return DbClient.Queryable<T>().Single(whereExpression);
        }
        public List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page)
        {
            int count = 0;
            var result = DbClient.Queryable<T>().Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }
        public List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = DbClient.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }
        public List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page)
        {
            int count = 0;
            var result = DbClient.Queryable<T>().Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }
        public List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
        {
            int count = 0;
            var result = DbClient.Queryable<T>().OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(conditionalList).ToPageList(page.PageIndex, page.PageSize, ref count);
            page.PageCount = count;
            return result;
        }
        public bool IsAny(Expression<Func<T, bool>> whereExpression)
        {
            return DbClient.Queryable<T>().Where(whereExpression).Any();
        }
        public int Count(Expression<Func<T, bool>> whereExpression)
        {

            return DbClient.Queryable<T>().Where(whereExpression).Count();
        }

        public bool Insert(T insertObj)
        {
            return this.DbClient.Insertable(insertObj).ExecuteCommand() > 0;
        }
        public int InsertReturnIdentity(T insertObj)
        {
            return this.DbClient.Insertable(insertObj).ExecuteReturnIdentity();
        }
        public bool InsertRange(T[] insertObjs)
        {
            return this.DbClient.Insertable(insertObjs).ExecuteCommand() > 0;
        }
        public bool InsertRange(List<T> insertObjs)
        {
            return this.DbClient.Insertable(insertObjs).ExecuteCommand() > 0;
        }
        public bool Update(T updateObj)
        {
            return this.DbClient.Updateable(updateObj).ExecuteCommand() > 0;
        }
        public bool UpdateRange(T[] updateObjs)
        {
            return this.DbClient.Updateable(updateObjs).ExecuteCommand() > 0;
        }
        public bool UpdateRange(List<T> updateObjs)
        {
            return this.DbClient.Updateable(updateObjs).ExecuteCommand() > 0;
        }
        public bool UpdateRange(List<T> updateObjs, Expression<Func<T, object>> whereColumns, Expression<Func<T, T>> columns)
        {
            return this.DbClient.Updateable(updateObjs).SetColumns(columns).WhereColumns(whereColumns).ExecuteCommand() > 0;
        }
        public bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression)
        {
            return this.DbClient.Updateable<T>().SetColumns(columns).Where(whereExpression).ExecuteCommand() > 0;
        }
        public bool Delete(T deleteObj)
        {
            return this.DbClient.Deleteable<T>().Where(deleteObj).ExecuteCommand() > 0;
        }
        public bool Delete(Expression<Func<T, bool>> whereExpression)
        {
            return this.DbClient.Deleteable<T>().Where(whereExpression).ExecuteCommand() > 0;
        }
        public bool DeleteById(TPrimaryKey id)
        {
            return this.DbClient.Deleteable<T>().In(id).ExecuteCommand() > 0;
        }
        public bool DeleteByIds(TPrimaryKey[] ids)
        {
            return this.DbClient.Deleteable<T>().In(ids).ExecuteCommand() > 0;
        }

        
    }
}
