using CsvHelper;
using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TianYiSdtdServerTools.Client.ViewModels.Managers
{
    public sealed class SdtdLocalizationManager : Singleton3<SdtdLocalizationManager>
    {
        private readonly Dictionary<string, string> _localizationDict;

        public Dictionary<string,string> LocalizationDict { get { return _localizationDict; } }

        private SdtdLocalizationManager()
        {
            _localizationDict = new Dictionary<string, string>();

            string path = CommonHelper.GetAppSettings("SdtdLocalizationPath");

            try
            {
                using (var reader = new StreamReader(path, Encoding.UTF8))
                using (var csv = new CsvReader(reader))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        string key = csv.GetField("Key");
                        string chinese = csv.GetField("schinese");
                        _localizationDict.Add(key, chinese);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "读取 {0} 错误", path);
            }
        }


        /// <summary>
        /// 得到翻译
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetTranslation(string key)
        {
            if (_localizationDict.TryGetValue(key, out string result))
            {
                return result;
            }
            return key;
        }

    }
}
