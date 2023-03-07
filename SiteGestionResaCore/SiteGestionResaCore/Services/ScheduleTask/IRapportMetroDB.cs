using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteGestionResaCore.Services.ScheduleTask
{
    public interface IRapportMetroDB
    {
        public Task SuppressionRapportsAnciens();
    }
}
