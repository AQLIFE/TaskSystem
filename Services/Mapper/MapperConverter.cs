using AutoMapper;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Repository;
using TaskManangerSystem.Tool;

namespace TaskManangerSystem.Services.Mapper
{
    /// <summary>
    /// 此转换器专用于新增
    /// </summary>
    public class ToEmployeeConverter : ITypeConverter<EmployeeAccountForLoginOrAdd, EmployeeAccount>
    {
        public EmployeeAccount Convert(EmployeeAccountForLoginOrAdd source, EmployeeAccount opt, ResolutionContext context)
        => new EmployeeAccount(source.EmployeeAlias, source.EmployeePwd);
    }

    public class ToCategoryConverter : ITypeConverter<CategoryForAddOrUpdate, Category>
    {
        public Category Convert(CategoryForAddOrUpdate source, Category opt, ResolutionContext context)
        {
            var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
            var sortSerial = (int)context.Items["Serial"];
            CategoryRepository CRA = new(dbContext);

            if (sortSerial != 0 && CreateObj(CRA, sortSerial, source) is Category obj)
                return obj;
            else return new Category
            {
                CategoryId = Guid.NewGuid(),
                ParentCategoryId = CRA.TryGetCategoryBySerial(source.ParentSortSerial)?.CategoryId,
                SortSerial = CRA.GetLastSerial() + 1,
                CategoryLevel = CRA.GetLevelBySerial(source.ParentSortSerial) + 1
            };
        }

        private static Category? CreateObj(CategoryRepository CRA, int sortSerial, CategoryForAddOrUpdate update)
        {
            if (CRA.TryGetCategoryBySerial(sortSerial) is Category y)
            {
                y.CategoryName = update.CategoryName;
                y.Remark = update.Remark;
                y.ParentCategoryId = CRA.TryGetCategoryBySerial(update.ParentSortSerial)?.CategoryId;
                return y;
            }
            return null;
        }
    }


    public class ToInventoryConverter : ITypeConverter<InventoryForAddOrUpdate, InventoryInfo>
    {
        public InventoryInfo Convert(InventoryForAddOrUpdate source, InventoryInfo opt, ResolutionContext context)
        {
            var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
            var id = (string)context.Items["name"];

            InventoryRepository IRA = new(dbContext);
            CategoryRepository CRA = new(dbContext);


            Guid guid = IRA.ExistsInventoryByName(id) && IRA.TryGetInventoryByName(id) is InventoryInfo i ? i.ProductId : Guid.NewGuid();
            Category? cate = source.ProductType is string s && source.ProductType != string.Empty ? CRA.TryGetCategoryByName(s) : CRA.TryGetCategoryBySerial(101);

            return new InventoryInfo(guid, source.ProductName, source.ProductPrice, source.ProductCost, source.ProductModel, cate);
        }
    }

    public class ToTaskAffairConverter : ITypeConverter<TaskAffairForAdd, TaskAffair>
    {
        public TaskAffair Convert(TaskAffairForAdd source, TaskAffair OPT, ResolutionContext context)
        {
            var storage = (ManagementSystemContext)context.Items["ManagementSystemContext"];
            CategoryRepository CRA = new(storage);
            EmployeeRepository ERA = new(storage);
            CustomerRepository CuRA = new(storage);
            TaskAffairRepository TRA = new(storage);

            Category? category = CRA.TryGetCategoryByName(source.TaskType ?? string.Empty);
            Customer? customer = CuRA.TryGetCustomerByName(source.CustomerName ?? string.Empty);
            EmployeeAccount? employee = ERA.TryGetEmployeeByName(source.EmployeeName ?? string.Empty);
            return new TaskAffair(source, category, customer, employee, TRA.GetLastSerial() + 1);
        }
    }

    public class ToPageContentConverter<TSource, TOut> : ITypeConverter<PageContent<TSource>, PageContent<TOut>>
    {
        public PageContent<TOut> Convert(PageContent<TSource> source, PageContent<TOut> opt, ResolutionContext context)
        => new PageContent<TOut>(source.PageIndex, source.MaxPage, source.Sum, context.Mapper.Map<List<TOut>>(source.PageData));
    }


    public class CategoryNameToGuidConverter : IValueConverter<string, Guid?>
    {
        public Guid? Convert(string source, ResolutionContext context)
        {
            var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
            var ser = (int)context.Items["Serial"];
            CategoryRepository CRA = new(dbContext);

            return (ser != SystemInfo.CATEGORY ? CRA.TryGetCategoryByName(source) : CRA.TryGetCategoryBySerial(SystemInfo.CATEGORY))?.CategoryId;
        }
    }

}
