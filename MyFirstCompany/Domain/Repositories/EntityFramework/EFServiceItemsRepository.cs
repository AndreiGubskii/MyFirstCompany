using Microsoft.EntityFrameworkCore;
using MyFirstCompany.Domain.Entities;
using MyFirstCompany.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstCompany.Domain.Repositories.EntityFramework
{
    public class EFServiceItemsRepository : IServiceItemsRepository
    {
        private readonly AppDbContext context;

        public EFServiceItemsRepository(AppDbContext context)
        {
            this.context = context;
        }
        public void DeleteServiceItem(Guid id)
        {
            context.ServiceItem.Remove(new ServiceItem() { Id = id});
            context.SaveChanges();
        }

        public IQueryable<ServiceItem> GetServiceItem()
        {
            return context.ServiceItem;
        }

        public ServiceItem GetServiceItemById(Guid id)
        {
            return context.ServiceItem.FirstOrDefault(m => m.Id == id);
        }

        public void SaveServiceItem(ServiceItem entity)
        {
            if (entity.Id == default) {
                context.Entry(entity).State = EntityState.Added;
            } else {
                context.Entry(entity).State = EntityState.Modified;
            }

            context.SaveChanges();
        }
    }
}
