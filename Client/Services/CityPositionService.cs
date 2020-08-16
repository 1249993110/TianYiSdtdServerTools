using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.Entitys;
using TianYiSdtdServerTools.Client.Services.Primitives;

namespace TianYiSdtdServerTools.Client.Services
{
    public class CityPositionService : BaseService<CityPosition, CityPositionDto>
    {
        public override string IdColumnName { get { return "TeleCmd"; } }       
    }
}
