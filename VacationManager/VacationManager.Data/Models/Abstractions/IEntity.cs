using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManager.Data.Models.Abstractions
{
    public interface IEntity
    {
        Guid Id { get; set; }

        DateTime CreatedAt { get; set; }
        DateTime LastModifiedAt { get; set; }

        bool IsDeleted { get; set; }
    }
}
