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
                .ForMember(e => e.AccountPermission, o => o.MapFrom(e => 1))
                .ForMember(e => e.EmployeeId, o => o.MapFrom(op => Guid.NewGuid()));

            CreateMap<EmployeeAccount, EmployeeAccountForSelectOrUpdate>();


            CreateMap<Category, CategoryForSelectOrUpdate>()
                .ForMember(dest => dest.ParentSortSerial, opt =>
                {
                    opt.PreCondition(e => e.ParentCategoryId != Guid.Empty && e.ParentCategoryId != null);
                    opt.MapFrom(e => e.ParentCategory!.SortSerial);
                }
                );

            CreateMap<CategoryForAdd, Category>()
                .ConstructUsing((src, context) =>
                {
                    var dbContext = (ManagementSystemContext)context.Items["ManagementSystemContext"];
                    CategoryActions categoryActions = new(dbContext);

                    return new Category
                    {
                        CategoryId = Guid.NewGuid(),
                        ParentCategoryId = categoryActions.TryGetCategoryIdBySortSerial(src.ParentSortSerial),
                        SortSerial =  categoryActions.GetLastSerial()+1,
                        CategoryLevel = categoryActions.GetLevelBySerial(src.ParentSortSerial)
                    };
                })
                .ForMember(dest => dest.CategoryId, opt => Guid.NewGuid());
        }
    }

    public class CustomResolver : IValueResolver<CategoryForAdd, Category, Guid?>
    {

        private readonly CategoryActions _context;
        public CustomResolver(ManagementSystemContext storage) { this._context = new(storage); }

        public Guid? Resolve(CategoryForAdd source, Category destination, Guid? member, ResolutionContext context)
        => _context.TryGetCategoryIdBySortSerial(source.ParentSortSerial);

    }

}
