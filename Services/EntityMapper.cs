using AutoMapper;
using TaskManangerSystem.Actions;
using TaskManangerSystem.Models.DataBean;
using TaskManangerSystem.Models.SystemBean;

namespace TaskManangerSystem.Services
{
    public class EntityMapper : Profile
    {
        public EntityMapper(ManagementSystemContext context, IMapper mapper)
        {
            CategoryActions categoryActions = new(context, mapper);

            CreateMap<EmployeeAccountForLoginOrAdd, EmployeeAccount>()
                .ForMember(e => e.AccountPermission, o => o.MapFrom(e => 1))
                .ForMember(e => e.EmployeeId, o => o.MapFrom(op => Guid.NewGuid())).ReverseMap();

            CreateMap<EmployeeAccount, EmployeeAccountForSelectOrUpdate>()
                .ReverseMap();

            CreateMap<Category, CategoryForSelectOrUpdate>()
                .ForMember(e => e.ParentSortSerial, o => o.MapFrom(e =>
                    categoryActions.GetCategory(e.ParentCategoryId)
                        .ConditionalCheck(e => e != null, t => t!.SortSerial, 0)//隐患代码
                ))
                .ReverseMap()
                .ForMember(e => e.ParentCategoryId, o => o.MapFrom( e =>
                     e.ConditionalCheck<CategoryForSelectOrUpdate, Guid?>(e => e.ParentSortSerial >= 0, t =>
                        categoryActions.GetCategoryBySerial(e.ParentSortSerial)!.CategoryId//隐患代码
                )));
        }
    }
}
