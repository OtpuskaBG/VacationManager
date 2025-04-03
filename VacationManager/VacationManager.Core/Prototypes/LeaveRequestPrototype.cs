using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Enums;

namespace VacationManager.Core.Prototypes
{
    class LeaveRequestPrototype
    {
        public required DateTime StartDate { get; init; }
        public required DateTime EndDate { get; init; }
        public required DateTime RequestDate { get; init; }
        public bool HalfDay { get; init; }
        public bool Approved { get; init; }


        public required LeaveType Type { get; init; }


        public string? AttachmentPath { get; init; } // Only for sick leave
    }
}
