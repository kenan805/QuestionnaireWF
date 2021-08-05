using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuestionnaireWF
{
    public partial class Form1 : Form
    {
        Person user = new Person();

        public Form1()
        {
            InitializeComponent();
        }

        private void TextBoxName_TextChanged(object sender, EventArgs e)
        {
            user.Name = textBoxName.Text;
        }

        private void TextBoxSurname_TextChanged(object sender, EventArgs e)
        {
            user.Surname = textBoxSurname.Text;
        }

        private void TextBoxEmail_TextChanged(object sender, EventArgs e)
        {
            user.Email = textBoxEmail.Text;
        }


        private void DateTimePickerBirthdate_ValueChanged(object sender, EventArgs e)
        {
            user.BirthDate = dateTimePickerBirthdate.Value;
        }

        private void BtnAddOrChange_Click(object sender, EventArgs e)
        {
            if (groupBox_Anket.Controls.OfType<TextBox>().Any(tb => string.IsNullOrEmpty(tb.Text))
                || !maskedTextBoxPhone.MaskCompleted)
            {
                MessageBox.Show("Fill in the blanks!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (btnAdd.Text == "Add")
            {
                listBox1.Items.Add(user);
            }
            else if (btnAdd.Text == "Change")
            {
                var selectPerson = (listBox1.SelectedItem as Person);
                File.Delete(selectPerson.Name + ".json");
                var index = listBox1.SelectedIndex;
                if (selectPerson != null)
                {
                    selectPerson.Name = textBoxName.Text;
                    selectPerson.Surname = textBoxSurname.Text;
                    selectPerson.Email = textBoxEmail.Text;
                    selectPerson.Phone = maskedTextBoxPhone.Text;
                    selectPerson.BirthDate = dateTimePickerBirthdate.Value;
                    listBox1.Items.Remove(selectPerson);
                    listBox1.Items.Insert(index, selectPerson);
                    btnAdd.Text = "Change";
                    MessageBox.Show("Changed is successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            btnAdd.Text = "Add";
            if (!string.IsNullOrWhiteSpace(textBoxFilename.Text))
            {
                if (groupBox_Anket.Controls.OfType<TextBox>().Any(tb => string.IsNullOrEmpty(tb.Text))
                || !maskedTextBoxPhone.MaskCompleted)
                {
                    MessageBox.Show("Fill in the blanks!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    var options = new JsonSerializerOptions();
                    options.WriteIndented = true;
                    options.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                    var textJson = JsonSerializer.Serialize(user, options);
                    File.WriteAllText(textBoxFilename.Text + ".json", textJson);
                    MessageBox.Show("The questionnaire was completed successfully!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    groupBox_Anket.Controls.OfType<TextBox>().ToList().ForEach(tb => tb.Text = string.Empty);
                    maskedTextBoxPhone.Text = string.Empty;
                    dateTimePickerBirthdate.Value = DateTime.Now;
                }
            }
            else
                MessageBox.Show("File name is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxFilename.Text))
            {
                if (File.Exists(textBoxFilename.Text + ".json"))
                {
                    Person user = new Person();
                    var text = File.ReadAllText(textBoxFilename.Text + ".json");
                    user = JsonSerializer.Deserialize<Person>(text);
                    listBox1.Items.Add(user);
                }
                else
                    MessageBox.Show("File not found!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("File name is empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            groupBox_Anket.Controls.OfType<TextBox>().ToList().ForEach(tb => tb.Text = string.Empty);
            maskedTextBoxPhone.Text = string.Empty;
            dateTimePickerBirthdate.Value = DateTime.Now;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectPerson = (listBox1.SelectedItem as Person);
            if (selectPerson != null)
            {
                textBoxName.Text = selectPerson.Name;
                textBoxSurname.Text = selectPerson.Surname;
                textBoxEmail.Text = selectPerson.Email;
                maskedTextBoxPhone.Text = selectPerson.Phone;
                dateTimePickerBirthdate.Value = selectPerson.BirthDate;
                btnAdd.Text = "Change";
            }
            else
            {
                MessageBox.Show("Person is null!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Form1_Load(object sender, EventArgs e) => listBox1.DisplayMember = "Name";

        private void MaskedTextBoxPhone_TextChanged(object sender, EventArgs e) => user.Phone = maskedTextBoxPhone.Text;

        private void BtnListBoxClear_Click(object sender, EventArgs e) => listBox1.Items.Clear();
    }
}
