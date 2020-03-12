using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IceCoffee.Common.LogManager;
using IceCoffee.Common.Xml;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Command;
using IceCoffee.Wpf.MvvmFrame.Utils;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;

namespace TianYiSdtdServerTools.Client.ViewModels.Primitives
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected IDispatcherService _dispatcherService;

        private static readonly List<Action<XmlDocument>> _saveConfigActions;

        static ViewModelBase()
        {
            _saveConfigActions = new List<Action<XmlDocument>>();
        }

        public ViewModelBase(IDispatcherService dispatcherService)
        {
            this._dispatcherService = dispatcherService;            

            _saveConfigActions.Add(this.SaveConfig);

            dispatcherService.ShutdownStarted -= OnDispatcherService_ShutdownStarted;
            dispatcherService.ShutdownStarted += OnDispatcherService_ShutdownStarted;

            LoadConfig();
        }
        /// <summary>
        /// 准备加载配置
        /// </summary>
        protected virtual void OnPrepareLoadConfig() { }

        /// <summary>
        /// 加载配置
        /// </summary>
        protected virtual void LoadConfig()
        {
            OnPrepareLoadConfig();

            Type type = this.GetType();
            try
            {
                string modelConfigPath = System.Configuration.ConfigurationManager.AppSettings["ViewModelConfigPath"];

                XmlDocument doc = new XmlDocument();
                doc.Load(modelConfigPath);

                string prefix = type.Namespace.Substring(type.Namespace.LastIndexOf('.') + 1);

                // 将继承于ViewModelBase的本类类名作为父节点
                XmlNode baseNode = doc.SelectSingleNode("ViewModelConfig/" + prefix + "/" + type.Name);

                PrivateLoad(this, baseNode);
            }
            catch (Exception ex)
            {
                Log.Error("加载配置失败 Model: " + type.Name, ex);
            }
        }

        private  static void PrivateLoad(object obj, XmlNode baseNode)
        {
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                ConfigNodeAttribute configNodeAttribute = property.GetCustomAttribute<ConfigNodeAttribute>();

                if (configNodeAttribute != null)
                {
                    switch (configNodeAttribute.XmlNodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                object propertyObj = property.GetValue(obj);

                                if (propertyObj != null)
                                {
                                    PrivateLoad(propertyObj, baseNode.SelectSingleNode(property.PropertyType.Name));
                                }                                
                            }
                            break;
                        case XmlNodeType.Attribute:
                            {
                                XmlElement currentNode = (XmlElement)baseNode.SelectSingleNode(string.Format("property[@name='{0}']", property.Name));
                                
                                string value = currentNode?.GetAttribute("value");

                                if(string.IsNullOrEmpty(value) == false)
                                {
                                    Type metaType = property.PropertyType;

                                    if (metaType.IsGenericType && metaType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        metaType = property.PropertyType.GetGenericArguments()[0];
                                    }

                                    property.SetValue(obj, Convert.ChangeType(value, metaType));
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        protected virtual void SaveConfig(XmlDocument contextDoc)
        {           
            Type type = this.GetType();

            string prefix = type.Namespace.Substring(type.Namespace.LastIndexOf('.') + 1);

            XmlNode baseNode = contextDoc.SelectSingleNode("ViewModelConfig");

            // 将PartialViews后一级类名作为父节点
            baseNode = baseNode.GetSingleChildNode(contextDoc, prefix);

            // 将继承于ViewModelBase的本类类名作为父节点
            baseNode = baseNode.GetSingleChildNode(contextDoc, type.Name);

            PrivateSave(this, contextDoc, baseNode);
        }

        private static void PrivateSave(object obj, XmlDocument contextDoc, XmlNode baseNode)
        {          
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                ConfigNodeAttribute configNodeAttribute = property.GetCustomAttribute<ConfigNodeAttribute>();

                if(configNodeAttribute != null)
                {
                    switch (configNodeAttribute.XmlNodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                // 将标记了ConfigNodeType.Element特性的属性名作为父节点
                                XmlNode currentNode = baseNode.GetSingleChildNode(contextDoc, property.PropertyType.Name);

                                PrivateSave(property.GetValue(obj), contextDoc, currentNode);
                            }
                            break;
                        case XmlNodeType.Attribute:
                            {
                                XmlElement currentNode = (XmlElement)baseNode.SelectSingleNode(string.Format("property[@name='{0}']", property.Name));
                                if (currentNode == null)
                                {
                                    currentNode = contextDoc.CreateElement("property");
                                    currentNode.SetAttribute("name", property.Name);
                                    baseNode.AppendChild(currentNode);
                                }
                                currentNode.SetAttribute("value", property.GetValue(obj)?.ToString());
                            }
                            break;
                        default:
                            break;
                    }
                }                                
            }
        }

        /// <summary>
        /// 保存所有ViewModel的配置
        /// </summary>
        public static void SaveAllConfig()
        {
            try
            {
                string configPath = System.Configuration.ConfigurationManager.AppSettings["ViewModelConfigPath"];

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
                Log.Error("保存配置失败", ex);
            }
        }

        private static void OnDispatcherService_ShutdownStarted(object sender, EventArgs e)
        {
            SaveAllConfig();
        }

    }
}
