using AutoMapper;
using VacationManager.Data.Models;
using VacationManager.Core.Prototypes;
using VacationManager.Models.LeaveRequest;

namespace VacationManager.Profiles
{
    public class LeaveRequestProfile : Profile
    {
        public LeaveRequestProfile()
        {
            //TODO: Further expand the mapper.

            this.CreateMap<LeaveRequest, LeaveRequestViewModel>();
            this.CreateMap<LeaveRequestPrototype, LeaveRequest>();
        }
    }
}