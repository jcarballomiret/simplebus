using System;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using Microsoft.DotNet.InternalAbstractions;
using System.Linq;

namespace SimpleBus.Core.Helpers
{
    public class TypeHelper
    {
        // taken from https://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class/458406#458406
        public static bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
        {
            if (typeToCheck == typeof(object))
                return false;

            if (typeToCheck == null)
                return false;

            var typeToCheckInfo = typeToCheck.GetTypeInfo();
            var genericTypeInfo = genericType.GetTypeInfo();

            if (typeToCheckInfo.IsGenericType && typeToCheckInfo.GetGenericTypeDefinition() == genericType)
                return true;

            return IsTypeDerivedFromGenericType(typeToCheckInfo.BaseType, genericType);
        }

        public static Type GetType(string typeName)
        {
            foreach (var library in DependencyContext.Default.RuntimeLibraries)
            {
                var assembly = Assembly.Load(new AssemblyName(library.Name));
                var assemblyResult = assembly.GetTypes()
                    .Where(type => type.Name == typeName)
                    .FirstOrDefault();
                if (assemblyResult != null)
                    return assemblyResult;
            }

            return null;
        }
    }
}
