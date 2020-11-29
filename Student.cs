using System.Collections.Generic;
using System.Windows.Forms;

namespace Mongo_DB
{
    class Student
    {
        public string Id { get; private set; }
        public string Fullname { get; private set; }
        public string Login { get; private set; }
        public string Password { get; private set; }

        /// <summary>
        /// Стандартный конструктор. Всем свойствам объекта присваивается значение NULL
        /// </summary>
        public Student()
        {
            Id = null;
            Fullname = null;
            Login = null;
            Password = null;
        }

        /// <summary>
        /// Конструктор с вводом свойств объекта
        /// </summary>
        /// <param name="Id">Id объекта</param>
        /// <param name="Fullname">Полное ФИО</param>
        /// <param name="Login">Логин</param>
        /// <param name="Password">Пароль</param>
        public Student(string Id, string Fullname, string Login, string Password)
        {
            this.Id = Id;
            this.Fullname = Fullname;
            this.Login = Login;
            this.Password = Password;
        }

        /// <summary>
        /// Конструктор с вводом свойств объекта и автоматическим вводом id
        /// </summary>
        /// <param name="students">Список студентов</param>
        /// <param name="Fullname">Полное ФИО</param>
        /// <param name="Login">Логин</param>
        /// <param name="Password">Пароль</param>
        public Student(List<Student> students, string Fullname, string Login, string Password)
        {
            FindUniqueId(students);
            this.Fullname = Fullname;
            this.Login = Login;
            this.Password = Password;
        }

        /// <summary>
        /// Изменение ФИО
        /// </summary>
        /// <param name="Fullname">Новое ФИО</param>
        public void Set_FullName(string Fullname) { this.Fullname = Fullname; }
        /// <summary>
        /// Изменение логина
        /// </summary>
        /// <param name="Login">Новый логин</param>
        public void Set_Login(string Login) { this.Login = Login; }

        /// <summary>
        /// Добавляет объект в DataGridView
        /// </summary>
        /// <param name="dataGridView">Ссылка на целевую таблицу</param>
        public void AddTo_DataGridView(ref DataGridView dataGridView)
        {
            dataGridView.Rows.Add();
            dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[0].Value = Id;
            dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[1].Value = Fullname;
            dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[2].Value = Login;
            dataGridView.Rows[dataGridView.Rows.Count - 1].Cells[3].Value = Password;
        }

        /// <summary>
        /// Проверка логина на уникальность
        /// </summary>
        /// <param name="list">Cписок студентов</param>
        /// <returns></returns>
        public bool ConfirmUniqueness(List<Student> list)
        {
            foreach(Student student in list)
            {
                if (Login == student.Login)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Поиск свободного ID
        /// </summary>
        /// <param name="list">Список студентов</param>
        /// <returns></returns>
        public void FindUniqueId(List<Student> list)
        {
            List<int> Ids = new List<int>();
            foreach (Student student in list)
                Ids.Add(int.Parse(student.Id));
            Ids.Sort();
            for(int i=0; i<Ids.Count; i++)
            {
                if (i != Ids[i])
                {
                    Id = i.ToString();
                    return;
                }
            }
            Id = Ids.Count.ToString();
        }
    }
}
