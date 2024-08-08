using AutoMapper;
using TaskManangerSystem.Models;
using TaskManangerSystem.Services.Info;
using TaskManangerSystem.Services.Repository;

namespace TaskManangerSystem.Services.Mapper
{
    public class EntityMapper : Profile
    {
        public EntityMapper()
        {

            #region Employee
            CreateMap<EmployeeAccountForLoginOrAdd, Employee>()
                .ConvertUsing<ToEmployeeConverter>();


            CreateMap<Employee, EmployeeAccountForSelectOrUpdate>();

            CreateMap<PageContent<Employee>, PageContent<EmployeeAccountForSelectOrUpdate>>()
                .ConvertUsing<ToPageContentConverter<Employee, EmployeeAccountForSelectOrUpdate>>();
            #endregion


            #region categoty
            CreateMap<Category, CategoryForSelect>()
                .ForMember(dest => dest.ParentSortSerial, opt =>
                {
                    opt.PreCondition(e => e.ParentCategoryId != Guid.Empty && e.ParentCategoryId != null);
                    opt.MapFrom(e => e.ParentCategory!.SortSerial);
                }
                );

            CreateMap<PageContent<Category>, PageContent<CategoryForSelect>>()
                .ConvertUsing<ToPageContentConverter<Category, CategoryForSelect>>();



            CreateMap<CategoryForAddOrUpdate, Category>().ConvertUsing<ToCategoryConverter>();
            #endregion


            #region customer
            CreateMap<Customer, CustomerForSelect>()
                .ForMember(e => e.CustomerType, c => c.MapFrom(src => src.Categories.CategoryName));

            CreateMap<PageContent<Customer>, PageContent<CustomerForSelect>>()
                .ConvertUsing<ToPageContentConverter<Customer, CustomerForSelect>>();


            CreateMap<CustomerForAddOrUpdate, Customer>()
                .ForMember(e => e.CustomerType, opt => opt.ConvertUsing(new CategoryNameToGuidConverter()));

            #endregion


            #region inventory
            CreateMap<InventoryForAddOrUpdate, InventoryInfo>()
            .ForMember(dest => dest.ProductType, opt => opt.MapFrom((src, dest) => dest.Categories?.CategoryId))
            .ConvertUsing<ToInventoryConverter>();
            //.ReverseMap()
            CreateMap<InventoryInfo, InventoryForAddOrUpdate>()
           .ForMember(dest => dest.ProductType, opt => opt.MapFrom(sc => sc.Categories.CategoryName ?? string.Empty));


            CreateMap<PageContent<InventoryInfo>, PageContent<InventoryForAddOrUpdate>>()
                .ConvertUsing<ToPageContentConverter<InventoryInfo, InventoryForAddOrUpdate>>();
            //.ConstructUsing((src, context) => new PageContent<InventoryForAddOrUpdate>(src.PageIndex, src.MaxPage, src.Sum, context.Mapper.Map<List<InventoryForAddOrUpdate>>(src.PageData)));
            #endregion


            #region task
            CreateMap<TaskAffair, TaskAffairForSelect>()
                .ConstructUsing((src, context) => new TaskAffairForSelect(src.Categorys?.CategoryName, src.Customers?.CustomerName, src.EmployeeAccounts?.EmployeeAlias));

            CreateMap<TaskAffairForAdd, TaskAffair>().ConvertUsing<ToTaskAffairConverter>();


            CreateMap<PageContent<TaskAffair>, PageContent<TaskAffairForSelect>>()
                .ConvertUsing<ToPageContentConverter<TaskAffair, TaskAffairForSelect>>();

            #endregion
        }
    }
}
