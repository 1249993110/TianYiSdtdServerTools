using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.Models.Players
{
    public static class PlayerExtension
    {
        /// <summary>
        /// 通过玩家名称得到玩家信息
        /// </summary>
        /// <param name="players"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PlayerInfo GetPlayerByName(this IReadOnlyDictionary<string, PlayerInfo> players, string name)
        {
            foreach(var i in players)
            {
                if(i.Value.PlayerName == name)
                {
                    return i.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 通过实体ID得到玩家信息
        /// </summary>
        /// <param name="players"></param>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public static PlayerInfo GetPlayerByEntityID(this IReadOnlyDictionary<string, PlayerInfo> players, int entityID)
        {
            foreach (var i in players)
            {
                if (i.Value.EntityID == entityID)
                {
                    return i.Value;
                }
            }
            return null;
        }
    }
}
