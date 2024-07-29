using AutoMapper;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {

            CreateMap<EmployeeAccountForLoginOrAdd, EmployeeAccount>()
                .ConvertUsing((src, context) =>
                {
                    var ts = new EmployeeAccount();
                    ts.EmployeeAlias = src.EmployeeAlias;
                    ts.EmployeePwd = ShaHashExtensions.ComputeSHA512Hash(src.EmployeePwd);
                    return ts;
                });


            CreateMap<EmployeeAccount, EmployeeAccountForSelectOrUpdate>();

            CreateMap<PageContext<EmployeeAccount>, PageContext<EmployeeAccountForSelectOrUpdate>>()
                .ConstructUsing((src, context) => new PageContext<EmployeeAccountForSelectOrUpdate>(src.pageIndex, src.MaxPage, src.Sum, context.Mapper.Map<List<EmployeeAccountForSelectOrUpdate>>(src.data)));


            CreateMap<Category, CategoryForSelect>()
                .ForMember(dest => dest.ParentSortSerial, opt =>
                {
                    opt.PreCondition(e => e.ParentCategoryId != Guid.Empty && e.ParentCategoryId != null);
                    opt.MapFrom(e => e.ParentCategory!.SortSerial);
                }
                );
            CreateMap<PageContext<Category>, PageContext<CategoryForSelect>>()
                .ConstructUsing((src, context) => new PageContext<CategoryForSelect>(src.pageIndex, src.MaxPage, src.Sum, context.Mapper.Map<List<CategoryForSelect>>(src.data))
                 );


            CreateMap<CategoryForAddOrUpdate, Category>().ConvertUsing<CategoryForAddOrUpdateToCategoryConverter>();


            CreateMap<Customer, CustomerForSelect>()
                .ForMember(e => e.CustomerType, c => { c.PreCondition(src => src.Categories is Category); c.MapFrom(src => src.Categories!.CategoryName); });

            CreateMap<PageContext<Customer>, PageContext<CustomerForSelect>>()
                .ConstructUsing((src, context) => new PageContext<CustomerForSelect>(src.pageIndex, src.MaxPage, src.Sum, context.Mapper.Map<List<CustomerForSelect>>(src.data)));

            CreateMap<CustomerForAddOrUpdate, Customer>()
                .ForMember(e => e.CustomerType, opt => opt.ConvertUsing(new CategoryNameToGuidConverter()));



            CreateMap<InventoryForAddOrUpdate, InventoryInfo>()
            .ConstructUsing((src, context) =>
            {
                var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
                var id = (string)context.Items["name"];

                InventoryRepository IRA = new(dbContext);
                CategoryRepository CRA = new(dbContext);


                Guid guid = IRA.ExistsInventoryByName(id) && IRA.TryGetInventoryByName(id) is InventoryInfo i ? i.ProductId : Guid.NewGuid();
                Category? cate = src.ProductType is string s && src.ProductType != string.Empty ? CRA.TryGetCategoryByName(s) : CRA.TryGetCategoryBySerial(101);

                return new InventoryInfo(guid, src.ProductName, src.ProductPrice, src.ProductCost, src.ProductModel, cate);

            })
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom((src, dest) => dest.Categories?.CategoryId))
            .ReverseMap()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom(sc => sc.Categories.CategoryName??string.Empty));


            CreateMap<PageContext<InventoryInfo>, PageContext<InventoryForAddOrUpdate>>()
                .ConstructUsing((src, context) => new PageContext<InventoryForAddOrUpdate>(src.pageIndex, src.MaxPage, src.Sum, context.Mapper.Map<List<InventoryForAddOrUpdate>>(src.data)));



            CreateMap<TaskAffair, TaskAffairForView>()
                .ConstructUsing((src, context) =>
                {
                    return new TaskAffairForView(src.Categorys?.CategoryName, src.Customers?.CustomerName, src.EmployeeAccounts?.EmployeeAlias);
                })
                .ReverseMap()
                .ConstructUsing((src, context) =>
                {
                    var storage = (ManagementSystemContext)context.Items["ManagementSystemContext"];
                    CategoryRepository CRA = new(storage);
                    EmployeeRepository ERA = new(storage);
                    CustomerRepository CuRA = new(storage);

                    Category? category = CRA.TryGetCategoryByName(src.TaskType ?? string.Empty);
                    Customer? customer = CuRA.TryGetCustomerByName(src.CustomerName ?? string.Empty);
                    EmployeeAccount? employee = ERA.TryGetEmployeeByName(src.EmployeeName ?? string.Empty);
                    return new TaskAffair(category, customer, employee);
                });

            CreateMap<PageContext<TaskAffair>, PageContext<TaskAffairForView>>();

        }
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

    public class CategoryForAddOrUpdateToCategoryConverter : ITypeConverter<CategoryForAddOrUpdate, Category>
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
}
