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
                .ForMember(e => e.AccountPermission, o => o.MapFrom(e => 1))
                .ForMember(e => e.EmployeeId, o => o.MapFrom(op => Guid.NewGuid()));

            CreateMap<EmployeeAccount, EmployeeAccountForSelectOrUpdate>();


            CreateMap<Category, CategoryForSelectOrUpdate>()
                .ForMember(dest => dest.ParentSortSerial, opt => {
                    opt.PreCondition(e => e.ParentCategoryId != Guid.Empty && e.ParentCategoryId != null);
                    opt.MapFrom(e => e.ParentCategory!.SortSerial);
                    }
                );
        }
    }



}
