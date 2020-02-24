using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IceCoffee.Common.LogManager;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Utils;
using TianYiSdtdServerTools.Client.Services.Primitives.UI;

namespace TianYiSdtdServerTools.Client.ViewModels.Primitives
{
    public class ViewModelBase : ObservableObject
    {
        protected IDialogService dialogService;

        protected IDispatcherService dispatcherService;

        public ViewModelBase(IDispatcherService dispatcherService)
        {
            this.dispatcherService = dispatcherService;
        }

        public ViewModelBase(IDispatcherService dispatcherService, IDialogService dialogService) : this(dispatcherService)
        {            
            this.dialogService = dialogService;

            try
            {
                LoadConfig();
            }
            catch (Exception ex)
            {
                Log.Error("加载配置失败", ex);
            }

            dispatcherService.ShutdownStarted += OnDispatcherService_ShutdownStarted;
        }

        private void OnDispatcherService_ShutdownStarted(object sender, EventArgs e)
        {
            try
            {
                SaveConfig();
            }
            catch (Exception ex)
            {
                Log.Error("保存配置失败", ex);
            }
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        protected virtual void LoadConfig()
        {
            string modelConfigPath = System.Configuration.ConfigurationManager.AppSettings["ViewModelConfigPath"];

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(modelConfigPath);
            }
            catch (Exception e)
            {

                //throw;
            }
            
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        protected virtual void SaveConfig()
        {
            string configPath = System.Configuration.ConfigurationManager.AppSettings["ViewModelConfigPath"];

            XmlDocument doc = new XmlDocument();

            if (System.IO.File.Exists(configPath) == false)
            {
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
                doc.AppendChild(dec);

                doc.AppendChild(doc.CreateElement("ViewModelConfig"));
            }
            else
            {
                doc.Load(configPath);
            }
            string _namespace = this.GetType().Namespace;

            string prefix = _namespace.Substring(_namespace.LastIndexOf('.') + 1);

            XmlNode baseNode = doc.FirstChild.NextSibling.SelectSingleNode(prefix);
            if (baseNode == null)
            {
                baseNode = doc.CreateElement(prefix);
                doc.FirstChild.NextSibling.AppendChild(baseNode);
            }

            SaveConfig(this, doc, baseNode);

            doc.Save(configPath);
        }

        private static void SaveConfig(object obj, XmlDocument doc, XmlNode baseNode)
        {
            Type objType = obj.GetType();

            foreach (PropertyInfo property in objType.GetProperties())
            {
                ConfigNodeAttribute configNodeAttribute = property.GetCustomAttribute<ConfigNodeAttribute>();

                if(configNodeAttribute == null)
                {
                    continue;
                }

                switch (configNodeAttribute.ConfigNodeType)
                {
                    case ConfigNodeType.Element:
                        {
                            XmlNode parentNode = baseNode.SelectSingleNode(objType.Name);
                            if (parentNode == null)
                            {
                                parentNode = doc.CreateElement(objType.Name);
                                baseNode.AppendChild(parentNode);
                            }
                            baseNode = parentNode;

                            XmlNode node = baseNode.SelectSingleNode(property.PropertyType.Name);
                            if (node == null)
                            {
                                node = doc.CreateElement(property.PropertyType.Name);
                                baseNode.AppendChild(node);
                            }
                            SaveConfig(property.GetValue(obj), doc, node);
                        }
                        break;
                    case ConfigNodeType.Attribute:
                        {
                            //object value = property.GetValue(obj);
                            //if (value == null)
                            //    continue;
                            bool isFound = false;
                            XmlNode currentNode = null;
                            foreach (XmlNode node in baseNode.ChildNodes)
                            {
                                XmlAttribute xmlAttribute = node.Attributes["name"];
                                if (xmlAttribute != null)
                                {
                                    if(xmlAttribute.Value == property.Name)
                                    {
                                        isFound = true;
                                        currentNode = node;
                                        break;
                                    }
                                }
                            }

                            if (isFound == false)
                            {
                                currentNode = doc.CreateElement("property");
                                XmlAttribute xmlAttribute = doc.CreateAttribute("name");
                                xmlAttribute.Value = property.Name;
                                currentNode.Attributes.Append(xmlAttribute);
                                baseNode.AppendChild(currentNode);
                            }
                            else
                            {
                                XmlAttribute xmlAttribute = currentNode.Attributes["value"];
                                if (xmlAttribute == null)
                                {
                                    xmlAttribute = doc.CreateAttribute("value");
                                    currentNode.Attributes.Append(xmlAttribute);
                                }
                                xmlAttribute.Value = property.GetValue(obj)?.ToString();
                            }

                            //AddOrUpdateValueByName(doc, xmlNode, "name", property.Name);
                        }
                        break;
                    default:
                        break;
                }                    
            }
        }

        private static void AddOrUpdateValueByName(XmlDocument doc, XmlNode baseNode, string name, string value)
        {
            XmlAttribute xmlAttribute = baseNode.Attributes[name];
            if (xmlAttribute == null)
            {
                baseNode.Attributes.Append(doc.CreateAttribute(name));
            }
            else
            {
                xmlAttribute.Value = value;
            }
        }
    }
}
