﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mart.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SoldEntities : DbContext
    {
        public SoldEntities()
            : base("name=SoldEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<GetDailySold_Result> GetDailySold(string mon, Nullable<int> year, Nullable<int> empID)
        {
            var monParameter = mon != null ?
                new ObjectParameter("mon", mon) :
                new ObjectParameter("mon", typeof(string));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            var empIDParameter = empID.HasValue ?
                new ObjectParameter("empID", empID) :
                new ObjectParameter("empID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDailySold_Result>("GetDailySold", monParameter, yearParameter, empIDParameter);
        }
    
        public virtual ObjectResult<GetDayDetailsSold_Result> GetDayDetailsSold(Nullable<System.DateTime> detail_date, Nullable<int> empID)
        {
            var detail_dateParameter = detail_date.HasValue ?
                new ObjectParameter("detail_date", detail_date) :
                new ObjectParameter("detail_date", typeof(System.DateTime));
    
            var empIDParameter = empID.HasValue ?
                new ObjectParameter("empID", empID) :
                new ObjectParameter("empID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDayDetailsSold_Result>("GetDayDetailsSold", detail_dateParameter, empIDParameter);
        }
    
        public virtual ObjectResult<GetMonthlySold_Result> GetMonthlySold(Nullable<int> y, Nullable<int> empID)
        {
            var yParameter = y.HasValue ?
                new ObjectParameter("y", y) :
                new ObjectParameter("y", typeof(int));
    
            var empIDParameter = empID.HasValue ?
                new ObjectParameter("empID", empID) :
                new ObjectParameter("empID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetMonthlySold_Result>("GetMonthlySold", yParameter, empIDParameter);
        }
    
        public virtual ObjectResult<GetYearlySold_Result> GetYearlySold(Nullable<int> empID)
        {
            var empIDParameter = empID.HasValue ?
                new ObjectParameter("empID", empID) :
                new ObjectParameter("empID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetYearlySold_Result>("GetYearlySold", empIDParameter);
        }
    }
}