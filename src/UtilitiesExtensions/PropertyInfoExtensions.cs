using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System.Linq
{
    public static class PropertyInfoExtensions
    {
        public static bool IsSameAs(this PropertyInfo propertyInfo, PropertyInfo otherPropertyInfo)
        {
            return propertyInfo == otherPropertyInfo || (propertyInfo.Name == otherPropertyInfo.Name && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType || propertyInfo.DeclaringType.IsSubclassOf(otherPropertyInfo.DeclaringType) || otherPropertyInfo.DeclaringType.IsSubclassOf(propertyInfo.DeclaringType) || propertyInfo.DeclaringType.GetInterfaces().Contains(otherPropertyInfo.DeclaringType) || otherPropertyInfo.DeclaringType.GetInterfaces().Contains(propertyInfo.DeclaringType)));
        }

        public static bool ContainsSame(this IEnumerable<PropertyInfo> enumerable, PropertyInfo propertyInfo)
        {
            return enumerable.Any(new Func<PropertyInfo, bool>(propertyInfo.IsSameAs));
        }

        //public static bool IsValidStructuralProperty(this PropertyInfo propertyInfo)
        //{
        //    return propertyInfo.IsValidInterfaceStructuralProperty() && !propertyInfo.Getter().IsAbstract;
        //}

        //public static bool IsValidInterfaceStructuralProperty(this PropertyInfo propertyInfo)
        //{
        //    return propertyInfo.CanRead && (propertyInfo.CanWriteExtended() || propertyInfo.PropertyType.IsCollection()) && propertyInfo.GetIndexParameters().Length == 0 && propertyInfo.PropertyType.IsValidStructuralPropertyType();
        //}

        //public static bool IsValidEdmScalarProperty(this PropertyInfo propertyInfo)
        //{
        //    return propertyInfo.IsValidInterfaceStructuralProperty() && propertyInfo.PropertyType.IsValidEdmScalarType();
        //}

        //public static bool IsValidEdmNavigationProperty(this PropertyInfo propertyInfo)
        //{
        //    Type type;
        //    return propertyInfo.IsValidInterfaceStructuralProperty() && ((propertyInfo.PropertyType.IsCollection(out type) && type.IsValidStructuralType()) || propertyInfo.PropertyType.IsValidStructuralType());
        //}

        //public static EdmProperty AsEdmPrimitiveProperty(this PropertyInfo propertyInfo)
        //{
        //    Type propertyType = propertyInfo.PropertyType;
        //    bool nullable = propertyType.TryUnwrapNullableType(out propertyType) || !propertyType.IsValueType();
        //    PrimitiveType primitiveType;
        //    if (propertyType.IsPrimitiveType(out primitiveType))
        //    {
        //        EdmProperty edmProperty = EdmProperty.CreatePrimitive(propertyInfo.Name, primitiveType);
        //        edmProperty.Nullable = nullable;
        //        return edmProperty;
        //    }
        //    return null;
        //}

        public static bool CanWriteExtended(this PropertyInfo propertyInfo)
        {
            if (propertyInfo.CanWrite)
            {
                return true;
            }
            PropertyInfo declaredProperty = PropertyInfoExtensions.GetDeclaredProperty(propertyInfo);
            return declaredProperty != null && declaredProperty.CanWrite;
        }

        public static PropertyInfo GetPropertyInfoForSet(this PropertyInfo propertyInfo)
        {
            PropertyInfo arg_15_0;
            if (!propertyInfo.CanWrite)
            {
                if ((arg_15_0 = PropertyInfoExtensions.GetDeclaredProperty(propertyInfo)) == null)
                {
                    return propertyInfo;
                }
            }
            else
            {
                arg_15_0 = propertyInfo;
            }
            return arg_15_0;
        }

        private static PropertyInfo GetDeclaredProperty(PropertyInfo propertyInfo)
        {
            if (!(propertyInfo.DeclaringType == propertyInfo.ReflectedType))
            {
                return propertyInfo.DeclaringType./*GetInstanceProperties().*/GetRuntimeProperties().SingleOrDefault((PropertyInfo p) => p.Name == propertyInfo.Name && p.DeclaringType == propertyInfo.DeclaringType && !p.GetIndexParameters().Any<ParameterInfo>() && p.PropertyType == propertyInfo.PropertyType);
            }
            return propertyInfo;
        }

        public static IEnumerable<PropertyInfo> GetPropertiesInHierarchy(this PropertyInfo property)
        {
            List<PropertyInfo> list = new List<PropertyInfo>
            {
                property
            };
            PropertyInfoExtensions.CollectProperties(property, list);
            return list.Distinct<PropertyInfo>();
        }

        private static void CollectProperties(PropertyInfo property, IList<PropertyInfo> collection)
        {
            PropertyInfoExtensions.FindNextProperty(property, collection, true);
            PropertyInfoExtensions.FindNextProperty(property, collection, false);
        }

        private static void FindNextProperty(PropertyInfo property, IList<PropertyInfo> collection, bool getter)
        {
            MethodInfo methodInfo = getter ? property.Getter() : property.Setter();
            if (methodInfo != null)
            {
                Type type = methodInfo.DeclaringType.BaseType;
                if (type != null && type != typeof(object))
                {
                    MethodInfo baseMethod = methodInfo.GetBaseDefinition();
                    PropertyInfo propertyInfo = (from p in type./*GetInstanceProperties().*/GetRuntimeProperties()
                                                 let candidateMethod = getter ? p.Getter() : p.Setter()
                                                 where candidateMethod != null && candidateMethod.GetBaseDefinition() == baseMethod
                                                 select p).FirstOrDefault<PropertyInfo>();
                    if (propertyInfo != null)
                    {
                        collection.Add(propertyInfo);
                        PropertyInfoExtensions.CollectProperties(propertyInfo, collection);
                    }
                }
            }
        }

        public static MethodInfo Getter(this PropertyInfo property)
        {
            return property.GetMethod;
        }

        public static MethodInfo Setter(this PropertyInfo property)
        {
            return property.SetMethod;
        }

        public static bool IsStatic(this PropertyInfo property)
        {
            return (property.Getter() ?? property.Setter()).IsStatic;
        }

        public static bool IsPublic(this PropertyInfo property)
        {
            MethodInfo methodInfo = property.Getter();
            MethodAttributes methodAttributes = (methodInfo == null) ? MethodAttributes.Private : (methodInfo.Attributes & MethodAttributes.MemberAccessMask);
            MethodInfo methodInfo2 = property.Setter();
            MethodAttributes methodAttributes2 = (methodInfo2 == null) ? MethodAttributes.Private : (methodInfo2.Attributes & MethodAttributes.MemberAccessMask);
            MethodAttributes methodAttributes3 = (methodAttributes > methodAttributes2) ? methodAttributes : methodAttributes2;
            return methodAttributes3 == MethodAttributes.Public;
        }
    }
}