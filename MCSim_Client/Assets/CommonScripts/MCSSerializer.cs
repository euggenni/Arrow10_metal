using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

public static class MCSSerializer
{
    // Регулярное выражение для поиска названия root-элемента в XML файле
    private static readonly Regex RootElementRegex = new Regex("<\\s*(\\w+)");

    // Кэш для информации о типах и их наследниках
    private static Hashtable BaseTypeDerivatives = Hashtable.Synchronized(new Hashtable());

    /// <summary>
    /// Сериализация объекта в поток
    /// </summary>
    /// <param name="writer">Поток для сериализации</param>
    /// <param name="obj">Объект</param>
    public static void Serialize(TextWriter writer, object obj)
    {
        XmlSerializer serializer = new XmlSerializer(obj.GetType());
        serializer.Serialize(writer, obj);
    }

    public static string SerializeToString(object obj)
    {
        TextWriter writer = new StringWriter();
        XmlSerializer serializer = new XmlSerializer(obj.GetType());
        serializer.Serialize(writer, obj);

        //Debug.Log(writer.ToString());

        return writer.ToString();
    }

    /// <summary>
    /// Десериализация объекта из потока
    /// </summary>
    /// <param name="reader">Поток для десериализации</param>
    /// <param name="baseType">Базовый класс объекта</param>
    /// <param name="useDerivatives">Использовать ли наследников</param>
    /// <returns>Объект</returns>
    public static object Deserialize(TextReader reader, Type baseType, bool useDerivatives)
    {
        Type serializerType = null;
        string xmlContent = reader.ReadToEnd();

        if (useDerivatives)
        {
            Match rootMatch = RootElementRegex.Match(xmlContent);
            string rootName = rootMatch.Groups["1"].Value;

            serializerType = GetSerializerType(baseType, rootName);
        }

        if (serializerType == null)
        {
            serializerType = baseType;
        }

        XmlSerializer deserializer = new XmlSerializer(serializerType);
        return deserializer.Deserialize(new StringReader(xmlContent));
    }

    /// <summary>
    /// Возвращает тип, подходящий для сериализации объектов baseType 
    /// с root-элементом rootName
    /// </summary>
    /// <param name="baseType">Базовый тип</param>
    /// <param name="rootName">root-элемент сериализованного в XML
    /// объекта
    /// </param>
    /// <returns></returns>
    public static Type GetSerializerType(Type baseType, string rootName)
    {
        //Определяем базовый тип - вершину иерархии
        Type theMostBaseType = baseType;
        while (!theMostBaseType.BaseType.Equals(typeof (object)))
        {
            theMostBaseType = theMostBaseType.BaseType;
        }

        Guid baseTypeGuid = theMostBaseType.GUID;
        if (BaseTypeDerivatives.Contains(baseTypeGuid))
        {
            return (Type) ((Hashtable)
                           BaseTypeDerivatives[baseTypeGuid])[rootName];
        }
        else
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assemblies = currentDomain.GetAssemblies();

            Hashtable typeTable = new Hashtable();
            TypeFilter derivativesFilter =
                new TypeFilter(FilterDerivatives);

            foreach (Assembly assembly in assemblies)
            {
                Module[] modules = assembly.GetModules(false);
                foreach (Module m in modules)
                {
                    Type[] types = m.FindTypes(derivativesFilter,
                                               theMostBaseType);
                    foreach (Type type in types)
                    {
                        XmlTypeAttribute typeAttribute =
                            (XmlTypeAttribute)
                            Attribute.GetCustomAttribute(type,
                                                         typeof (XmlTypeAttribute));
                        if ((typeAttribute != null) &&
                            (typeAttribute.TypeName != null) && (typeAttribute.TypeName != ""))
                            typeTable.Add(typeAttribute.TypeName,
                                          type);
                        else
                            typeTable.Add(type.Name, type);
                    }
                }
            }
            BaseTypeDerivatives[baseTypeGuid] = typeTable;
            return (Type) typeTable[rootName];
        }
    }

    /// <summary>
    /// Сброс кэша с информацией о типах и их наследниках
    /// </summary>
    public static void ResetTypeCache()
    {
        BaseTypeDerivatives.Clear();
    }

    /// <summary>
    /// Делегат для фильтрации наследников заданного класса
    /// </summary>
    /// <param name="type">Проверяемый тип</param>
    /// <param name="baseType">Критерий фильтрации</param>
    /// <returns></returns>
    private static bool FilterDerivatives(Type type, object baseType)
    {
        return type.IsSubclassOf((Type) baseType);
    }

    /// <summary>
    /// Десериализация объекта из потока
    /// </summary>
    /// <param name="reader">Поток</param>
    /// <param name="baseType">Базовый класс объекта</param>
    /// <returns>Объект</returns>
    public static object Deserialize(TextReader reader, Type baseType)
    {
        return Deserialize(reader, baseType, true);
    }
}
