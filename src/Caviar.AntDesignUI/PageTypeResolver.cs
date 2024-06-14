using System;
using System.Linq;
using System.Reflection;
using Caviar.AntDesignUI;
using Microsoft.AspNetCore.Components;

public class PageTypeResolver
{

    public PageTypeResolver()
    {

    }

    public static Type ResolvePageType(string pagePath)
    {
        // 根据页面路径构建组件名称
        var componentName = pagePath.TrimStart('/');

        // 获取所有 Blazor 组件类型i
        var allAssembly = Config.AdditionalAssemblies;
        allAssembly.Add(Assembly.GetExecutingAssembly());
        foreach (var assembly in allAssembly)
        {
            var componentTypes = assembly.GetTypes()
                .Where(t => typeof(IComponent).IsAssignableFrom(t));

            // 在组件类型中查找与页面路径对应的组件类型
            foreach (var componentType in componentTypes)
            {
                var routeAttributes = componentType.GetCustomAttributes<RouteAttribute>();

                // 获取第一个匹配的 RouteAttribute 属性
                var matchingAttribute = routeAttributes.FirstOrDefault(a => a.Template.TrimStart('/') == componentName);
                if (matchingAttribute != null)
                {
                    return componentType;
                }
            }
        }


        return null; // 如果找不到对应路径的页面类型，则返回 null
    }
}
