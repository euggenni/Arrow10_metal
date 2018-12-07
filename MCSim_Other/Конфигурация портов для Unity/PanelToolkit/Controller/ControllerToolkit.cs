using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public static class ControllerToolkit {
    public static string controller_name = "";
    public static string ip_address = "";
    public static string tcp_port = "";

    //public class ConfigBlock
    //{
    //    private string _blockName;
    //    public string BlockName
    //    {
    //        get{ return _blockName; }
    //        set { _blockName = value; }
    //    }
    //}

    public static List<BlockControl> bcList;
    public static List<PortDevice> pdList;
    public static List<Port> pList;

    public class BlockControl
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<ElemTumbler> etList = new List<ElemTumbler>();
        public List<ElemIndicator> eiList = new List<ElemIndicator>();
        public List<ElemSpinner> esList = new List<ElemSpinner>();
        public List<ElemJoystick> ejList = new List<ElemJoystick>();
    }

    public class PortDevice
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }

    public class Port
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _position;
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }

    public class ConfigElem
    {
        private string _elemName;
        public string ElemName
        {
            get { return _elemName; }
            set { _elemName = value; }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public List<Port> _portList = new List<Port>();
    }

    public class ElemTumbler : ConfigElem
    {
        public List<string> statelist = new List<string>();

    }

    public class ElemIndicator : ConfigElem
    {
        
    }

    public class ElemSpinner : ConfigElem
    {

    }

    public class ElemJoystick : ConfigElem
    {
        private float _min_calib;
        public float Min_calib
        {
            get { return _min_calib; }
            set { _min_calib = value; }
        }

        private float _max_calib;
        public float Max_calib
        {
            get { return _max_calib; }
            set { _max_calib = value; }
        }
    }

    public static string ports_configuration_path; //пути устанавливаются относительно корневой директории проекта

    public static string controller_ip;
    public static int controller_port;

    public static bool enable_controller = true;
    private const int portsCount = 30; //количество элементов всех типов в схеме

    public static int[] ports = new int[portsCount];

    public static string[] elements = new string[portsCount];

    public static List<ElemTumbler> InitedTumblers = new List<ElemTumbler>();

    static void Start () {
        if (initControllersConfig())
            ControllerOutResult();

        initPortsConfig();
	}

    static void ControllerOutResult()
    {
        bool plc_connect;

        //DiscreteCoils передаются не как регистры, поэтому пока непонятно как отличить ввод, который не существует, от ввода, на который не подат сигнал (оба false)
        //public bool[] ports = new bool[portsCount]; //понадобится на случай, если не удастся отличить один тип ввода от другого, тогда придется количественно задавать порты
    }

    private class PortsOutResult
    {
        //public bool ;
    }


    static bool initControllersConfig()
    {
        //первый тэг декларации файла
        //первый тэг оборудования

        bool controller_inited = false;
        bool xmldeclar = false;
        bool ip_set = false;
        bool port_set = false;

        string curr_xml_text;
        string prev_elem;

        BlockControl bc = new BlockControl();
        PortDevice pd = new PortDevice();

        ElemTumbler et = new ElemTumbler();
        ElemIndicator ei = new ElemIndicator();
        ElemSpinner es = new ElemSpinner();
        ElemJoystick ej = new ElemJoystick();

        ConfigElem ce;

        XmlTextReader xmlrd = null;
        try
        {
            xmlrd = new XmlTextReader(ports_configuration_path);
            xmlrd.WhitespaceHandling = WhitespaceHandling.None;

            while (xmlrd.Read())
            {
                switch (xmlrd.NodeType)
                {
                    case XmlNodeType.Element:
                        if (xmlrd.Name == "controller")
                        {
                            prev_elem = "controller";
                            Debug.Log("Тэг контроллера найден");
                            if (xmlrd.HasAttributes)
                                controller_name = xmlrd.GetAttribute("name");
                        }
                        else
                        if (xmlrd.Name == "ip_address")
                        {
                            prev_elem = "ip_address";
                            Debug.Log("Тэг адреса устройства найден");
                            if (xmlrd.HasAttributes)
                                ip_address = xmlrd.GetAttribute("address");
                        }
                        else
                        if (xmlrd.Name == "tcp_port")
                        {
                            prev_elem = "tcp_port";
                            Debug.Log("Тэг адреса порта устройства найден");
                            if (xmlrd.HasAttributes)
                                tcp_port = xmlrd.GetAttribute("port");
                        }
                        else
                        if (xmlrd.Name == "port_device")
                        {
                            prev_elem = "port_device";
                            Debug.Log("Тэг адреса порта на устройстве найден");
                            if (xmlrd.HasAttributes)
                            {
                                ip_address = xmlrd.GetAttribute("id");
                                string type = xmlrd.GetAttribute("type");
                                pd = new PortDevice();
                                pdList.Add(pd);
                            }
                        }
                        else
                        if (xmlrd.Name == "open_ports")
                        {
                            prev_elem = "open_ports";
                            Debug.Log("Тэг списка открытых портов найден");
                        }
                        else
                        if (xmlrd.Name == "block_element")
                        {
                            prev_elem = "block_element";
                            Debug.Log("Тэг блока управления");
                            if (xmlrd.HasAttributes)
                            {
                                curr_xml_text = xmlrd.GetAttribute("name");
                                bc = new BlockControl();
                                bc.Name = curr_xml_text;
                                bcList.Add(bc);
                            }
                        }
                        else
                        if (xmlrd.Name == "toolkit_element")
                        {
                            prev_elem = "toolkit_element";
                            Debug.Log("Тэг элемента управления");
                            if (xmlrd.HasAttributes)
                            {
                                string name = xmlrd.GetAttribute("name");
                                string description = xmlrd.GetAttribute("description");
                                if (xmlrd.GetAttribute("type") == "tumbler")
                                {
                                    et = new ElemTumbler();
                                    et.ElemName = name;
                                    et.Description = description;
                                    bcList[bcList.LastIndexOf(bc)].etList.Add(et);
                                }
                                if (xmlrd.GetAttribute("type") == "indicator")
                                {
                                    ei = new ElemIndicator();
                                    ei.ElemName = name;
                                    ei.Description = description;
                                    bcList[bcList.LastIndexOf(bc)].eiList.Add(ei);
                                }
                                if (xmlrd.GetAttribute("type") == "spinner")
                                {
                                    es = new ElemSpinner();
                                    es.ElemName = name;
                                    es.Description = description;
                                    bcList[bcList.LastIndexOf(bc)].esList.Add(es);
                                }
                                if (xmlrd.GetAttribute("type") == "joystick")
                                {
                                    ej = new ElemJoystick();
                                    ej.ElemName = name;
                                    ej.Description = description;
                                    bcList[bcList.LastIndexOf(bc)].ejList.Add(ej);
                                }
                            }
                        }
                        if (xmlrd.Name == "port")
                        {
                            prev_elem = "port";
                            Debug.Log("Тэг соответствия порта и элемента управления");
                            if (xmlrd.HasAttributes)
                            {
                                string id = xmlrd.GetAttribute("id");
                                string 
                                bc = new BlockControl();
                                bc.Name = curr_xml_text;
                                bcList.Add(bc);

                                if (prev_elem == "port")

                            }
                        }
                        else {
                            prev_elem = "another_element";
                            Debug.Log("Посторонний тэг"); //в документе встретился обычный элемент, который не относится к контроллерам, либо портам
                        }
                        break;
                    case XmlNodeType.XmlDeclaration:
                        Debug.Log("Тэг декларации найден"); //соответствует началу любой правильной xml-разметки
                        xmldeclar = true;
                        break;
                    case XmlNodeType.EndElement:
                        Debug.Log("Достигнут тэг завершения "+xmlrd.Name);
                        break;
                }
            }
        }
        catch (IOException io)
        {
            Debug.Log("Загрузка xml - файла по заданному пути не выполнена");
        }
        catch (XmlException xe)
        {
            Debug.Log("Ошибка в построении xml - дерева");
            controller_inited = false;
            return false;
        }
        finally { }

        //тэги подключенных портов
        checkLivePorts();

        if (!controller_inited)
        { return false; }
        else return true;

    }

    static bool checkLivePorts()
    {
        return true;
    }

    static void initPortsConfig()
    {

    }
}
