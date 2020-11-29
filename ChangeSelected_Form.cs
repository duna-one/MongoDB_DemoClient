using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mongo_DB
{
    public partial class ChangeSelected_Form : Form
    {
        private DataBase_Client dataBase;
        private Stack<DataGridViewRow> Rows = new Stack<DataGridViewRow>();
        private string Collection_Name;
        private string Id;

        public ChangeSelected_Form(ref DataBase_Client dataBase, DataGridViewSelectedRowCollection selectedRows, string Collection_Name)
        {
            InitializeComponent();
            this.dataBase = dataBase;            
            this.Collection_Name = Collection_Name;

            foreach (DataGridViewRow row in selectedRows)
                Rows.Push(row);

            Id = Rows.Peek().Cells[0].Value.ToString();
            textBox1.Text = Rows.Peek().Cells[1].Value.ToString();
            textBox2.Text = Rows.Peek().Cells[2].Value.ToString();
            textBox3.Text = Rows.Peek().Cells[3].Value.ToString();
            Rows.Pop();
        }

        private async Task Change()
        {
            Student student = new Student(Id, textBox1.Text, textBox2.Text, textBox3.Text);
            List<Student> students = await dataBase.Get_AllDocs<Student>(Collection_Name);
            IMongoCollection<Student> Collection = dataBase.Database.GetCollection<Student>(Collection_Name);

            if (!student.ConfirmUniqueness(students))
            {
                MessageBox.Show("Логин уже существует!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            await Collection.ReplaceOneAsync(p => p.Id == Id, student);

            if (Rows.Count != 0)
            {
                Id = Rows.Peek().Cells[0].Value.ToString();
                textBox1.Text = Rows.Peek().Cells[1].Value.ToString();
                textBox2.Text = Rows.Peek().Cells[2].Value.ToString();
                textBox3.Text = Rows.Peek().Cells[3].Value.ToString();
                Rows.Pop();
                return;
            }
            else
            {
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _ = Change();
        }
    }
}
