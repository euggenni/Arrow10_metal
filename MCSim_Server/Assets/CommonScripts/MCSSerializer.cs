using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

/// <summary>
/// ������ � ������ ��� ������������
/// </summary>
[Serializable]
public class PanelXMLData
{
    public string PanelName;
    public List<PanelControlXMLData> Data;

    public PanelXMLData(string panelName)
    {
        PanelName = panelName;
        Data = new List<PanelControlXMLData>();
    }

    public PanelXMLData() { }
}

/// <summary>
/// ������ � �������� ��� ������������
/// </summary>
[Serializable]
public class PanelControlXMLData
{
    public string Name;
    public object State;

    public PanelControlXMLData(string name, object state)
    {
        Name = name;
        State = state;
    }

    public PanelControlXMLData() { }
}

public static class MCSSerializer
{
    // ���������� ��������� ��� ������ �������� root-�������� � XML �����
    private static readonly Regex RootElementRegex = new Regex("<\\s*(\\w+)");

    // ��� ��� ���������� � ����� � �� �����������
    private static Hashtable BaseTypeDerivatives = Hashtable.Synchronized(new Hashtable());

    /// <summary>
    /// ������������ ������� � �����
    /// </summary>
    /// <param name="writer">����� ��� ������������</param>
    /// <param name="obj">������</param>
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
        return writer.ToString();
    }

    /// <summary>
    /// �������������� ������� �� ������
    /// </summary>
    /// <param name="reader">����� ��� ��������������</param>
    /// <param name="baseType">������� ����� �������</param>
    /// <param name="useDerivatives">������������ �� �����������</param>
    /// <returns>������</returns>
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
    /// ���������� ���, ���������� ��� ������������ �������� baseType 
    /// � root-��������� rootName
    /// </summary>
    /// <param name="baseType">������� ���</param>
    /// <param name="rootName">root-������� ���������������� � XML
    /// �������
    /// </param>
    /// <returns></returns>
    public static Type GetSerializerType(Type baseType, string rootName)
    {
        //���������� ������� ��� - ������� ��������
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
    /// ����� ���� � ����������� � ����� � �� �����������
    /// </summary>
    public static void ResetTypeCache()
    {
        BaseTypeDerivatives.Clear();
    }

    /// <summary>
    /// ������� ��� ���������� ����������� ��������� ������
    /// </summary>
    /// <param name="type">����������� ���</param>
    /// <param name="baseType">�������� ����������</param>
    /// <returns></returns>
    private static bool FilterDerivatives(Type type, object baseType)
    {
        return type.IsSubclassOf((Type) baseType);
    }

    /// <summary>
    /// �������������� ������� �� ������
    /// </summary>
    /// <param name="reader">�����</param>
    /// <param name="baseType">������� ����� �������</param>
    /// <returns>������</returns>
    public static object Deserialize(TextReader reader, Type baseType)
    {
        return Deserialize(reader, baseType, true);
    }
}
