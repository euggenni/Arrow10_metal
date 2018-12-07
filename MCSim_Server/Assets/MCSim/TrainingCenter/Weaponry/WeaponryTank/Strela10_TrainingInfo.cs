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
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "К БОЮ", new List<OrderSubtask>()
                                                                                        {
            new OrderSubtask("Ручка УСИЛЕНИЕ в среднее положение на суммирующем усилителе","Strela10_SoundPanel", "SPINNER_SOUNDCO", "50"),
            new OrderSubtask("Ручка РЕЧЬ в среднее положение на суммирующем усилителе","Strela10_SoundPanel", "SPINNER_SPEECH", "180"),
            new OrderSubtask("Ручка ЗВУК в среднее положение на суммирующем усилителе","Strela10_SoundPanel", "SPINNER_AMPLIFIER", "180"),
            new OrderSubtask("На пульте оперативноего управления оценке зоны тумблер ДЕЖУРНЫЙ-БОЕВОЙ в положение ДЕЖУРНЫЙ","Strela10_OperationalPanel", "TUMBLER_MODE", "TRAINING"),
            new OrderSubtask("На пульте оперативноего управления оценке зоны тумблер ВКЛ-ВЫКЛ в положение ВЫКЛ","Strela10_OperationalPanel", "TUMBLER_AOZ", "OFF"),
            new OrderSubtask("На пульте оператора тумблер АОЗ в положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_AOZ", "ON"),
            new OrderSubtask("На пульте оператора тумблер ПРИВОД-ВЫКЛ-РУЧНОЕ в положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "OFF"),
            new OrderSubtask("На пульте оператора переключатель ФОН в положение ФОН-1","Strela10_OperatorPanel", "TUMBLER_FON", "STATE_1"),
            new OrderSubtask("На пульте оператора тумблер РЕЖИМ в положение УЧЕБНЫЙ","Strela10_OperatorPanel", "TUMBLER_MODE", "TRAINING"),
            new OrderSubtask("На пульте оператора переключать РОД-РАБОТЫ в положение АВТО","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "AUTO"),
            new OrderSubtask("На пульте оператора ручку ЯРКОСТЬ в сренее положение","Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"),
            new OrderSubtask("На пульте оператора тумблер ПСП в положение БП","Strela10_OperatorPanel", "TUMBLER_PSP", "OFF"),
            new OrderSubtask("На пульте оператора тумблер РЕЖИМ ПСП в положение НП","Strela10_OperatorPanel", "TUMBLER_PSP_MODE", "NP"),
            new OrderSubtask("На пульте оператора тумблер СОС в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_SS", "ON"),
            new OrderSubtask("На пульте оператора тумблер ЗАДЕРЖКА НВУ в  положение ОСНОВ","Strela10_OperatorPanel", "TUMBLER_TEST", "ON"),
            new OrderSubtask("На пульте оператора тумблер 24В в  положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "OFF"),
            new OrderSubtask("На пульте оператора тумблер 28В в  положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ВЕНТИЛЯТОР в положение ВЫКЛ","Strela10_SupportPanel", "TUMBLER_FAN", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ОБОГРЕВ СТЕКЛА в положение ВЫКЛ","Strela10_SupportPanel", "TUMBLER_GLASS_HEATING", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ОСВЕЩЕНИЕ в положение ВЫКЛ","Strela10_SupportPanel", "TUMBLER_LIGHT", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ПЕРЕВОД в положение ПОХОДНОЕ","Strela10_SupportPanel", "TUMBLER_POSITION", "STOWED"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер СЛЕЖЕНИЕ в положение РУЧНОЕ","Strela10_SupportPanel", "TUMBLER_TRACKING", "MANUAL"),
            new OrderSubtask("На левой рукоятке пульта наведения тумблер ОХЛАЖДЕНИЕ в положение ВЫКЛ","Strela10_GuidancePanel", "TUMBLER_COOL", "OFF"),
            new OrderSubtask("На указателе азимута тумблер С в положение ВЫКЛ","Strela10_AzimuthIndicator", "TUMBLER_C", "OFF"),
            new OrderSubtask("На указателе азимута тумблер ШКАЛА  в положение ВЫКЛ","Strela10_AzimuthIndicator", "TUMBLER_BACKLIGHT", "OFF"),
            new OrderSubtask("На указателе азимута ручку ЯРКОСТЬ в среднее положение","Strela10_AzimuthIndicator", "SPINNER_AMPLIFIER", "180"),
            new OrderSubtask("На оптическом визире 9Ш127М предварительно открыв крышку визира, рукоятку переключателя светофильтров в положение НЕЙТРАЛЬНЫЙ ФИЛЬТР","Strela10_VizorPanel", "TUMBLER_FILTER_UP", "CENTER"),
            new OrderSubtask("Тумблер ПОДСВЕТКА  - положение ВЫКЛ","Strela10_VizorPanel", "TUMBLER_ILLUM", "OFF"),
            new OrderSubtask("Оправа окуляра в настроенное для глаз положение; рукоятки защитной крышки головной призмы визира - в положение ЗАКРЫТО ","Strela10_CommonPanel", "HANDL", "OFF"),
            new OrderSubtask("На визире 9Ш127М рукоятку переключение кратности - в положение 1,8*","Strela10_VizorPanel", "TUMBLER_FILTER_DOWN", "X_2"),
            new OrderSubtask("На панели АЗС  тумблер ВГ","Strela10_VizorPanel","TUMBLER_VV","ON"),
            new OrderSubtask("На панели АЗС  тумблер ВВ","Strela10_VizorPanel","TUMBLER_СС","ON"),
            new OrderSubtask("На панели АЗС  тумблер ВП","Strela10_VizorPanel","TUMBLER_СС2","ON"),
            new OrderSubtask("На блоке управлние НРЗ тумблер ПИТАНИЕ - в положение ВЫКЛ","Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "OFF"),
            new OrderSubtask("Переключатель кодов - в положение, соответсвующее действующему расписанию","Strela10_VizorPanel", "TUMBLER_CODE_POSITION", "P_0"),
                
            new OrderSubtask("На пульте управление системы пеленгования ручку ЯРКОСТЬ в среднее положение","Strela10_ControlBlock", "SPINNER_BRIGHTNESS", "50"),
            new OrderSubtask("На пульте управление системы пеленгования переключатель РАБОТА-КОНТРОЛЬ I-КОНТРОЛЬ II в положение РАБОТА","Strela10_ControlBlock", "TUMBLER_MODE", "WORK"),
            // не нашел ступени 1 2 естьтолько 3 и 4 
            new OrderSubtask("На пульте управление системы пеленгования переключатель СТУПЕНЬ III в положение ВКЛ","Strela10_ControlBlock", "TUMBLER_STAGE_3", "ON"),
            new OrderSubtask("На пульте управление системы пеленгования переключатель СТУПЕНЬ IV в положение ВКЛ","Strela10_ControlBlock", "TUMBLER_STAGE_4", "ON"),
            new OrderSubtask("Рукоятку стопора башни по азимуту в положение ЗАСТОПОРЕНО","Strela10_CommonPanel", "PEDAL_AZIM", "OFF"),
            new OrderSubtask("После доклада второго номера на пульте оператора ставит тумблеры 24В","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("Тумблер 28В в положение ВКЛ, при этом загорается лампа ПОХОД, а при наличии на постах ракет загорается лампы КОНТРОЛЬ ПЗ, ОХЛ I,II","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("Контролирует напряжение 28В по вольтамперметру на ПО, для чего нажимет кнопку КОНТР.30 на ПО , Докладывает ПЕРВЫЙ ГОТОВ","Strela10_OperatorPanel", "TUMBLER_POWER_30B", "ON"),
            new OrderSubtask("На пульте оператора тумблер ПРИВОД -РУЧНОЕ устанавливает в положение ПРИВОД","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),
            new OrderSubtask("Тумблер ПЕРЕВОД устанавливает в положение БОЕВОЕ","Strela10_OperatorPanel", "TUMBLER_MODE", "COMBAT"),
            new OrderSubtask("Переводит рукоятку крышки визира 9Ш127М в положение открыто ","Strela10_CommonPanel", "HANDL", "ON"),
            
                                                                                        }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "ФУНКЦИОН. КОНТРОЛЬ", new List<OrderSubtask>()
            
        {
            new OrderSubtask("На пульте оператора тумблер 24В в  положение ВКЛ", "Strela10_OperatorPanel",
                "TUMBLER_POWER_24B", "ON"), //из-за того, что тумблеры уже включены, 2 условия пропускаются
            new OrderSubtask("На пульте оператора тумблер 28В в  положение ВКЛ", "Strela10_OperatorPanel",
                "TUMBLER_POWER_28B", "ON"), //тож самое
            new OrderSubtask(
                "Контрилирует по вольтметру наличие 30 В для чего нажимает КОНТРОЛЬ 30 В, Стрелка должна находиться в класном поле допуска",
                "Strela10_OperatorPanel", "TUMBLER_POWER_30B", "ON"),
            new OrderSubtask(
                "Тумблер ПРИВОД -РУЧНОЕ устанавливает в положение ПРИВОД. Нажимае гашетку на правой рукоятке пульта наведения и проверяет плавность вращения пуской установки по азимуту и углу места",
                "Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),
            new OrderSubtask("Проверяет при угле места меньше 3-30 загорание лампы НИЗ в визире", "Strela10_VizorPanel",
                "TUMBLER_LOSE", "ON"),

            new OrderSubtask(
                "Устанавливает переключатель РОД РАБОТЫ в положение 1, при наличии ракет загораются соответствующие транспоранты",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_1"),
            new OrderSubtask(
                "Устанавливает переключатель РОД РАБОТЫ в положение 2, при наличии ракет загораются соответствующие транспоранты",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_2"),
            new OrderSubtask(
                "Устанавливает переключатель РОД РАБОТЫ в положение 3, при наличии ракет загораются соответствующие транспоранты",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_3"),
            new OrderSubtask(
                "Устанавливает переключатель РОД РАБОТЫ в положение 4, при наличии ракет загораются соответствующие транспоранты",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_4"),
            new OrderSubtask(
                "Устанавливает переключатель РОД РАБОТЫ в положение 3, при наличии ракет загораются соответствующие транспоранты",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_3"),
            new OrderSubtask(
                "Устанавливает переключатель РОД РАБОТЫ в положение 2, при наличии ракет загораются соответствующие транспоранты",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_2"),
            new OrderSubtask(
                "Устанавливает переключатель РОД РАБОТЫ в положение 1, при наличии ракет загораются соответствующие транспоранты",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_1"),
            new OrderSubtask(
                "Одновременно кнтролирует исправность пирозапалов в балонах охлаждения ГСН по загоранию ламп КОНТРОЛЬ ПЗ ОХЛ I II. Переключатель РОД РАБОТЫ устанавливает в положение АВТ",
                "Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "AUTO"),

            new OrderSubtask(
                "Нажать кнопку БОРТ и отпускает ее, в наушниках слышен звук сигнализирующий о ракркучивании готора ГСН, а на пульте  загорается транспарант БОРТ",
                "Strela10_GuidancePanel", "TUMBLER_BOARD", "ON"),
            new OrderSubtask(
                "Нажать кнопку БОРТ и отпускает ее, в наушниках слышен звук сигнализирующий о ракркучивании готора ГСН, а на пульте  загорается транспорант БОРТ",
                "Strela10_GuidancePanel", "TUMBLER_BOARD", "OFF"),


            new OrderSubtask("Регулирует звук ручкой потенциомера загорается транспорант БОРТ", "Strela10_SoundPanel",
                "SPINNER_SOUNDCO", "50"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "На пульте оператора ручку ЯРКОСТЬ в сренее положение, через 2 - 3 секунды после нажатия кнопки БОРТ в поле зрения визира следящая марка устанавливается по центру кольца перекрестия или пересает его.",
                "Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На приборе 1Ж2-3 нажимает и отпускает кнопку ВКЛ при этом загорается лампа ВКЛ",
                "Strela10_ControlBlock", "TUMBLER_STATUS", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Устанавливает переключатель РАБОТА - КОНТРОЛЬ I - КОНТРОЛЬ II в положение КОНТРОЛЬ I. На указателя азимута должны подсвечиваться правая и левая и центральный круг.",
                "Strela10_ControlBlock", "TUMBLER_MODE", "CHECK_I"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Устанавливает переключатель РАБОТА - КОНТРОЛЬ I - КОНТРОЛЬ II в положение КОНТРОЛЬ II при этом гаснут центральный круг правая и левая стрелка, и загарается правая и левая стрелка в поле зрения визира",
                "Strela10_ControlBlock", "TUMBLER_MODE", "CHECK_II"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Устанавливает переключатель РАБОТА - КОНТРОЛЬ I - КОНТРОЛЬ II в положение РАБОТА",
                "Strela10_ControlBlock", "TUMBLER_MODE", "WORK"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Наводит пусковую установку на участок неба с неравномерным фоном нажимает СЛЕЖЕНИЕ - ПУСК до первого упора, в различные положения и находит такое, при котором ГСН не переходит в режим слежения за контрастными элементами неоднородного фона.",
                "Strela10_VizorPanel", "TUMBLER_LOSE", "ON"),
            new OrderSubtask(
                "Наводит пусковую установку на участок неба с неравномерным фоном нажимает СЛЕЖЕНИЕ - ПУСК до первого упора, в различные положения и находит такое, при котором ГСН не переходит в режим слежения за контрастными элементами неоднородного фона.",
                "Strela10_OperatorPanel", "TUMBLER_FON", "STATE_2"),
            new OrderSubtask(
                "Если ГСН ракеты переходит в режим слежения в положении ФОН-2 то пуски ракеты необходимо производить в инфракрасном канеле",
                "Strela10_VizorPanel", "TUMBLER_LOSE", "ON"),
            new OrderSubtask(
                "Если ГСН ракеты переходит в режим слежения в положении ФОН-2 то пуски ракеты необходимо производить в инфракрасном канеле",
                "Strela10_GuidancePanel", "TUMBLER_COOL", "ON"),


            new OrderSubtask("Тумблер РЕЖИМ установить в БОЕВОЕ", "Strela10_OperatorPanel", "TUMBLER_MODE", "COMBAT"),
            new OrderSubtask(
                "Тумблер ВКЛ-ВЫКЛ на пульте оперативного управления установить в положение ВКЛ, при этом начинает работать преобразователь ПО-500А и загорается лампа ВКЛ АОЗ на пульте оперативного управления, и  на приборе У - лампа КОНТР. ВЕНТ. Через 180+-20 с на пульте оперативного управления загорается сигнальная лампа ДЕЖУРНЫЙ ",
                "Strela10_OperationalPanel", "TUMBLER_AOZ", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "АОЗ находится в ежурном режиме работы: устанавливается переключатель УСЛОВИЯ РАБОТЫ на приборе У  в положение КОНТРОЛЬ ВМП, при нормальной работе загорается сигнальная лампа РЕЖИМ ИЗМЕР.",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_7"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Аппаратура оценки зоны находится в дежурном режиме: Тумблер ДЕЖУРНЫЙ - БОЕВОЙ на пульте оперативного управления устанавливается в положение БОЕВОЙ, при этом загорается лампа ВНИМАНИЕ -ИЗЛУЧЕНИЕ, гаснут сигнальные лампы ДЕЖУРНЫЙ и РЕЖИМ ИЗМЕР. на приборе У",
                "Strela10_OperationalPanel", "TUMBLER_MODE", "COMBAT"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Проверяет горение индикаторной лампы 115В на передней панели прибора У сигнализирующая работу выходного напряжения преобразователя ПО - 500А. Проверяет показания приборов ТОКИ КРИСТАЛОВ на передней панели прибора У: - на приборе СИГНАЛ -0,15-0.7мА; - на приборе АЧП 0,1-1,0 мА; проверяет показания прибора ТОК МАГН на соотвествие величине указанной на шильдике прибора ЛППс допуском 1,25мА",
                "Strela10_VizorPanel", "TUMBLER_LOSE", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "НАжимает до упора кнопку КОНТРОЛЬ УПЧ на передней панели прибора У при этом показания пробора ТОК МАГН должны быть в пределах 3,5-7,0 мА",
                "Strela10_VizorPanel", "TUMBLER_YPCH", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ


            new OrderSubtask(
                "Нажимает кнопку КОНТРОЛЬ ВЗ на передней панели прибора У при этом загорается сигнальные лампы ЗОНА и НАЗАД",
                "Strela10_VizorPanel", "TUMBLER_VZ", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Устанавливаем переключатель УСЛОВИЯ РАБОТЫ в положение КОНТРОЛЬ ПОМЕХ и нажимает КОНТРОЛЬ СД при этом загорается лампа РЕЖИМ ИЗМЕРЕН и гаснет лампа КОНТР.ВЕНТ. При отсутсвии помех в установленном тракте сигнальные лампы ЗОНА и НАЗАД не горят",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_6"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Устанавливаем переключатель УСЛОВИЯ РАБОТЫ в положение КОНТРОЛЬ ПОМЕХ и нажимает КОНТРОЛЬ СД при этом загорается лампа РЕЖИМ ИЗМЕРЕН и гаснет лампа КОНТР.ВЕНТ. При отсутсвии помех в установленном тракте сигнальные лампы ЗОНА и НАЗАД не горят",
                "Strela10_VizorPanel", "TUMBLER_SD", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ

            new OrderSubtask(
                "Устанавливаем переключатель УСЛОВИЯ РАБОТЫ в положение КОНТРОЛЬ НС  при этом загорается лампа КОНТРОЛЬ НС",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_8"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Устанавливаем переключатель УСЛОВИЯ РАБОТЫ в одно из следующий положений +30 - +50 при температуре окружающий среды +50 : +30; -10 : + 30 при температуры окружающей среды +30 : -10 ; -50 : -10 при температуре от -10 до -50; МАНЕВР - при маневре цели при любой температуре ",
                "Strela10_VizorPanel", "TUMBLER_POSITION_WORK_TYPE", "POS_2"),//НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Тумблер ДЕЖУРНЫЙ - БОЕВОЙ в положение ДЕЖУРНЫЙ", "Strela10_OperationalPanel",
                "TUMBLER_MODE", "TRAINING"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Тумблер АОЗ в положение ВЫКЛ", "Strela10_OperatorPanel", "TUMBLER_AOZ", "OFF"),
            new OrderSubtask(
                "Проверяет работу наземного радиолокационного запросчика, устанавливает ПИТАНИЕ ВЫКЛ на пульте 1ЛУ в положение ПИТАНИЕ при этом загорается светодиод ГОТОВ",
                "Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Устанавливает переключатель 0-1-2-3-4-5-6 на блоке 1ЛУ вположение соответствующее действующему расписанию кодов",
                "Strela10_VizorPanel", "TUMBLER_CODE_POSITION", "P_0"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Через 3 сек нажимает поочередно кнопки РАБОТА и СЛЕЖЕНИЕ - ПУСК до первого упора и по загоранию светодиода ИСПРАВЛЕНО в узле индикации 9Ш24 на световом табло , расположенном на визире 9Ш127 убеждается в исправности аппаратуры НРЗ",
                "Strela10_GuidancePanel", "TUMBLER_TRACK_LAUNCH", "LAUNCH"), 

            new OrderSubtask(
                "Устанавливает тумблер ЯРКОСТЬ на блоке 9Ш23 в положение МИНИМ.(МАКС в зависимости от наружнего освещения)",
                "Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Выключает тумблер ПИТАНИЕ на пульте управления НРЗ", "Strela10_VizorPanel",
                "TUMBLER_POWER_NRZ", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask(
                "Нажимает и отпускает кнопку БОРТ на левой рукоятке пульта наведения, при этом загорается светодиод ГОТОВ на пульте управления НРЗ",
                "Strela10_GuidancePanel", "TUMBLER_BOARD", "ON")
           }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "Готовность №1", new List<OrderSubtask>()
                                                                                   {
            new OrderSubtask("На пульте оператора тумблер 24В в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("На пульте оператора тумблер 28В в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("Проверяет наличие готовых к пуску ракет по транспорантам 1","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_1"),
            new OrderSubtask("Проверяет наличие готовых к пуску ракет по транспорантам 2","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_2"),
            new OrderSubtask("Проверяет наличие готовых к пуску ракет по транспорантам 3","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_3"),
            new OrderSubtask("Проверяет наличие готовых к пуску ракет по транспорантам 4","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "MODE_4"),
            new OrderSubtask("Проверяет наличие световой сигнализации ВКЛ АРЦ в блоке ВТ ","Strela10_ARC", "TUMBLER_A3", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Включаем тумблер АОЗ на пульте оперативного управления","Strela10_OperatorPanel", "TUMBLER_AOZ", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("После загорания лампы ДЕЖУРНЫЙ - БОЕВОЙ тумблер ДЕЖУРНЫЙ переводит в положение БОЕВОЙ","Strela10_OperationalPanel", "TUMBLER_MODE", "COMBAT"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Тумблер РЕЖИМ устанавливает в положение БОЕВОЙ","Strela10_OperatorPanel", "TUMBLER_MODE", "COMBAT"),
            new OrderSubtask("Открывает крышку головной призмы визира 9Ш127М","Strela10_CommonPanel", "HANDL", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Снимает ПУ со стопора по азимуту","Strela10_CommonPanel", "PEDAL_AZIM", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Включает привод (на ПО тумблер ПРИВОД устанавливает положение ПРИВОД) и проверяет возможность навдение ПУ по азимуту и углу места. Докладывает ПЕРВЫЙ ГОТОВ","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),

                                                                                   }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "Готовность №2", new List<OrderSubtask>()
                                                                                   {
            new OrderSubtask("На пульте оператора тумблер 24В в  положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "OFF"),
            new OrderSubtask("На пульте оператора тумблер 28В в  положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "OFF"),
            new OrderSubtask("Тумблер ПРИВОД - ВЫКЛ - РУЧНОЕ в положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "OFF"),
            new OrderSubtask("На пульте оперативного управления тумблер БОЕВОЙ - ДЕЖУРНЫЙ в положение ДЕЖУРНЫЙ ","Strela10_OperationalPanel", "TUMBLER_MODE", "TRAINING"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Тумблер АОЗ в положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_AOZ", "OFF"),
            new OrderSubtask("На пульте управления НРЗ тумблер ПИТАНИЕ в пололжение ВЫКЛ. Докладывает ПЕРВЫЙ ГОТОВ","Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
                                                                                   }));
        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "Исходные настройки", new List<OrderSubtask>(){
            new OrderSubtask("УСЛОВИЯ РАБ.— в положение, соответствующее выбранному режиму.","Strela10_VizorPanel","TUMBLER_POSITION_WORK_TYPE","POS_2"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Ручка УСИЛЕНИЕ в среднее положение на суммирующем усилителе","Strela10_SoundPanel", "SPINNER_SOUNDCO", "50"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Ручка РЕЧЬ в среднее положение на суммирующем усилителе","Strela10_SoundPanel", "SPINNER_SPEECH", "180"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Ручка ЗВУК в среднее положение на суммирующем усилителе","Strela10_SoundPanel", "SPINNER_AMPLIFIER", "180"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте оперативноего управления оценке зоны тумблер ДЕЖУРНЫЙ-БОЕВОЙ в положение ДЕЖУРНЫЙ","Strela10_OperationalPanel", "TUMBLER_MODE", "TRAINING"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте оперативноего управления оценке зоны тумблер ВКЛ-ВЫКЛ в положение ВЫКЛ","Strela10_OperationalPanel", "TUMBLER_AOZ", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте оператора тумблер АОЗ в положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_AOZ", "ON"),
            new OrderSubtask("На пульте оператора тумблер ПРИВОД-ВЫКЛ-РУЧНОЕ в положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "OFF"),
            new OrderSubtask("На пульте оператора переключатель ФОН в положение ФОН-1","Strela10_OperatorPanel", "TUMBLER_FON", "STATE_1"),
            new OrderSubtask("На пульте оператора тумблер РЕЖИМ в положение УЧЕБНЫЙ","Strela10_OperatorPanel", "TUMBLER_MODE", "TRAINING"),
            new OrderSubtask("На пульте оператора переключать РОД-РАБОТЫ в положение АВТО","Strela10_OperatorPanel", "TUMBLER_WORK_TYPE", "AUTO"),
            new OrderSubtask("На пульте оператора ручку ЯРКОСТЬ в сренее положение","Strela10_OperatorPanel", "SPINNER_BRIGHTNESS", "50"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте оператора тумблер ПСП в положение БП","Strela10_OperatorPanel", "TUMBLER_PSP", "OFF"),
            new OrderSubtask("На пульте оператора тумблер РЕЖИМ ПСП в положение НП","Strela10_OperatorPanel", "TUMBLER_PSP_MODE", "NP"),
            new OrderSubtask("На пульте оператора тумблер СОС в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_SS", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте оператора тумблер ЗАДЕРЖКА НВУ в  положение ОСНОВ","Strela10_OperatorPanel", "TUMBLER_TEST", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте оператора тумблер 24В в  положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "OFF"),
            new OrderSubtask("На пульте оператора тумблер 28В в  положение ВЫКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ВЕНТИЛЯТОР в положение ВЫКЛ","Strela10_SupportPanel", "TUMBLER_FAN", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ОБОГРЕВ СТЕКЛА в положение ВЫКЛ","Strela10_SupportPanel", "TUMBLER_GLASS_HEATING", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ОСВЕЩЕНИЕ в положение ВЫКЛ","Strela10_SupportPanel", "TUMBLER_LIGHT", "OFF"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ПЕРЕВОД в положение ПОХОДНОЕ","Strela10_SupportPanel", "TUMBLER_POSITION", "STOWED"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер СЛЕЖЕНИЕ в положение РУЧНОЕ","Strela10_SupportPanel", "TUMBLER_TRACKING", "MANUAL"),
            new OrderSubtask("На левой рукоятке пульта наведения тумблер ОХЛАЖДЕНИЕ в положение ВЫКЛ","Strela10_GuidancePanel", "TUMBLER_COOL", "OFF"),
            new OrderSubtask("На указателе азимута тумблер С в положение ВЫКЛ","Strela10_AzimuthIndicator", "TUMBLER_C", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На указателе азимута тумблер ШКАЛА  в положение ВЫКЛ","Strela10_AzimuthIndicator", "TUMBLER_BACKLIGHT", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На указателе азимута ручку ЯРКОСТЬ в среднее положение","Strela10_AzimuthIndicator", "SPINNER_AMPLIFIER", "180"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На оптическом визире 9Ш127М предварительно открыв крышку визира, рукоятку переключателя светофильтров в положение НЕЙТРАЛЬНЫЙ ФИЛЬТР","Strela10_VizorPanel", "TUMBLER_FILTER_UP", "CENTER"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Тумблер ПОДСВЕТКА  - положение ВЫКЛ","Strela10_VizorPanel", "TUMBLER_ILLUM", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Оправа окуляра в настроенное для глаз положение; рукоятки защитной крышки головной призмы визира - в положение ЗАКРЫТО ","Strela10_CommonPanel", "HANDL", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На визире 9Ш127М рукоятку переключение кратности - в положение 1,8*","Strela10_VizorPanel", "TUMBLER_FILTER_DOWN", "X_2"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На панели АЗС  тумблер ВГ","Strela10_VizorPanel","TUMBLER_VV","ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На панели АЗС  тумблер ВВ","Strela10_VizorPanel","TUMBLER_СС","ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На панели АЗС  тумблер ВП","Strela10_VizorPanel","TUMBLER_СС2","ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
             new OrderSubtask("На блоке управлние НРЗ тумблер ПИТАНИЕ - в положение ВЫКЛ","Strela10_VizorPanel", "TUMBLER_POWER_NRZ", "OFF"), //НЕТ АППАРАТНОЙ ЧАСТИ
             new OrderSubtask("Переключатель кодов - в положение, соответсвующее действующему расписанию","Strela10_VizorPanel", "TUMBLER_CODE_POSITION", "P_0"), //НЕТ АППАРАТНОЙ ЧАСТИ
                
            new OrderSubtask("На пульте управление системы пеленгования ручку ЯРКОСТЬ в среднее положение","Strela10_ControlBlock", "SPINNER_BRIGHTNESS", "50"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте управление системы пеленгования переключатель РАБОТА-КОНТРОЛЬ I-КОНТРОЛЬ II в положение РАБОТА","Strela10_ControlBlock", "TUMBLER_MODE", "WORK"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте управление системы пеленгования переключатель СТУПЕНЬ III в положение ВКЛ","Strela10_ControlBlock", "TUMBLER_STAGE_3", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("На пульте управление системы пеленгования переключатель СТУПЕНЬ IV в положение ВКЛ","Strela10_ControlBlock", "TUMBLER_STAGE_4", "ON"), //НЕТ АППАРАТНОЙ ЧАСТИ
            new OrderSubtask("Рукоятку стопора башни по азимуту в положение ЗАСТОПОРЕНО","Strela10_CommonPanel", "TUMBLER_STOP", "ON")  //НЕТ АППАРАТНОЙ ЧАСТИ
        }));

        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "Включить питание", new List<OrderSubtask>(){
            new OrderSubtask("На пульте оператора тумблер 24В в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("На пульте оператора тумблер 28В в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("На пульте оператора тумблер 30В в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_30B", "ON")
        }));

        orders.Add(new MCSTrainingOrder("Strela-10_Operator", "Перевод ПУ", new List<OrderSubtask>(){
            new OrderSubtask("На пульте оператора тумблер 24В в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_24B", "ON"),
            new OrderSubtask("На пульте оператора тумблер 28В в  положение ВКЛ","Strela10_OperatorPanel", "TUMBLER_POWER_28B", "ON"),
            new OrderSubtask("На пульте оператора тумблер ПРИВОД-ВЫКЛ-РУЧНОЕ в положение ПРИВОД","Strela10_OperatorPanel", "TUMBLER_DRIVE_HANDLE_OFF", "DRIVE"),
            new OrderSubtask("На пульте оператора ПО-2 тумблер ПЕРЕВОД в положение БОЕВОЕ","Strela10_SupportPanel", "TUMBLER_POSITION", "BATTLE"),
            new OrderSubtask("Снять пусковую установку со стопора","Strela10_CommonPanel", "PEDAL_AZIM", "ON")
            //new OrderSubtask("Растопорить ПУ по азимуту","Strela10_CommonPanel", "TUMBLER_STOP", "OFF")  //Засыпалов team: убрали для работы с блоками (аппаратная часть)
            }));
    }

    public override List<MCSTrainingOrder> GetAllOrders()
    {
        //orders[1].OrderName.ToString()
        return orders;
    }
}