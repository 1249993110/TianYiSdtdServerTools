using TianYiSdtdServerTools.Client.Models.Dtos;
using TianYiSdtdServerTools.Client.Models.Entitys;
using TianYiSdtdServerTools.Client.Services.Primitives;

namespace TianYiSdtdServerTools.Client.Services
{
    public class TeleRecordService : MyServiceBase<TeleRecord, TeleRecordDto>
    {
        public override string IdColumnName { get { return "SteamID"; } }
    }
}
