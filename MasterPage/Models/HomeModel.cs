using System;
using System.Data;
using Garza.Data;

namespace Garza.Models
{
    public class CommonModel
    {
        public NskdSessionLite Session;
        public Object PageModel;
        public UserMainMenu UserMainMenu;
    }
    public class UserMainMenu
    {
        public String SelectedNodePath { get; set; }
        public String JsonUserMainMenu { get; set; }
        public UserMainMenu()
        {
            SelectedNodePath = "[Пользователь: ?]";
            JsonUserMainMenu = @"
                { name: 'Пользователь: ?', cont: [
                    { name: 'Регистрация', cont: [
                        { name: 'Вход' },
                        { name: 'Выход' } ] } ]
                }";
        }
        public UserMainMenu(NskdSessionLite session)
        {
            SelectedNodePath = "[" + session.UserName + "]";
            switch (session.UserId)
            {
                case 2: // Соколов Евгений Анатольевич - программист
                case 3: // Шанин Григорий Олегович - управляющий
                case 4: // Мартовицкий Дмитрий Владимрович - ген. директор
                case 6: // Родоманченко Наталья Витальевна - отд. кадров
                case 7: // Сорокина Надежда Анатольевна - тендерный отдел (договоры)
                case 8: // Максимова Екатерина Викторовна - юр. отдел
                case 9: // Бельченко Юлия Викторовна - юр. отдел
                case 10: // Федущак Роман Владимирович - юр. отдел
                case 20: // Потекаева Ирина Ивановна - юр. отдел
                case 26: // Мурзина Татьяна - секретарь
                case 34: // Егорова Евгения Валерьевна - тендерный отдел
                case 63: // Воронов Максим Владимирович - склад
                case 65: // Мурашова Т. Н. - склад
                case 66: // Ястребова Елена - зам. Шанина
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: [
                            { name: 'Договоры', url: '/Agrs/F1' },
                            { name: 'Отчёты', url: null, cont: [
                                { name: 'Неоплаченные отгрузки', url: '/Reports/R1' },
                                { name: 'Отчёт по оплатам', url: '/Reports/R2' },
                                { name: 'Судебные документы', url: '/Reports/R3' } ] } ]
                        }";
                    break;
                case 0: // Не прошел проверку
                case 1: // Пустой
                case 5:  // Пирожкова Вероника - отдел продаж помошница Заваловой Елены
                case 11: // Баржина Татьяна - секретарь
                case 12: // Баризова Наталья - секретарь
                case 13: // Коледова Юлия Ивановна - бывший регистратор теперь помощник Августовой Ангелины
                case 14: // Максутов Игорь
                case 15: // Заколодкин Владимир
                case 16: // Кодина Марина
                case 17: // Магергут Татьяна - менеджер по продажам
                case 18: // Скворцова Марина - менеджер по продажам
                case 19: // Сущева Ольга - менеджер по продажам
                case 21: // Романова Нина - менеджер по продажам
                case 22: // Каблукова М.
                case 23: // Завалова Елена - менеджер по продажам
                case 24: // Борисова Валентина
                case 25: // Миронова Кристина - менеджер по продажам
                case 27: // Волостных Роман - менеджер по продажам
                case 28: // Горинова Анастасия
                case 29: // Борисяк Ольга - менеджер по продажам
                case 30: // Ерастова Людмила - менеджер по продажам
                case 31: // Августова Ангелина - менеджер по продажам
                case 32: // Королькова Анна - менеджер по продажам
                case 33: // Шаповалова Валентина
                case 35: // Шанина Елена - менеджер по продажам - помощник Сущевой Ольги
                case 36: // Кравчук Ирина - менеджер по продажам - помощник Заваловой Елены
                case 37: // Алдущенкова Эльвира - менеджер по продажам - помощник Магергут Татьяны
                case 38: // Коробкова Юля - менеджер по продажам - помощник Корольковой Анны
                case 39: // Синицкая Иветта - менеджер по продажам - помощник Августовой Ангелины
                case 40: // Легонькова Анастасия - менеджер по продажам - помощник Заваловой Елены (Горинова?)
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54: // Михайлова Анна Андреевна - ассистент отдела закупок
                case 55: // Паннафидина Екатерина - помощник менеджера Ерастовой Людмилы
                case 56: // Серкерова
                case 57: // Тарунтаева - помощник Мироновой
                case 58: // Лобанова Елена - помощник Магергут Татьяны
                case 59: // Углова Алёна Александрована - помощник менеджера Корольковой Анны
                case 60: // Миловидов Василий Александрович
                case 61: // Морева Марина - помощник Августовой Ангелины
                case 62: // Перевалова Юлия Викторовна - менеджер
                case 64: // Мехрабова
                default: // Все остальные
                    JsonUserMainMenu = @"
                        { name: '" + session.UserName + @"', url: null, cont: []
                        }";
                    break;
            }
        }
    }
    public class NskdSessionLite
    {
        public Guid SessionId;
        public Int32 UserId;
        public String UserName;
        public NskdSessionLite() { }
        public NskdSessionLite(String userHostAddress)
        {
            SessionId = Guid.NewGuid();
            UserId = 0;
            // регистрируем новую сессию пока без ключа и без пользователя
            HomeData.Home.CreateSession(SessionId, userHostAddress);
        }
        public void UpdateSession(String userToken, String cryptKey)
        {
            HomeData.Home.UpdateSession(userToken, SessionId, cryptKey);
        }
        public static NskdSessionLite GetById(Guid sessionId)
        {
            NskdSessionLite session = new NskdSessionLite();
            // загружаем данные сессии
            DataTable dt = HomeData.Home.GetSessionById(sessionId);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    session.SessionId = (Guid)dr["id"];
                    session.UserId = (dr["user_id"] == DBNull.Value) ? 0 : (Int32)dr["user_id"];
                    session.UserName = (dr["name"] == DBNull.Value) ? "Гость" : (String)dr["name"];
                }
            }
            return session;
        }
    }

}