using Microsoft.AspNetCore.Mvc.Filters;
using SB.Core.UnitOfWork.Interface;
using System;

namespace SB.Core.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class TransactionAttribute : ActionFilterAttribute
    {
        public TransactionAttribute()
        {
            this.Order = 5;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IUnitOfWork uow = filterContext.HttpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

            if (uow == null)
            {
                // TODO throw ex.
            }

            uow.BeginTransaction();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            IUnitOfWork uow = filterContext.HttpContext.RequestServices.GetService(typeof(IUnitOfWork)) as IUnitOfWork;

            if (uow == null)
            {
                // TODO throw ex.
            }

            if (filterContext.Exception == null)
            {
                uow.CommitTransaction();
            }
            else
            {
                uow.Rollback();
            }

        }
    }
}
