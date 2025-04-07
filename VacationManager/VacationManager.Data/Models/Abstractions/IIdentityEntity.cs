using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManager.Data.Models.Abstractions;
public interface IIdentityEntity
{
    string Id { get; set; }
}