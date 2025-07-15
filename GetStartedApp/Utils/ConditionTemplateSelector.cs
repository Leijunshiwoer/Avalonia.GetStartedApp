using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using GetStartedApp.Models;

namespace GetStartedApp.Utils;
public class ConditionTemplateSelector : IDataTemplate
{
    public IDataTemplate? ComboBoxTemplate { get; set; }
    public IDataTemplate? TextBoxTemplate { get; set; }

    public bool Match(object? data)
    {
        return data != null; // 匹配所有非空对象
    }

    public Control Build(object? param)
    {
        if (param == null)
            return new TextBlock { Text = "Null data" };

        // 获取数据对象的类型
        var type = param.GetType();

        // 检查是否存在 "Remark" 属性
        var remarkProperty = type.GetProperty("Remark");
        if (remarkProperty == null)
            return new TextBlock { Text = $"No Remark property in {type.Name}" };

        // 获取属性值
        var remarkValue = remarkProperty.GetValue(param) as string;

        // 根据条件选择模板
        if (remarkValue == "下拉框" && ComboBoxTemplate != null)
        {
            return ComboBoxTemplate.Build(param);
        }
        else if (TextBoxTemplate != null)
        {
            return TextBoxTemplate.Build(param);
        }

        // 默认回退
        return new TextBlock
        {
            Text = $"No template for: {remarkValue}",
            Foreground = Brushes.Red
        };
    }
}