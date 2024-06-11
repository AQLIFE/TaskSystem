using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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


            CreateMap<CategoryForAddOrUpdate, Category>()
                .ConstructUsing((src, context) =>
                {
                    var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
                    var sortSerial = (int)context.Items["Serial"];
                    CategoryActions categoryActions = new(dbContext);

                    if (sortSerial != 0 && Sh.CreateObj(categoryActions, sortSerial, src) is Category obj)
                        return obj;
                    else return new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        ParentCategoryId = categoryActions.TryGetCategoryIdBySortSerial(src.ParentSortSerial),
                        SortSerial = categoryActions.GetLastSerial() + 1,
                        CategoryLevel = categoryActions.GetLevelBySerial(src.ParentSortSerial) + 1
                    };
                });


            CreateMap<Customer, CustomerForSelect>()
                .ForMember(e => e.CustomerType, c => { c.PreCondition(src => src.Categories is Category); c.MapFrom(src => src.Categories!.CategoryName); });

            CreateMap<PageContext<Customer>, PageContext<CustomerForSelect>>()
                .ConstructUsing((src, context) => new PageContext<CustomerForSelect>(src.pageIndex, src.MaxPage, src.Sum, context.Mapper.Map<List<CustomerForSelect>>(src.data)));

            CreateMap<CustomerForView, Customer>()
                .ConstructUsing((src, context) =>
                {
                    var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
                    var ser = (int)context.Items["Serial"];
                    CustomerActions customerActions = new(dbContext);
                    CategoryActions categoryActions = new(dbContext);


                    return new Customer(src.CustomerType is string str ? categoryActions.GetCategoryByName(str) : categoryActions.GetCategoryBySerial(ser));

                });


                CreateMap<InventoryForView, InventoryInfo>()
                .ConstructUsing((src, context) =>
                {
                    var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
                    var id = (string)context.Items["name"];

                    InventoryActions inventoryActions = new InventoryActions(dbContext);
                    CategoryActions categoryActions = new CategoryActions(dbContext);


                    Guid guid = inventoryActions.ExistsInventoryByName(id) ? inventoryActions.GetInventoryByName(id)!.ProductId : Guid.NewGuid();
                    Category? cate = src.ProductType is string s && src.ProductType != string.Empty ? categoryActions.TryGetCategoryByName(s) : categoryActions.GetCategoryBySerial(100);

                    return new InventoryInfo(guid, src.ProductName, src.ProductPrice, src.ProductCost, src.ProductModel,cate);
                    
                })
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom((src, dest) => dest.Categories?.CategoryId))
                .ReverseMap()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(sc => sc.Categories.CategoryName ?? string.Empty));
                

            CreateMap<PageContext<InventoryInfo>, PageContext<InventoryForView>>()
                .ConstructUsing((src, context) => new PageContext<InventoryForView>(src.pageIndex, src.MaxPage, src.Sum, context.Mapper.Map<List<InventoryForView>>(src.data)));

        }
    }

    public class Sh
    {
        public static Category? CreateObj(CategoryActions categoryActions, int sortSerial, CategoryForAddOrUpdate update)
        {
            var x = categoryActions.GetCategoryBySerial(sortSerial);
            if (x is Category y)
            {
                y.CategoryName = update.CategoryName;
                y.Remark = update.Remark;
                y.ParentCategoryId = categoryActions.TryGetCategoryBySerial(update.ParentSortSerial)?.CategoryId;
                return y;
            }
            return null;
        }
    }
}
