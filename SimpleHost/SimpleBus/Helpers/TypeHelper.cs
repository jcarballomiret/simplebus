using System;

namespace SimpleBus.Helpers
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

            if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                return true;

            return IsTypeDerivedFromGenericType(typeToCheck.BaseType, genericType);
        }

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}
