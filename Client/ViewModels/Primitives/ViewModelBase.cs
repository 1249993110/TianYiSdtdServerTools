using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IceCoffee.Wpf.MvvmFrame;
using IceCoffee.Wpf.MvvmFrame.Utils;

namespace TianYiSdtdServerTools.Client.ViewModels.Primitives
{
    public class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {
            LoadConfig();

            System.Windows.Threading.Dispatcher.CurrentDispatcher.ShutdownStarted += OnCurrentDispatcher_ShutdownStarted;
        }

        private void OnCurrentDispatcher_ShutdownStarted(object sender, EventArgs e)
        {
            SaveConfig();
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
            Type objType = this.GetType();

            foreach (PropertyInfo property in objType.GetProperties())
            {
                if (property.PropertyType.IsSubclassOf(typeof(ObservableObject)))
                {
                    foreach (PropertyInfo p in property.PropertyType.GetProperties())
                    {
                        if (p.IsDefined(typeof(ConfigNodeAttribute)))
                        {
                            System.Diagnostics.Debug.Assert(p.PropertyType.IsPrimitive || typeof(string).Equals(p.PropertyType));

                            var node = doc.SelectSingleNode(string.Format("{0}/{1}", "ViewModelConfig", objType.Name));
                            if (node == null)
                            {
                                node = doc.CreateElement(objType.Name);
                                doc.SelectSingleNode("ViewModelConfig").AppendChild(node);
                            }

                            object value = p.GetValue(property.GetValue(this));
                            if (value == null)
                                continue;
                            string valueStr = value.ToString();
                            AddOrUpdateKeyValue(doc, "ViewModelConfig/" + objType.Name, property.Name,p.Name, valueStr);
                        }
                    }
                }
            }

            doc.Save(configPath);
        }

        private static void AddOrUpdateKeyValue(XmlDocument doc, string basePath, string key, 
            string name, string value)
        {
            var node = doc.SelectSingleNode(string.Format("{0}/{1}", basePath, key));
            if(node == null)
            {
                node = doc.CreateElement(key);                

                doc.SelectSingleNode(basePath).AppendChild(node);
            }
            XmlNode xmlNode = doc.CreateElement("property");
            node.AppendChild(xmlNode);

            XmlAttribute ra = doc.CreateAttribute("name");
            ra.Value = name;
            var x = xmlNode.Attributes;
            xmlNode.Attributes.Append(ra);


            //node.InnerText = value;
            //node.Attributes.Append(doc.CreateAttribute("type"));
            //node.Attributes.Append(doc.CreateAttribute("value"));
            //node.Attributes["type"].Value = "string";
            //node.Attributes["value"].Value = value;
        }
    }
}
