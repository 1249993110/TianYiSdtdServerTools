using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IceCoffee.Common;
using IceCoffee.Common.LogManager;
using IceCoffee.Common.Xml;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using TianYiSdtdServerTools.Client.Services.UI;

namespace TianYiSdtdServerTools.Client.ViewModels.Primitives
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly IDispatcherService dispatcherService;

        private static readonly List<Action<XmlDocument>> _saveConfigActions;

        static ViewModelBase()
        {
            _saveConfigActions = new List<Action<XmlDocument>>();
        }

        public ViewModelBase(IDispatcherService dispatcherService)
        {
            this.dispatcherService = dispatcherService;            

            _saveConfigActions.Add(this.PrivateSaveConfig);

            dispatcherService.ShutdownStarted -= OnDispatcherService_ShutdownStarted;
            dispatcherService.ShutdownStarted += OnDispatcherService_ShutdownStarted;

            PrivateLoadConfig();
        }

        private void PrivateLoadConfig()
        {
            Type type = this.GetType();
            try
            {
                string modelConfigPath = CommonHelper.GetAppSettings("ViewModelConfigPath");

                XmlDocument doc = new XmlDocument();
                doc.Load(modelConfigPath);

                string prefix = type.Namespace.Substring(type.Namespace.LastIndexOf('.') + 1);

                // 将继承于ViewModelBase的本类类名作为父节点
                XmlNode baseNode = doc.SelectSingleNode("ViewModelConfig/" + prefix + "/" + type.Name);

                XmlHelper.LoadConfig(this, baseNode);

                OnLoadConfig(doc, baseNode);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "加载配置失败 Model: " + type.Name);
            }
        }
       

        private void PrivateSaveConfig(XmlDocument contextDoc)
        {           
            Type type = this.GetType();

            string prefix = type.Namespace.Substring(type.Namespace.LastIndexOf('.') + 1);

            XmlNode baseNode = contextDoc.SelectSingleNode("ViewModelConfig");

            // 将PartialViews后一级类名作为父节点
            baseNode = baseNode.GetSingleChildNode(contextDoc, prefix);

            // 将继承于ViewModelBase的本类类名作为父节点
            baseNode = baseNode.GetSingleChildNode(contextDoc, type.Name);

            XmlHelper.SaveConfig(this, contextDoc, baseNode);

            OnSaveConfig(contextDoc, baseNode);
        }


        /// <summary>
        /// 加载配置
        /// </summary>
        protected virtual void OnLoadConfig(XmlDocument contextDoc, XmlNode baseNode) { }

        /// <summary>
        /// 保存配置
        /// </summary>
        protected virtual void OnSaveConfig(XmlDocument contextDoc, XmlNode baseNode) { }

        /// <summary>
        /// 保存所有已加载ViewModel的配置
        /// </summary>
        public static void SaveAllConfig()
        {
            try
            {
                string configPath = CommonHelper.GetAppSettings("ViewModelConfigPath");

                XmlDocument contextDoc = new XmlDocument();

                if (System.IO.File.Exists(configPath) == false)
                {
                    XmlDeclaration dec = contextDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    contextDoc.AppendChild(dec);

                    contextDoc.AppendChild(contextDoc.CreateElement("ViewModelConfig"));
                }
                else
                {
                    contextDoc.Load(configPath);
                }

                foreach (var item in _saveConfigActions)
                {
                    item.Invoke(contextDoc);
                }

                contextDoc.Save(configPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "保存配置失败");
            }
        }

        private static void OnDispatcherService_ShutdownStarted(object sender, EventArgs e)
        {
            SaveAllConfig();
        }

    }
}
