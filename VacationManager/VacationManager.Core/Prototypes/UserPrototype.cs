using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Enums;
using VacationManager.Data.Models;

namespace VacationManager.Core.Prototypes
{
    class UserPrototype
    {
        public Guid? TeamId { get; init; }
        public Team? Team { get; init; }
        public required Role Role { get; init; }
    }
}
