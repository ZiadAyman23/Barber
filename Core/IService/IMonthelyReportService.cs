using System;
using System.Collections.Generic;
using System.Text;

namespace Core.IService
{
    public interface IMonthelyReportService
    {
        public Task<MonthelyReportMV> GenrateMonthelyReport(int Month, int Year);
        
    }
}
