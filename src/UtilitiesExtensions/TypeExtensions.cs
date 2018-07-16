using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// 类型<see cref="Type"/>辅助扩展方法类
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断类型是否为Nullable类型
        /// </summary>
        /// <param name="type"> 要处理的类型 </param>
        /// <returns> 是返回True，不是返回False </returns>
        public static bool IsNullableType(this Type type)
        {
            return ((type != null) && type.IsGenericType) && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        ///// <summary>
        ///// 由类型的Nullable类型返回实际类型
        ///// </summary>
        ///// <param name="type"> 要处理的类型对象 </param>
        ///// <returns> </returns>
        //public static Type GetNonNummableType(this Type type)
        //{
        //    if (IsNullableType(type))
        //    {
        //        return type.GetGenericArguments()[0];
        //    }
        //    return type;
        //}

        /// <summary>
        /// 通过类型转换器获取Nullable类型的基础类型
        /// </summary>
        /// <param name="type"> 要处理的类型对象 </param>
        /// <returns> </returns>
        public static Type GetUnNullableType(this Type type)
        {
            if (IsNullableType(type))
            {
                NullableConverter nullableConverter = new NullableConverter(type);
                return nullableConverter.UnderlyingType;
            }
            return type;
        }

        /// <summary>
        /// 获取成员元数据的Description特性描述信息
        /// </summary>
        /// <param name="member">成员元数据对象</param>
        /// <param name="inherit">是否搜索成员的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称</returns>
        public static string ToDescription(this MemberInfo member, bool inherit = false)
        {
            DescriptionAttribute desc = member.GetAttribute<DescriptionAttribute>(inherit);
            return desc == null ? member.Name : desc.Description;
        }

        /// <summary>
        /// 检查指定指定类型成员中是否存在指定的Attribute特性
        /// </summary>
        /// <typeparam name="T">要检查的Attribute特性类型</typeparam>
        /// <param name="memberInfo">要检查的类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>是否存在</returns>
        public static bool ExistsAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Any(m => (m as T) != null);
        }

        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttribute<T>(inherit);
            //return GetAttribute(memberInfo, typeof(T), inherit) as T;
        }
        public static Attribute GetAttribute(this MemberInfo memberInfo, Type type, bool inherit = false)
        {
            return memberInfo.GetCustomAttribute(type, inherit);
        }
        /// <summary>
        /// 从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>返回所有指定Attribute特性的数组</returns>
        public static IEnumerable<T> GetAttributes<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes<T>(inherit);
        }

        /// <summary>
        /// 判断类型是否为集合类型
        /// </summary>
        /// <param name="type">要处理的类型</param>
        /// <returns>是返回True，不是返回False</returns>
        public static bool IsEnumerable(this Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }
            return typeof(IEnumerable).IsAssignableFrom(type);
        }
        /// <summary>
        /// 判断当前泛型类型是否可由指定类型的实例填充
        /// </summary>
        /// <param name="genericType">泛型类型</param>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        public static bool IsGenericAssignableFrom(this Type genericType, Type type)
        {
            genericType.CheckNotNull("genericType");
            type.CheckNotNull("type");
            if (!genericType.IsGenericType)
            {
                return genericType.IsAssignableFrom(type);
                //throw new ArgumentException("该功能只支持泛型类型的调用，非泛型类型可使用 IsAssignableFrom 方法。");
            }

            List<Type> allOthers = new List<Type> { type };
            if (genericType.IsInterface)
            {
                allOthers.AddRange(type.GetInterfaces());
            }

            foreach (var other in allOthers)
            {
                Type cur = other;
                while (cur != null)
                {
                    if (cur.IsGenericType)
                    {
                        cur = cur.GetGenericTypeDefinition();
                    }
                    if (cur.IsSubclassOf(genericType) || cur == genericType)
                    {
                        return true;
                    }
                    cur = cur.BaseType;
                }
            }
            return false;
        }
        //public static object CreateInstance(this Type type)
        //{

        //}
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type)
        {
            //default ctor:
            ConstructorInfo ctor = type.GetConstructors().First(c => c.GetParameters().Count() == 0);
            System.Linq.Expressions.NewExpression newexpr = System.Linq.Expressions.Expression.New(ctor);
            System.Linq.Expressions.LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(newexpr);
            var newFn = lambda.Compile();
            return newFn.DynamicInvoke(new object[0]);
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T CreateInstance<T>(this Type type, params object[] args)
        {
            return (T)type.CreateInstance(args);
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstance(this Type type, params object[] args)
        {
            return Activator.CreateInstance(type, args);
            //type.GetConstructor(null).Invoke()
            ////default ctor:
            //ConstructorInfo ctor = type.GetConstructors().First(c => c.GetParameters().Count() == 0);
            //System.Linq.Expressions.NewExpression newexpr = System.Linq.Expressions.Expression.New(ctor);
            //System.Linq.Expressions.LambdaExpression lambda = System.Linq.Expressions.Expression.Lambda(newexpr);
            //var newFn = lambda.Compile();
            //return newFn.DynamicInvoke(new object[0]);
        }
        /// <summary>
        /// 是否为动态类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(this Type type)
        {
            if (!type.IsGenericType) return false;
            if ((type.Attributes & System.Reflection.TypeAttributes.NotPublic) != System.Reflection.TypeAttributes.NotPublic)
                return false;
            if (!Attribute.IsDefined(type, typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false))
                return false;
            return type.Name.Contains("AnonymousType");
        }

        //public static Type GetResolveType(this Type objectType)
        //{
        //    if (objectType == null)
        //        return null;
        //    if (objectType == typeof(IList<>))
        //    {
        //        return typeof(List<>);
        //    }
        //    if (objectType == typeof(ISet<>))
        //    {
        //        return typeof(HashSet<>);
        //    }

        //    bool nullable = (objectType.IsGenericType && (objectType.GetGenericTypeDefinition() == typeof(Nullable<>)));
        //    if (nullable)
        //    {
        //        var rt = GetResolveType(Nullable.GetUnderlyingType(objectType));
        //        return typeof(Nullable).MakeGenericType(rt);
        //    }
        //    var isArray = objectType.IsArray;
        //    if (isArray)
        //    {
        //        var rt = GetResolveType(objectType.GetElementType());
        //        return rt.MakeArrayType();
        //    }

        //    var isGenericType = objectType.IsGenericType;
        //    if (isGenericType)
        //    {
        //        var rt1 = GetResolveType(objectType.GetGenericTypeDefinition());
        //        var rt2 = objectType.GetGenericArguments();//.Select(f => GetResolveType(f)).ToArray();
        //        return rt1.MakeGenericType(rt2);
        //    }
        //    if (objectType.IsInterface || objectType.IsAbstract)
        //    {
        //        var _workContext = System.Web.HttpContext.Current.GetWorkContext();
        //        if (_workContext != null)
        //        {
        //            return _workContext.ResolveType(objectType);
        //        }
        //    }
        //    return objectType;
        //}

        public static MemberInfo[] GetInterfacesMember(this Type type, string name)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            return type.GetInterfacesMember(name, bindingAttr);
        }
        public static MemberInfo[] GetInterfacesMember(this Type type, string name, BindingFlags bindingAttr)
        {
            return type.GetInterfacesMember(name, MemberTypes.All, bindingAttr);
        }
        public static MemberInfo[] GetInterfacesMember(this Type type, string name, MemberTypes memberType, BindingFlags bindingAttr)
        {
            if (type == null) return new PropertyInfo[] { };
            return type.GetMember(name, memberType, bindingAttr).Union(type.GetInterfaces().SelectMany(f => f.GetInterfacesMember(name, memberType, bindingAttr))).Union(GetInterfacesMember(type.BaseType, name, memberType, bindingAttr)).Distinct().OrderBy(f => f.DeclaringType?.FullName).GroupBy(f => f.Name, (name1, props) => props.FirstOrDefault()).ToArray();
        }
        public static MemberInfo[] GetInterfacesMembers(this Type type)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            return type.GetInterfacesMembers(bindingAttr);
        }
        public static MemberInfo[] GetInterfacesMembers(this Type type, BindingFlags bindingAttr)
        {
            if (type == null) return new PropertyInfo[] { };
            return type.GetMembers(bindingAttr).Union(type.GetInterfaces().SelectMany(f => f.GetInterfacesMembers(bindingAttr))).Union(GetInterfacesMembers(type.BaseType, bindingAttr)).Distinct().OrderBy(f => f.DeclaringType?.FullName).GroupBy(f => f.Name, (name, props) => props.FirstOrDefault()).ToArray();
        }
        public static MethodInfo[] GetInterfacesMethods(this Type type)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            return type.GetInterfacesMethods(bindingAttr);
        }
        public static MethodInfo[] GetInterfacesMethods(this Type type, BindingFlags bindingAttr)
        {
            if (type == null) return new MethodInfo[] { };
            return type.GetMethods(bindingAttr).Union(type.GetInterfaces().SelectMany(f => f.GetInterfacesMethods(bindingAttr))).Union(GetInterfacesMethods(type.BaseType, bindingAttr)).Distinct().OrderBy(f => f.DeclaringType?.FullName).GroupBy(f => f.Name, (name, props) => props.FirstOrDefault()).ToArray();
        }
        public static PropertyInfo[] GetInterfacesProperties(this Type type)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            return type.GetInterfacesProperties(bindingAttr);
        }
        public static PropertyInfo[] GetInterfacesProperties(this Type type, BindingFlags bindingAttr)
        {
            if (type == null) return new PropertyInfo[] { };
            return type.GetProperties(bindingAttr).Union(type.GetInterfaces().SelectMany(f => f.GetInterfacesProperties(bindingAttr))).Union(GetInterfacesProperties(type.BaseType, bindingAttr)).Distinct().OrderBy(f => f.DeclaringType?.FullName).GroupBy(f => f.Name, (name, props) => props.FirstOrDefault()).ToArray();
        }


        public static PropertyInfo GetInterfacesProperty(this Type type, string name)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            var prop = type.GetProperty(name, bindingAttr);
            return prop != null ? prop : type.GetInterfaces().Select(f => f.GetProperty(name, bindingAttr)).FirstOrDefault();
        }

        public static EventInfo[] GetInterfacesEvents(this Type type)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            return type.GetInterfacesEvents(bindingAttr);
        }
        public static EventInfo[] GetInterfacesEvents(this Type type, BindingFlags bindingAttr)
        {
            if (type == null) return new EventInfo[] { };
            return type.GetEvents(bindingAttr).Union(type.GetInterfaces().SelectMany(f => f.GetInterfacesEvents(bindingAttr))).Union(GetInterfacesEvents(type.BaseType, bindingAttr)).Distinct().OrderBy(f => f.DeclaringType?.FullName).GroupBy(f => f.Name, (name, props) => props.FirstOrDefault()).ToArray();
        }
        public static FieldInfo[] GetInterfacesFields(this Type type)
        {
            var bindingAttr = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            return type.GetInterfacesFields(bindingAttr);
        }
        public static FieldInfo[] GetInterfacesFields(this Type type, BindingFlags bindingAttr)
        {
            if (type == null) return new FieldInfo[] { };
            return type.GetFields(bindingAttr).Union(type.GetInterfaces().SelectMany(f => f.GetInterfacesFields(bindingAttr))).Union(GetInterfacesFields(type.BaseType, bindingAttr)).Distinct().OrderBy(f => f.DeclaringType?.FullName).GroupBy(f => f.Name, (name, props) => props.FirstOrDefault()).ToArray();
        }
        public static Type[] GetInterfacesInterfaces(this Type type)
        {
            if (type == null) return new Type[] { };
            return type.GetInterfaces().Union(type.GetInterfaces().SelectMany(f => f.GetInterfacesInterfaces())).Union(GetInterfacesInterfaces(type.BaseType)).Distinct().OrderBy(f => f.DeclaringType?.FullName).GroupBy(f => f.Name, (name, props) => props.FirstOrDefault()).ToArray();
        }
    }
}
