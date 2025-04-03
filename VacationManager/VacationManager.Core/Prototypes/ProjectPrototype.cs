using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models;

namespace VacationManager.Core.Prototypes
{
    class ProjectPrototype
    {
        public required string Name { get; init; }
        public required string Description { get; init; }


        public required List<Team> Teams { get; init; } = [];
    }
}
