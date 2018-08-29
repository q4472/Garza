﻿using Garza.Base.Data;
using Nskd;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Garza.AreasAgrs.Areas.Agrs.Models
{
    public class F0Model
    {
        public SelectorWithListBox custSelector { get; set; }
        public SelectorWithListBox presSelector { get; set; }
        public SelectorWithListBox stuffSelector { get; set; }
        public NpcDataTable workingData { get; set; } // таблица Договоры
        public NpcDataTable filterTable { get; set; } // фильтр для таблицы Договоры
        //private NpcDataRow filterRow { get; set; }
        public F0Model(String sessionId)
        {
            GetData();
            GetFilter();
            LoadDictionariesAndFillPopups();
        }
        public F0Model(Dictionary<String, String> fs)
        {
            GetData(fs);
        }
        public void LoadDictionariesAndFillPopups()
        {
            custSelector = new SelectorWithListBox("Клиенты", workingData.Rows[0]["F4"] as String);
            presSelector = new SelectorWithListBox("Представители", workingData.Rows[0]["pres"] as String);
            stuffSelector = new SelectorWithListBox("Сотрудники", workingData.Rows[0]["F12"] as String);
        }
        public void GetData(Dictionary<String, String> fs = null)
        {
            workingData = Ss.CreateNpcDataTableWithMd(SsAgrTable.Md);
            Ss.FillSsAgrTable(workingData, fs);
        }
        public void GetFilter()
        {
            filterTable = Ss.CreateNpcDataTableWithMd(SsAgrTable.Md);
            // row for min
            NpcDataRow filterRow = filterTable.NewRow();
            filterTable.Rows.Add(filterRow);
            // row for max
            filterRow = filterTable.NewRow();
            filterTable.Rows.Add(filterRow);
        }
        public static DataTable GetDataForSelectorWithListBox(String tableName, String filter)
        {
            DataTable dt = null;
            switch (tableName)
            {
                case "Клиенты":
                case "Представители":
                    dt = HomeData.Agrs.F0GetCustTable(filter);
                    break;
                case "Сотрудники":
                    dt = HomeData.Agrs.F0GetStuffTable(filter);
                    break;
                default:
                    break;
            }
            return dt;
        }
        public static String Upsert(Dictionary<String, String> pars = null)
        {
            String status = "error";
            if (pars != null)
            {
                // надо сохранить в 1с, в xl, в sql и потом обновить sql из xl и 1с
                // первое сохранение в 1с потому, что если это новая запись, то получим код который нужен для xl

                if (String.IsNullOrWhiteSpace(pars["f0"]) && (!pars["f4"].Contains("-15")))
                {
                    // здесь надо получить номер из sql
                    String agrNum = HomeData.Agrs.F0GetAgrNumSql();
                    pars["f4"] = agrNum;
                    if (pars["f5"].Length == 0)
                    {
                        pars["f5"] = pars["f4"];
                    }
                }

                Int32 code = -1;
                String cmd = null;
                Int32.TryParse(pars["f16"], out code);
                if (code > 0)
                {
                    cmd = "Обновить";
                }
                else
                {
                    cmd = "Добавить";
                }
                Dictionary<String, String> ocPars = new Dictionary<string, string>();
                ocPars.Add("Тип", "Справочник");
                ocPars.Add("Вид", "Договоры");
                ocPars.Add("Код", code.ToString());
                ocPars.Add("Примечание", pars["f3"]);
                ocPars.Add("Наименование", pars["f5"]);
                ocPars.Add("Владелец", pars["f6"]);
                ocPars.Add("ВладелецКод", System.Convert.ToString(pars["c0"]));
                ocPars.Add("ДатаДоговора", pars["f8"]);
                ocPars.Add("ОтветЛицо", pars["f14"]);
                ocPars.Add("СуммаДоговора", pars["f15"]);
                ocPars.Add("НомерТоргов", pars["f17"]);
                ocPars.Add("ДатаОкончания", pars["f19"]);
                ocPars.Add("ОтсрочкаПлатежа", pars["f20"]);
                ocPars.Add("Представитель", pars["f21"]);
                ocPars.Add("ДопСоглашение", pars["f22"]);
                ocPars.Add("Пролонгация", pars["f23"]);
                ocPars.Add("ГосударственныйИдентификатор", pars["f24"]);

                code = HomeData.Agrs.F0Upsert1c(cmd, ocPars);
                if (code >= 0)
                {
                    pars["f16"] = code.ToString();
                }

                // сохраняем в xl
                if (String.IsNullOrWhiteSpace(pars["f0"]))
                {
                    cmd = "Добавить";
                }
                else
                {
                    cmd = "Обновить";
                }
                Dictionary<String, String> xlPars = new Dictionary<string, string>();
                // в метаданных первые 19 строк это колонки из xl
                //Object[] md = SsAgrTable.Md;
                for (int i = 0; i < 19; i++)
                {
                    //(String)((Object[])md[i])[0]
                    String key = "f" + i.ToString();
                    xlPars.Add(key, pars[key]);
                }
                String newNum = pars["f4"];
                // в xl сохраняем только договоры 2015 года, потом они попадут в sql при обновлении.
                // договоры 2016 в xl не сохраняем, а сохраняем их сразу sql
                // 2017-01-10 перестали обращаться к xl
                /*
                if (newNum.Contains("-15"))
                {
                    status = Data.HomeData.Agrs.F0UpsertExcel(cmd, xlPars);
                }
                else
                {
                    Data.HomeData.Agrs.F0UpsertSql(cmd, xlPars);
                    status = "ok";
                }
                */


                xlPars.Add("f19", pars["f24"]);

                HomeData.Agrs.F0UpsertSql(cmd, xlPars);
                status = "ok";
            }
            return status;
        }
        public static String Delete(Dictionary<String, String> pars = null)
        {
            String status = "error";
            if (pars != null)
            {
                if (pars.ContainsKey("f0") && (!String.IsNullOrWhiteSpace(pars["f0"])))
                {
                    // есть ссылка на xl
                    status = "есть ссылка на xl: " + pars["f0"];
                }
                if (pars.ContainsKey("f16") && (!String.IsNullOrWhiteSpace(pars["f16"])))
                {
                    // есть ссылка на 1c
                    status += ", есть ссылка на 1c: " + pars["f16"];

                    String cmd = null;
                    Dictionary<String, String> ocPars = new Dictionary<string, string>();
                    Int32 code = -1;
                    Int32.TryParse(pars["f16"], out code);
                    if (code > 0)
                    {
                        cmd = "Удалить";

                        ocPars.Add("Тип", "Справочник");
                        ocPars.Add("Вид", "Договоры");
                        ocPars.Add("Код", code.ToString());

                        code = HomeData.Agrs.F0Upsert1c(cmd, ocPars);
                    }
                }
            }
            return status;
        }
    }

    public class SelectorWithListBox
    {
        public String Id { get; set; }
        public String TableName { get; set; }
        public String InitialValue { get; set; }
        public SelectorWithListBox(String tableName, String initialValue)
        {
            Id = Guid.NewGuid().ToString();
            TableName = tableName;
            InitialValue = initialValue;
        }
    }
}