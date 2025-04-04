using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models;

namespace VacationManager.Core.Prototypes
{
    public class TeamPrototype
    {
        public required string Name { get; init; }


        public Guid? ProjectId { get; init; }
        public required Project Project { get; init; }


        public List<ApplicationUser> Developers { get; init; } = [];
    }
}
