using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mongo_DB
{
    /// <summary>
    /// Окно изменения выбранных записей.
    /// Не закрывается пока не будут изменены все выбранные записи
    /// Можно закрыть вручную, но тогда будут изменены не все записи.
    /// </summary>
    public partial class ChangeSelected_Form : Form
    {
        private DataBase_Client dataBase; 
        private Stack<DataGridViewRow> Rows = new Stack<DataGridViewRow>(); //Стек выбранных записей
        private string Collection_Name; // Имя коллекции
        private string Id; //ID изменяемой записи

        /// <summary>
        /// Конструктор
        /// Инициализирует компонент
        /// Создает стек выбранных записей
        /// </summary>
        /// <param name="dataBase">Ссылка на базу данных</param>
        /// <param name="selectedRows">Коллекция выбранных строк</param>
        /// <param name="Collection_Name">Имя коллекции в БД</param>
        public ChangeSelected_Form(ref DataBase_Client dataBase, DataGridViewSelectedRowCollection selectedRows, string Collection_Name)
        {
            InitializeComponent();
            this.dataBase = dataBase;            
            this.Collection_Name = Collection_Name;

            foreach (DataGridViewRow row in selectedRows)
                Rows.Push(row); //Создание стека выбранных записей

            Id = Rows.Peek().Cells[0].Value.ToString();
            textBox1.Text = Rows.Peek().Cells[1].Value.ToString();
            textBox2.Text = Rows.Peek().Cells[2].Value.ToString();
            textBox3.Text = Rows.Peek().Cells[3].Value.ToString();
            Rows.Pop();
        }

        /// <summary>
        /// Обработчик события "Нажатие на кномпу "Изменить" "
        /// Функция изменения конкретной записи в БД
        /// </summary>
        private async void Change(object sender, EventArgs e)
        {
            Student student = new Student(Id, textBox1.Text, textBox2.Text, textBox3.Text);
            List<Student> students = await dataBase.Get_AllDocs<Student>(Collection_Name);
            IMongoCollection<Student> Collection = dataBase.Database.GetCollection<Student>(Collection_Name);

            if (!student.ConfirmUniqueness(students)) //Проверка логина на уникальность
            {
                MessageBox.Show("Логин уже существует!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            await Collection.ReplaceOneAsync(p => p.Id == Id, student);

            if (Rows.Count != 0) //Если в стеке еще остались записи, то добавить их данные в поля для изменения и продолжить работу окна
            {
                Id = Rows.Peek().Cells[0].Value.ToString();
                textBox1.Text = Rows.Peek().Cells[1].Value.ToString();
                textBox2.Text = Rows.Peek().Cells[2].Value.ToString();
                textBox3.Text = Rows.Peek().Cells[3].Value.ToString();
                Rows.Pop();
                return;
            }
            // Если стек пуст, то окно закрывается
            else
            {
                Close();
            }
        }
    }
}
