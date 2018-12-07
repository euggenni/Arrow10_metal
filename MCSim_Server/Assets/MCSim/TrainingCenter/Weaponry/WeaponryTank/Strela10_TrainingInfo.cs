using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using MilitaryCombatSimulator;

public class Strela10_TrainingInfo : MCSTrainingInfo
{
    List<MCSTrainingOrder> orders = new List<MCSTrainingOrder>();

    public Strela10_TrainingInfo()
    {
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "� ���", new List<OrderSubtask>()
                                                                                        {
            new OrderSubtask("����� �������� � ������� ��������� �� ����������� ���������","Strela10_SoundPanel", "SPINNER_SOUNDCO", "50"),
            new OrderSubtask("����� ���� � ������� ��������� �� ����������� ���������","Strela10_SoundPanel", "SPINNER_SPEECH", "180"),
            new OrderSubtask("����� ���� � ������� ��������� �� ����������� ���������","Strela10_SoundPanel", "SPINNER_AMPLIFIER", "180"),
            new OrderSubtask("�� ������ ������������� ���������� ������ ���� ������� ��������-������ � ��������� ��������","Strela10_OperationalPanel", "TUMBLER_MODE", "TRAINING"),
            new OrderSubtask("�� ������ ������������� ���������� ������ ���� ������� ���-���� � ��������� ����","Strela10_OperationalPanel", "TUMBLER_AOZ", "OFF"),
            new OrderSubtask("�� ������ ��������� ������� ��� � ��������� ���","Strela10_OperatorPanel", "TUMBLER_AOZ", "ON"),
            new OrderSubtask("�� ������ ��������� ������� ������-����-������ � ��������� ����","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "OFF"),
            new OrderSubtask("�� ������ ��������� ������������� ��� � ��������� ���-1","Strela10_OperatorPanel", "TUMBLER_FON", "STATE_1"),
            new OrderSubtask("�� ������ ��������� ������� ����� � ��������� �������","Strela10_OperatorPanel", "TUMBLER_MODE", "TRAINING"),
            new OrderSubtask("�� ������ ��������� ����������� ���-������ � ��������� ����","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "AUTO"),
            new OrderSubtask("�� ������ ��������� ����� ������� � ������ ���������","Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"),
            new OrderSubtask("�� ������ ��������� ������� ��� � ��������� ��","Strela10_OperatorPanel", "TUMBLER_PSP", "OFF"),
            new OrderSubtask("�� ������ ��������� ������� ����� ��� � ��������� ��","Strela10_OperatorPanel", "TUMBLER_PSP_MODE", "NP"),
            new OrderSubtask("�� ������ ��������� ������� ��� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_SS", "ON"),
            new OrderSubtask("�� ������ ��������� ������� �������� ��� �  ��������� �����","Strela10_OperatorPanel", "TUMBLER_TEST", "ON"),
            new OrderSubtask("�� ������ ��������� ������� 24� �  ��������� ����","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "OFF"),
            new OrderSubtask("�� ������ ��������� ������� 28� �  ��������� ����","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ���������� � ��������� ����","Strela10_SupportPanel", "TUMBLER_FAN", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ������� ������ � ��������� ����","Strela10_SupportPanel", "TUMBLER_GLASS_HEATING", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ��������� � ��������� ����","Strela10_SupportPanel", "TUMBLER_LIGHT", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ������� � ��������� ��������","Strela10_SupportPanel", "TUMBLER_POSITION", "STOWED"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� �������� � ��������� ������","Strela10_SupportPanel", "TUMBLER_TRACKING", "MANUAL"),
            new OrderSubtask("�� ����� �������� ������ ��������� ������� ���������� � ��������� ����","Strela10_GuidancePanel", "TUMBLER_COOL", "OFF"),
            new OrderSubtask("�� ��������� ������� ������� � � ��������� ����","Strela10_AzimuthIndicator", "TUMBLER_C", "OFF"),
            new OrderSubtask("�� ��������� ������� ������� �����  � ��������� ����","Strela10_AzimuthIndicator", "TUMBLER_BACKLIGHT", "OFF"),
            new OrderSubtask("�� ��������� ������� ����� ������� � ������� ���������","Strela10_AzimuthIndicator", "SPINNER_AMPLIFIER", "180"),
            new OrderSubtask("�� ���������� ������ 9�127� �������������� ������ ������ ������, �������� ������������� ������������� � ��������� ����������� ������","Strela10_VizorPanel", "TUMBLER_FILTER_UP", "CENTER"),
            new OrderSubtask("������� ���������  - ��������� ����","Strela10_VizorPanel", "TUMBLER_ILLUM", "OFF"),
            new OrderSubtask("������ ������� � ����������� ��� ���� ���������; �������� �������� ������ �������� ������ ������ - � ��������� ������� ","Strela10_CommonPanel", "HANDL", "OFF"),
            new OrderSubtask("�� ������ 9�127� �������� ������������ ��������� - � ��������� 1,8*","Strela10_VizorPanel", "TUMBLER_FILTER_DOWN", "X_2"),
            new OrderSubtask("�� ������ ���  ������� ��","Strela10_VizorPanel","TUMBLER_VV","ON"),
            new OrderSubtask("�� ������ ���  ������� ��","Strela10_VizorPanel","TUMBLER_��","ON"),
            new OrderSubtask("�� ������ ���  ������� ��","Strela10_VizorPanel","TUMBLER_��2","ON"),
            new OrderSubtask("�� ����� ��������� ��� ������� ������� - � ��������� ����","Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "OFF"),
            new OrderSubtask("������������� ����� - � ���������, �������������� ������������ ����������","Strela10_VizorPanel", "TUMBLER_CODE_POSITION", "P_0"),
                
            new OrderSubtask("�� ������ ���������� ������� ������������ ����� ������� � ������� ���������","Strela10_ControlBlock", "SPINNER_BRIGHTNESS", "50"),
            new OrderSubtask("�� ������ ���������� ������� ������������ ������������� ������-�������� I-�������� II � ��������� ������","Strela10_ControlBlock", "TUMBLER_MODE", "WORK"),
            // �� ����� ������� 1 2 ���������� 3 � 4 
            new OrderSubtask("�� ������ ���������� ������� ������������ ������������� ������� III � ��������� ���","Strela10_ControlBlock", "TUMBLER_STAGE_3", "ON"),
            new OrderSubtask("�� ������ ���������� ������� ������������ ������������� ������� IV � ��������� ���","Strela10_ControlBlock", "TUMBLER_STAGE_4", "ON"),
            new OrderSubtask("�������� ������� ����� �� ������� � ��������� �����������","Strela10_CommonPanel", "PEDAL_AZIM", "OFF"),
            new OrderSubtask("����� ������� ������� ������ �� ������ ��������� ������ �������� 24�","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("������� 28� � ��������� ���, ��� ���� ���������� ����� �����, � ��� ������� �� ������ ����� ���������� ����� �������� ��, ��� I,II","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("������������ ���������� 28� �� ��������������� �� ��, ��� ���� ������� ������ �����.30 �� �� , ����������� ������ �����","Strela10_OperatorPanel", "TUMBLER_POWER_30B", "ON"),
            new OrderSubtask("�� ������ ��������� ������� ������ -������ ������������� � ��������� ������","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),
            new OrderSubtask("������� ������� ������������� � ��������� ������","Strela10_OperatorPanel", "TUMBLER_MODE", "COMBAT"),
            new OrderSubtask("��������� �������� ������ ������ 9�127� � ��������� ������� ","Strela10_CommonPanel", "HANDL", "ON"),
            
                                                                                        }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "��������. ��������", new List<OrderSubtask>()
            
        {
            new OrderSubtask("�� ������ ��������� ������� 24� �  ��������� ���", "Strela10_OperatorPanel",
                "TUMBLER_POWER_24B", "ON"), //��-�� ����, ��� �������� ��� ��������, 2 ������� ������������
            new OrderSubtask("�� ������ ��������� ������� 28� �  ��������� ���", "Strela10_OperatorPanel",
                "TUMBLER_POWER_28B", "ON"), //��� �����
            new OrderSubtask(
                "������������ �� ���������� ������� 30 � ��� ���� �������� �������� 30 �, ������� ������ ���������� � ������� ���� �������",
                "Strela10_OperatorPanel", "TUMBLER_POWER_30B", "ON"),
            new OrderSubtask(
                "������� ������ -������ ������������� � ��������� ������. ������� ������� �� ������ �������� ������ ��������� � ��������� ��������� �������� ������ ��������� �� ������� � ���� �����",
                "Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),
            new OrderSubtask("��������� ��� ���� ����� ������ 3-30 ��������� ����� ��� � ������", "Strela10_VizorPanel",
                "TUMBLER_LOSE", "ON"),

            new OrderSubtask(
                "������������� ������������� ��� ������ � ��������� 1, ��� ������� ����� ���������� ��������������� ������������",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_1"),
            new OrderSubtask(
                "������������� ������������� ��� ������ � ��������� 2, ��� ������� ����� ���������� ��������������� ������������",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_2"),
            new OrderSubtask(
                "������������� ������������� ��� ������ � ��������� 3, ��� ������� ����� ���������� ��������������� ������������",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_3"),
            new OrderSubtask(
                "������������� ������������� ��� ������ � ��������� 4, ��� ������� ����� ���������� ��������������� ������������",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_4"),
            new OrderSubtask(
                "������������� ������������� ��� ������ � ��������� 3, ��� ������� ����� ���������� ��������������� ������������",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_3"),
            new OrderSubtask(
                "������������� ������������� ��� ������ � ��������� 2, ��� ������� ����� ���������� ��������������� ������������",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_2"),
            new OrderSubtask(
                "������������� ������������� ��� ������ � ��������� 1, ��� ������� ����� ���������� ��������������� ������������",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_1"),
            new OrderSubtask(
                "������������ ����������� ����������� ����������� � ������� ���������� ��� �� ��������� ���� �������� �� ��� I II. ������������� ��� ������ ������������� � ��������� ���",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "AUTO"),

            new OrderSubtask(
                "������ ������ ���� � ��������� ��, � ��������� ������ ���� ��������������� � ������������� ������ ���, � �� ������  ���������� ����������� ����",
                "Strela10_GuidancePanel", "TUMBLER_BOARD", "ON"),
            new OrderSubtask(
                "������ ������ ���� � ��������� ��, � ��������� ������ ���� ��������������� � ������������� ������ ���, � �� ������  ���������� ����������� ����",
                "Strela10_GuidancePanel", "TUMBLER_BOARD", "OFF"),


            new OrderSubtask("���������� ���� ������ ������������ ���������� ����������� ����", "Strela10_SoundPanel",
                "SPINNER_SOUNDCO", "50"), //��� ���������� �����
            new OrderSubtask(
                "�� ������ ��������� ����� ������� � ������ ���������, ����� 2 - 3 ������� ����� ������� ������ ���� � ���� ������ ������ �������� ����� ��������������� �� ������ ������ ����������� ��� �������� ���.",
                "Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"), //��� ���������� �����
            new OrderSubtask("�� ������� 1�2-3 �������� � ��������� ������ ��� ��� ���� ���������� ����� ���",
                "Strela10_ControlBlock", "TUMBLER_STATUS", "ON"), //��� ���������� �����
            new OrderSubtask(
                "������������� ������������� ������ - �������� I - �������� II � ��������� �������� I. �� ��������� ������� ������ �������������� ������ � ����� � ����������� ����.",
                "Strela10_ControlBlock", "TUMBLER_MODE", "CHECK_I"), //��� ���������� �����
            new OrderSubtask(
                "������������� ������������� ������ - �������� I - �������� II � ��������� �������� II ��� ���� ������ ����������� ���� ������ � ����� �������, � ���������� ������ � ����� ������� � ���� ������ ������",
                "Strela10_ControlBlock", "TUMBLER_MODE", "CHECK_II"), //��� ���������� �����
            new OrderSubtask("������������� ������������� ������ - �������� I - �������� II � ��������� ������",
                "Strela10_ControlBlock", "TUMBLER_MODE", "WORK"), //��� ���������� �����
            new OrderSubtask(
                "������� �������� ��������� �� ������� ���� � ������������� ����� �������� �������� - ���� �� ������� �����, � ��������� ��������� � ������� �����, ��� ������� ��� �� ��������� � ����� �������� �� ������������ ���������� ������������� ����.",
                "Strela10_VizorPanel", "TUMBLER_LOSE", "ON"),
            new OrderSubtask(
                "������� �������� ��������� �� ������� ���� � ������������� ����� �������� �������� - ���� �� ������� �����, � ��������� ��������� � ������� �����, ��� ������� ��� �� ��������� � ����� �������� �� ������������ ���������� ������������� ����.",
                "Strela10_OperatorPanel", "TUMBLER_FON", "STATE_2"),
            new OrderSubtask(
                "���� ��� ������ ��������� � ����� �������� � ��������� ���-2 �� ����� ������ ���������� ����������� � ������������ ������",
                "Strela10_VizorPanel", "TUMBLER_LOSE", "ON"),
            new OrderSubtask(
                "���� ��� ������ ��������� � ����� �������� � ��������� ���-2 �� ����� ������ ���������� ����������� � ������������ ������",
                "Strela10_GuidancePanel", "TUMBLER_COOL", "ON"),


            new OrderSubtask("������� ����� ���������� � ������", "Strela10_OperatorPanel", "TUMBLER_MODE", "COMBAT"),
            new OrderSubtask(
                "������� ���-���� �� ������ ������������ ���������� ���������� � ��������� ���, ��� ���� �������� �������� ��������������� ��-500� � ���������� ����� ��� ��� �� ������ ������������ ����������, �  �� ������� � - ����� �����. ����. ����� 180+-20 � �� ������ ������������ ���������� ���������� ���������� ����� �������� ",
                "Strela10_OperationalPanel", "TUMBLER_AOZ", "ON"), //��� ���������� �����
            new OrderSubtask(
                "��� ��������� � ������� ������ ������: ��������������� ������������� ������� ������ �� ������� �  � ��������� �������� ���, ��� ���������� ������ ���������� ���������� ����� ����� �����.",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_7"), //��� ���������� �����
            new OrderSubtask(
                "���������� ������ ���� ��������� � �������� ������: ������� �������� - ������ �� ������ ������������ ���������� ��������������� � ��������� ������, ��� ���� ���������� ����� �������� -���������, ������ ���������� ����� �������� � ����� �����. �� ������� �",
                "Strela10_OperationalPanel", "TUMBLER_MODE", "COMBAT"), //��� ���������� �����
            new OrderSubtask(
                "��������� ������� ������������ ����� 115� �� �������� ������ ������� � ��������������� ������ ��������� ���������� ��������������� �� - 500�. ��������� ��������� �������� ���� ��������� �� �������� ������ ������� �: - �� ������� ������ -0,15-0.7��; - �� ������� ��� 0,1-1,0 ��; ��������� ��������� ������� ��� ���� �� ����������� �������� ��������� �� �������� ������� ���� �������� 1,25��",
                "Strela10_VizorPanel", "TUMBLER_LOSE", "ON"), //��� ���������� �����
            new OrderSubtask(
                "�������� �� ����� ������ �������� ��� �� �������� ������ ������� � ��� ���� ��������� ������� ��� ���� ������ ���� � �������� 3,5-7,0 ��",
                "Strela10_VizorPanel", "TUMBLER_YPCH", "ON"), //��� ���������� �����


            new OrderSubtask(
                "�������� ������ �������� �� �� �������� ������ ������� � ��� ���� ���������� ���������� ����� ���� � �����",
                "Strela10_VizorPanel", "TUMBLER_VZ", "ON"), //��� ���������� �����
            new OrderSubtask(
                "������������� ������������� ������� ������ � ��������� �������� ����� � �������� �������� �� ��� ���� ���������� ����� ����� ������� � ������ ����� �����.����. ��� ��������� ����� � ������������� ������ ���������� ����� ���� � ����� �� �����",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_6"), //��� ���������� �����
            new OrderSubtask(
                "������������� ������������� ������� ������ � ��������� �������� ����� � �������� �������� �� ��� ���� ���������� ����� ����� ������� � ������ ����� �����.����. ��� ��������� ����� � ������������� ������ ���������� ����� ���� � ����� �� �����",
                "Strela10_VizorPanel", "TUMBLER_SD", "ON"), //��� ���������� �����

            new OrderSubtask(
                "������������� ������������� ������� ������ � ��������� �������� ��  ��� ���� ���������� ����� �������� ��",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_8"), //��� ���������� �����
            new OrderSubtask(
                "������������� ������������� ������� ������ � ���� �� ��������� ��������� +30 - +50 ��� ����������� ���������� ����� +50 : +30; -10 : + 30 ��� ����������� ���������� ����� +30 : -10 ; -50 : -10 ��� ����������� �� -10 �� -50; ������ - ��� ������� ���� ��� ����� ����������� ",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_2"),//��� ���������� �����
            new OrderSubtask("������� �������� - ������ � ��������� ��������", "Strela10_OperationalPanel",
                "TUMBLER_MODE", "TRAINING"), //��� ���������� �����
            new OrderSubtask("������� ��� � ��������� ����", "Strela10_OperatorPanel", "TUMBLER_AOZ", "OFF"),
            new OrderSubtask(
                "��������� ������ ��������� ����������������� ����������, ������������� ������� ���� �� ������ 1�� � ��������� ������� ��� ���� ���������� ��������� �����",
                "Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "ON"), //��� ���������� �����
            new OrderSubtask(
                "������������� ������������� 0-1-2-3-4-5-6 �� ����� 1�� ���������� ��������������� ������������ ���������� �����",
                "Strela10_VizorPanel", "TUMBLER_CODE_POSITION", "P_0"), //��� ���������� �����
            new OrderSubtask(
                "����� 3 ��� �������� ���������� ������ ������ � �������� - ���� �� ������� ����� � �� ��������� ���������� ���������� � ���� ��������� 9�24 �� �������� ����� , ������������� �� ������ 9�127 ���������� � ����������� ���������� ���",
                "Strela10_GuidancePanel", "TUMBLER_TRACK_LAUNCH", "LAUNCH"), 

            new OrderSubtask(
                "������������� ������� ������� �� ����� 9�23 � ��������� �����.(���� � ����������� �� ��������� ���������)",
                "Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"), //��� ���������� �����
            new OrderSubtask("��������� ������� ������� �� ������ ���������� ���", "Strela10_VizorPanel",
                "TUMBLER_POWER_NRZ", "OFF"), //��� ���������� �����
            new OrderSubtask(
                "�������� � ��������� ������ ���� �� ����� �������� ������ ���������, ��� ���� ���������� ��������� ����� �� ������ ���������� ���",
                "Strela10_GuidancePanel", "TUMBLER_BOARD", "ON")
           }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "���������� �1", new List<OrderSubtask>()
                                                                                   {
            new OrderSubtask("�� ������ ��������� ������� 24� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("�� ������ ��������� ������� 28� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("��������� ������� ������� � ����� ����� �� ������������� 1","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_1"),
            new OrderSubtask("��������� ������� ������� � ����� ����� �� ������������� 2","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_2"),
            new OrderSubtask("��������� ������� ������� � ����� ����� �� ������������� 3","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_3"),
            new OrderSubtask("��������� ������� ������� � ����� ����� �� ������������� 4","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_4"),
            new OrderSubtask("��������� ������� �������� ������������ ��� ��� � ����� �� ","Strela10_ARC", "TUMBLER_A3", "ON"), //��� ���������� �����
            new OrderSubtask("�������� ������� ��� �� ������ ������������ ����������","Strela10_OperatorPanel", "TUMBLER_AOZ", "ON"), //��� ���������� �����
            new OrderSubtask("����� ��������� ����� �������� - ������ ������� �������� ��������� � ��������� ������","Strela10_OperationalPanel", "TUMBLER_MODE", "COMBAT"), //��� ���������� �����
            new OrderSubtask("������� ����� ������������� � ��������� ������","Strela10_OperatorPanel", "TUMBLER_MODE", "COMBAT"),
            new OrderSubtask("��������� ������ �������� ������ ������ 9�127�","Strela10_CommonPanel", "HANDL", "ON"), //��� ���������� �����
            new OrderSubtask("������� �� �� ������� �� �������","Strela10_CommonPanel", "PEDAL_AZIM", "OFF"), //��� ���������� �����
            new OrderSubtask("�������� ������ (�� �� ������� ������ ������������� ��������� ������) � ��������� ����������� �������� �� �� ������� � ���� �����. ����������� ������ �����","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),

                                                                                   }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "���������� �2", new List<OrderSubtask>()
                                                                                   {
            new OrderSubtask("�� ������ ��������� ������� 24� �  ��������� ����","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "OFF"),
            new OrderSubtask("�� ������ ��������� ������� 28� �  ��������� ����","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "OFF"),
            new OrderSubtask("������� ������ - ���� - ������ � ��������� ����","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "OFF"),
            new OrderSubtask("�� ������ ������������ ���������� ������� ������ - �������� � ��������� �������� ","Strela10_OperationalPanel", "TUMBLER_MODE", "TRAINING"), //��� ���������� �����
            new OrderSubtask("������� ��� � ��������� ����","Strela10_OperatorPanel", "TUMBLER_AOZ", "OFF"),
            new OrderSubtask("�� ������ ���������� ��� ������� ������� � ���������� ����. ����������� ������ �����","Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "OFF"), //��� ���������� �����
                                                                                   }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "�������� ���������", new List<OrderSubtask>(){
            new OrderSubtask("������� ���.� � ���������, ��������������� ���������� ������.","Strela10_VizorPanel","TUMBLER_POSITION_WORK_TYPE","POS_2"), //��� ���������� �����
            new OrderSubtask("����� �������� � ������� ��������� �� ����������� ���������","Strela10_SoundPanel", "SPINNER_SOUNDCO", "50"), //��� ���������� �����
            new OrderSubtask("����� ���� � ������� ��������� �� ����������� ���������","Strela10_SoundPanel", "SPINNER_SPEECH", "180"), //��� ���������� �����
            new OrderSubtask("����� ���� � ������� ��������� �� ����������� ���������","Strela10_SoundPanel", "SPINNER_AMPLIFIER", "180"), //��� ���������� �����
            new OrderSubtask("�� ������ ������������� ���������� ������ ���� ������� ��������-������ � ��������� ��������","Strela10_OperationalPanel", "TUMBLER_MODE", "TRAINING"), //��� ���������� �����
            new OrderSubtask("�� ������ ������������� ���������� ������ ���� ������� ���-���� � ��������� ����","Strela10_OperationalPanel", "TUMBLER_AOZ", "OFF"), //��� ���������� �����
            new OrderSubtask("�� ������ ��������� ������� ��� � ��������� ���","Strela10_OperatorPanel", "TUMBLER_AOZ", "ON"),
            new OrderSubtask("�� ������ ��������� ������� ������-����-������ � ��������� ����","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "OFF"),
            new OrderSubtask("�� ������ ��������� ������������� ��� � ��������� ���-1","Strela10_OperatorPanel", "TUMBLER_FON", "STATE_1"),
            new OrderSubtask("�� ������ ��������� ������� ����� � ��������� �������","Strela10_OperatorPanel", "TUMBLER_MODE", "TRAINING"),
            new OrderSubtask("�� ������ ��������� ����������� ���-������ � ��������� ����","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "AUTO"),
            new OrderSubtask("�� ������ ��������� ����� ������� � ������ ���������","Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"), //��� ���������� �����
            new OrderSubtask("�� ������ ��������� ������� ��� � ��������� ��","Strela10_OperatorPanel", "TUMBLER_PSP", "OFF"),
            new OrderSubtask("�� ������ ��������� ������� ����� ��� � ��������� ��","Strela10_OperatorPanel", "TUMBLER_PSP_MODE", "NP"),
            new OrderSubtask("�� ������ ��������� ������� ��� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_SS", "ON"), //��� ���������� �����
            new OrderSubtask("�� ������ ��������� ������� �������� ��� �  ��������� �����","Strela10_OperatorPanel", "TUMBLER_TEST", "ON"), //��� ���������� �����
            new OrderSubtask("�� ������ ��������� ������� 24� �  ��������� ����","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "OFF"),
            new OrderSubtask("�� ������ ��������� ������� 28� �  ��������� ����","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ���������� � ��������� ����","Strela10_SupportPanel", "TUMBLER_FAN", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ������� ������ � ��������� ����","Strela10_SupportPanel", "TUMBLER_GLASS_HEATING", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ��������� � ��������� ����","Strela10_SupportPanel", "TUMBLER_LIGHT", "OFF"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ������� � ��������� ��������","Strela10_SupportPanel", "TUMBLER_POSITION", "STOWED"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� �������� � ��������� ������","Strela10_SupportPanel", "TUMBLER_TRACKING", "MANUAL"),
            new OrderSubtask("�� ����� �������� ������ ��������� ������� ���������� � ��������� ����","Strela10_GuidancePanel", "TUMBLER_COOL", "OFF"),
            new OrderSubtask("�� ��������� ������� ������� � � ��������� ����","Strela10_AzimuthIndicator", "TUMBLER_C", "OFF"), //��� ���������� �����
            new OrderSubtask("�� ��������� ������� ������� �����  � ��������� ����","Strela10_AzimuthIndicator", "TUMBLER_BACKLIGHT", "OFF"), //��� ���������� �����
            new OrderSubtask("�� ��������� ������� ����� ������� � ������� ���������","Strela10_AzimuthIndicator", "SPINNER_AMPLIFIER", "180"), //��� ���������� �����
            new OrderSubtask("�� ���������� ������ 9�127� �������������� ������ ������ ������, �������� ������������� ������������� � ��������� ����������� ������","Strela10_VizorPanel", "TUMBLER_FILTER_UP", "CENTER"), //��� ���������� �����
            new OrderSubtask("������� ���������  - ��������� ����","Strela10_VizorPanel", "TUMBLER_ILLUM", "OFF"), //��� ���������� �����
            new OrderSubtask("������ ������� � ����������� ��� ���� ���������; �������� �������� ������ �������� ������ ������ - � ��������� ������� ","Strela10_CommonPanel", "HANDL", "OFF"), //��� ���������� �����
            new OrderSubtask("�� ������ 9�127� �������� ������������ ��������� - � ��������� 1,8*","Strela10_VizorPanel", "TUMBLER_FILTER_DOWN", "X_2"), //��� ���������� �����
            new OrderSubtask("�� ������ ���  ������� ��","Strela10_VizorPanel","TUMBLER_VV","ON"), //��� ���������� �����
            new OrderSubtask("�� ������ ���  ������� ��","Strela10_VizorPanel","TUMBLER_��","ON"), //��� ���������� �����
            new OrderSubtask("�� ������ ���  ������� ��","Strela10_VizorPanel","TUMBLER_��2","ON"), //��� ���������� �����
             new OrderSubtask("�� ����� ��������� ��� ������� ������� - � ��������� ����","Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "OFF"), //��� ���������� �����
             new OrderSubtask("������������� ����� - � ���������, �������������� ������������ ����������","Strela10_VizorPanel", "TUMBLER_CODE_POSITION", "P_0"), //��� ���������� �����
                
            new OrderSubtask("�� ������ ���������� ������� ������������ ����� ������� � ������� ���������","Strela10_ControlBlock", "SPINNER_BRIGHTNESS", "50"), //��� ���������� �����
            new OrderSubtask("�� ������ ���������� ������� ������������ ������������� ������-�������� I-�������� II � ��������� ������","Strela10_ControlBlock", "TUMBLER_MODE", "WORK"), //��� ���������� �����
            new OrderSubtask("�� ������ ���������� ������� ������������ ������������� ������� III � ��������� ���","Strela10_ControlBlock", "TUMBLER_STAGE_3", "ON"), //��� ���������� �����
            new OrderSubtask("�� ������ ���������� ������� ������������ ������������� ������� IV � ��������� ���","Strela10_ControlBlock", "TUMBLER_STAGE_4", "ON"), //��� ���������� �����
            new OrderSubtask("�������� ������� ����� �� ������� � ��������� �����������","Strela10_CommonPanel", "TUMBLER_STOP", "ON")  //��� ���������� �����
        }));

        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "�������� �������", new List<OrderSubtask>(){
            new OrderSubtask("�� ������ ��������� ������� 24� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("�� ������ ��������� ������� 28� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("�� ������ ��������� ������� 30� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_POWER_30B", "ON")
        }));

        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "������� ��", new List<OrderSubtask>(){
            new OrderSubtask("�� ������ ��������� ������� 24� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("�� ������ ��������� ������� 28� �  ��������� ���","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("�� ������ ��������� ������� ������-����-������ � ��������� ������","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),
            new OrderSubtask("�� ������ ��������� ��-2 ������� ������� � ��������� ������","Strela10_SupportPanel", "TUMBLER_POSITION", "BATTLE"),
            new OrderSubtask("����� �������� ��������� �� �������","Strela10_CommonPanel", "PEDAL_AZIM", "ON")
            //new OrderSubtask("����������� �� �� �������","Strela10_CommonPanel", "TUMBLER_STOP", "OFF")  //��������� team: ������ ��� ������ � ������� (���������� �����)
            }));
    }

    public override List<MCSTrainingOrder> GetAllOrders()
    {
        //orders[1].OrderName.ToString()
        return orders;
    }
}