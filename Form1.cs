using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mongo_DB
{
    /// <summary>
    /// Класс главной формы
    /// Содержит обработчики всех событий.
    /// </summary>
    public partial class Form1 : Form
    {

        private const string Server_Path = "mongodb://localhost:27017"; // Адрес сервера MongoDB
        private const string DB_Name = "test_db"; // Название базы данный
        private const string Collection_Name = "students"; // Название коллекции
        private DataBase_Client dataBase;

        public Form1()
        {
            InitializeComponent();
            dataBase = new DataBase_Client(Server_Path, DB_Name);
        }        

        /// <summary>
        /// Функция добавления нового студента в коллекцию
        /// Осуществляет проверку логина на уникальность
        /// Добавляет информацию в таблицу
        /// Добавляет информацию в БД
        /// </summary>
        private async Task AddNewStudent()
        {
            List<Student> students = await dataBase.Get_AllDocs<Student>(Collection_Name);
            Student NewStudent = new Student(students, textBox1.Text, textBox2.Text, textBox3.Text);
            if (!NewStudent.ConfirmUniqueness(students)) //Проверка логина на уникальность
            {
                MessageBox.Show("Логин уже существует!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            NewStudent.AddTo_DataGridView(ref dataGridView1);
            _ = dataBase.AddToCollectionAsync(NewStudent, Collection_Name);
        }

        /// <summary>
        /// Получает информацию из базы данных и добавляет ее в таблицу
        /// </summary>
        private async Task LoadDataFromDB()
        {
            List<Student> students = await dataBase.Get_AllDocs<Student>(Collection_Name);
            dataGridView1.Rows.Clear();
            foreach(Student student in students)
            {
                student.AddTo_DataGridView(ref dataGridView1);
            }
        }

        /// <summary>
        /// Удаляет выбранные в таблице записи
        /// Удаление происходит в базе данный, затем таблица обновляется функцией LoadDataFromDB()
        /// </summary>
        private async Task DeleteSelected()
        {
            DataGridViewSelectedRowCollection SelectedRows = dataGridView1.SelectedRows;
            Student student;
            IMongoCollection<Student> Collection = dataBase.Database.GetCollection<Student>(Collection_Name);

            foreach (DataGridViewRow Row in SelectedRows)
            {
                student = new Student(Row.Cells[0].Value.ToString(), Row.Cells[1].Value.ToString(),
                                      Row.Cells[2].Value.ToString(), Row.Cells[3].Value.ToString());
                await Collection.DeleteOneAsync(p => p.Id == student.Id);
            } 
            _= LoadDataFromDB();
        }

        /// <summary>
        /// Обработчик события "Нажатие на кнопку "Добавить" "
        /// Вызывает функцию добавления новой записи в БД
        /// </summary>
        private void AddNewStudent_Click(object sender, EventArgs e) => _ = AddNewStudent();        

        /// <summary>
        /// Обработчик события "Нажатие на кнопку "Обновить таблицу" "
        /// Вызывает функцию обновления данных в таблицк
        /// </summary>
        private void Update_Data(object sender, EventArgs e) => _ = LoadDataFromDB();

        /// <summary>
        /// Обработчик события "Нажатие на кнопку "Изменить выбранное" "
        /// Вызывает функцию изменения выбранных записей (открывается отдельное окно)
        /// Обновляет данные в таблице с помощью функции LoadDataFromDB()
        /// </summary>
        private void ChangeSelected_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0) { return; } // Проверка на наличие выбранных записей

            ChangeSelected_Form changeSelected_Form = new ChangeSelected_Form(ref dataBase, dataGridView1.SelectedRows, Collection_Name);
            changeSelected_Form.ShowDialog();
            _ = LoadDataFromDB();
        }

        /// <summary>
        /// Обработчик события "Нажатие на кнопку "Удалить выбранное" "
        /// Вызывает функцию удаления выбранных записей DeleteSelected()
        /// </summary>
        private void DeleteSelected_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) { return; } // Проверка на наличие выбранных записей
            _ = DeleteSelected();
        }
    }
}
