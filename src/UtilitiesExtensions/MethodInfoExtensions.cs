using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// 
    /// </summary>
    public static class MethodInfoExtensions
    {
        public static System.Reflection.MethodInfo 根据泛型参数转换为适合的泛型方法元数据(this System.Reflection.MethodInfo method, params Type[] arguments)
        {
            return method.GetGenericMethodDefinition().MakeGenericMethod(根据泛型方法参数反推泛型类型(method, arguments));
        }
        public static Type[] 根据泛型方法参数反推泛型类型(this System.Reflection.MethodInfo method, params Type[] arguments)
        {
            //非泛型方法，直接null
            if (!method.IsGenericMethod) return null;
            var genericMethodDefinition = method.GetGenericMethodDefinition();
            var genericArguments = genericMethodDefinition.GetGenericArguments();
            var parameters = genericMethodDefinition.GetParameters().Select(f => f.ParameterType).ToArray();
            if (arguments.Length != parameters.Length) return null;
            var ssss = genericArguments.ToDictionary(f => f, f => null as Type);
            根据泛型方法参数反推泛型类型_递归执行泛型匹配(parameters, arguments, ssss);

            if (ssss.Values.Any(f => f == null)) throw new Exception("反推泛型不成功，参数有缺。");
            return ssss.Values.ToArray();
        }
        private static void 根据泛型方法参数反推泛型类型_递归执行泛型匹配(Type[] parameters, Type[] arguments, Dictionary<Type, Type> ssss)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (ssss.Keys.Contains(parameters[i]))
                {
                    if (ssss[parameters[i]] == null)
                        ssss[parameters[i]] = arguments[i];
                    else if (ssss[parameters[i]] != arguments[i])
                    {
                        if (ssss[parameters[i]].IsAssignableFrom(arguments[i]))
                        {
                            //ssss[parameters[i]] = arguments[i];
                            throw new Exception("泛型的 GenericArguments 与 比较的参数的 GenericArguments 长度不一致");
                        }
                        //else if (arguments[i].IsAssignableFrom(ssss[parameters[i]]))
                        //{
                        //    throw new Exception("泛型的 GenericArguments 与 比较的参数的 GenericArguments 长度不一致");
                        //}
                    }
                    else { }
                    continue;
                }
                if (parameters[i].IsGenericType)
                {
                    var ssdfsdfsdfs = arguments[i].GetInterfacesInterfaces().Concat(arguments[i]).FirstOrDefault(f => f.IsGenericType && f.GetGenericTypeDefinition() == parameters[i].GetGenericTypeDefinition());
                    if (ssdfsdfsdfs != null)
                        根据泛型方法参数反推泛型类型_递归执行泛型匹配(parameters[i].GetGenericArguments(), ssdfsdfsdfs.GetGenericArguments(), ssss);
                    else
                        throw new Exception("类型不一致");
                }
            }
        }
    }
}
