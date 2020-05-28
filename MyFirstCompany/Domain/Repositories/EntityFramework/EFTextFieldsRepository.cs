using Microsoft.EntityFrameworkCore;
using MyFirstCompany.Domain.Entities;
using MyFirstCompany.Domain.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyFirstCompany.Domain.Repositories.EntityFramework
{
    public class EFTextFieldsRepository : ITextFieldsRepository
    {
        private readonly AppDbContext context;
        public EFTextFieldsRepository(AppDbContext context) {
            this.context = context;
        }

        public void DeleteTextField(Guid id)
        {
            context.TextFields.Remove(new TextField() { Id = id });
        }

        public TextField GetTextFieldByCodeWord(string codeWord)
        {
            return context.TextFields.FirstOrDefault(m=>m.CodeWord == codeWord);
        }

        public TextField GetTextFieldById(Guid id)
        {
            return context.TextFields.FirstOrDefault(m=>m.Id==id);
        }

        public IQueryable<TextField> GetTextFields()
        {
            return context.TextFields;
        }

        public void SaveTextField(TextField entity)
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
