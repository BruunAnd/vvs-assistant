using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VVSAssistant.ViewModels.Interfaces
{
    public interface IFilterable
    {
        bool DoesFilterMatch(string query);
    }
}
