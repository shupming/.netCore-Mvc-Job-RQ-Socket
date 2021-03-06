﻿using HospitalReport.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HospitalReport.Service.Common
{ 
    public interface IRepository<T, TPrimaryKey> 
    {
        ISugarQueryable<T> GetQueryable(Expression<Func<T, bool>> whereExpression = null, string orderFileds = null);
        T Get(string orderFileds);
        T Get(Expression<Func<T, bool>> whereExpression, string orderFileds = null);
        T GetById(TPrimaryKey id);
        List<T> GetList();
        List<T> GetList(Expression<Func<T, bool>> whereExpression);
        T GetSingle(Expression<Func<T, bool>> whereExpression);
        List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page);
        List<T> GetPageList(Expression<Func<T, bool>> whereExpression, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);
        List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page);
        List<T> GetPageList(List<IConditionalModel> conditionalList, PageModel page, Expression<Func<T, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc);
        bool IsAny(Expression<Func<T, bool>> whereExpression);
        int Count(Expression<Func<T, bool>> whereExpression);
        bool Insert(T insertObj);
        int InsertReturnIdentity(T insertObj);
        bool InsertRange(T[] insertObjs);
        bool InsertRange(List<T> insertObjs);
        bool Update(T updateObj);
        bool UpdateRange(List<T> updateObjs, Expression<Func<T, object>> whereColumns, Expression<Func<T, T>> columns);
        bool UpdateRange(T[] updateObjs);
        bool UpdateRange(List<T> updateObjs);
        bool Update(Expression<Func<T, T>> columns, Expression<Func<T, bool>> whereExpression);
        bool Delete(T deleteObj);
        bool DeleteById(TPrimaryKey id);
        bool DeleteByIds(TPrimaryKey[] ids);
    }
}
