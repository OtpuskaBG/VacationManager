using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Core.Prototypes;
using VacationManager.Data.Models;

namespace VacationManager.Core.Services.Abstractions
{
    public interface ILeaveRequestService : IService<LeaveRequest, LeaveRequestPrototype>
    {
    }
}
