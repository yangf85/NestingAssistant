using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileOptimizer.Nesting
{
    public interface INestHandler
    {
        Task<ProfileNestingSummary> NestAsync();
    }
}