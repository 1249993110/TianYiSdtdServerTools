using IceCoffee.Common;
using IceCoffee.Wpf.MvvmFrame.Command;
using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using TianYiSdtdServerTools.Client.Models.EventArgs;

namespace TianYiSdtdServerTools.Client.Views.Converters
{
    /// <summary>
    /// <see cref="DataGridCellEditEndingEventArgs"/>转换器
    /// </summary>
    public class DataGridEventArgsConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            DataGridItemChangedEventArgs itemChangedEventArgs = new DataGridItemChangedEventArgs();

            DataGridCellEditEndingEventArgs args = (DataGridCellEditEndingEventArgs)value;

            Type itemType = args.Row.Item.GetType();

            if (args.Column is DataGridBoundColumn dataGridBoundColumn)
            {
                Binding binding = dataGridBoundColumn.Binding as Binding;
                string propertyName = binding.Path.Path;

                itemChangedEventArgs.BindPath = propertyName;                

                PropertyInfo propertyInfo = itemType.GetProperty(propertyName);

                itemChangedEventArgs.OldItem = ObjectClone.CopyProperties(args.Row.Item, itemType);

                BindingExpression bindingExpression = args.EditingElement.GetBindingExpression(TextBox.TextProperty);
                bindingExpression.UpdateSource();

                itemChangedEventArgs.NewItem = args.Row.Item;

                itemChangedEventArgs.IsChanged = !object.Equals(propertyInfo.GetValue(itemChangedEventArgs.OldItem),
                    propertyInfo.GetValue(itemChangedEventArgs.NewItem));
            }

            #region Old
            //Type itemType = args.Row.Item.GetType();

            //itemChangedEventArgs.IsNewItem = args.Row.IsNewItem;

            //if (itemChangedEventArgs.IsNewItem == false)
            //{
            //    itemChangedEventArgs.OldItem = ObjectClone.CopyProperties(args.Row.Item, itemType);
            //}

            //if (args.Column is DataGridBoundColumn dataGridBoundColumn)
            //{
            //    Binding binding = dataGridBoundColumn.Binding as Binding;
            //    string propertyName = binding.Path.Path;
            //    PropertyInfo propertyInfo = itemType.GetProperty(propertyName);

            //    BindingExpression bindingExpression = args.EditingElement.GetBindingExpression(TextBox.TextProperty);
            //    bindingExpression.UpdateSource();

            //    itemChangedEventArgs.NewItem = args.Row.Item;

            //    if (itemChangedEventArgs.IsNewItem)
            //    {
            //        itemChangedEventArgs.IsChanged = true;
            //    }
            //    else
            //    {
            //        itemChangedEventArgs.IsChanged = !object.Equals(propertyInfo.GetValue(itemChangedEventArgs.OldItem),
            //        propertyInfo.GetValue(itemChangedEventArgs.NewItem));
            //    }                    

                

            //}
            #endregion

            #region Old
            //if (args.EditingElement is TextBox textBox)
            //{
            //if (string.IsNullOrEmpty(textBox.Text) == false)
            //{
            //    Type itemType = itemChangedEventArgs.OldItem.GetType();

            //    PropertyInfo propertyInfo = itemType.GetProperty(propertyName);

            //    Type metaType = propertyInfo.PropertyType;

            //    if (metaType.IsGenericType && metaType.GetGenericTypeDefinition() == typeof(Nullable<>))
            //    {
            //        metaType = propertyInfo.PropertyType.GetGenericArguments()[0];
            //    }

            //    try
            //    {
            //        object _value = System.Convert.ChangeType(textBox.Text, metaType);
            //        itemChangedEventArgs.NewItem = ObjectClone.CopyProperties(itemType, itemChangedEventArgs.OldItem);
            //        propertyInfo.SetValue(itemChangedEventArgs.NewItem, _value);
            //    }
            //    catch (Exception)
            //    {
            //        ApplicationCommands.Undo.Execute(null, textBox);
            //        //Log.Error("DataGridCellEditEndingEventArgs转换失败", e);
            //    }
            //}
            //}
            #endregion

            return itemChangedEventArgs;
        }
    }
}
