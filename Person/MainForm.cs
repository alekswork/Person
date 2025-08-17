using Person.Infrastructure.Persistence.Repositories;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Person
{
    public partial class MainForm : Form
    {
        private PersonRepository _personRepository;

        public MainForm(PersonRepository personRepository)
        {
            InitializeComponent();

            _personRepository = personRepository;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var newPerson = new Domain.Entities.Person { PersonalId = Guid.NewGuid(), BirthDate = DateTime.Now};

                var addEditPersonForm = new AddEditPersonForm(AddEditPersonFormEnum.Add, newPerson, _personRepository);
                if (addEditPersonForm.ShowDialog() == DialogResult.OK)
                {
                    RefreshGv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var person = gv.CurrentRow?.DataBoundItem as Domain.Entities.Person;
                if (person == null)
                {
                    MessageBox.Show("Необходимо выбрать строку для редактирования", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var addEditPersonForm = new AddEditPersonForm(AddEditPersonFormEnum.Edit, person, _personRepository);

                if (addEditPersonForm.ShowDialog() == DialogResult.OK)
                {
                    RefreshGv();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при редактировании записи: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gv.CurrentRow == null)
            {
                MessageBox.Show("Необходимо выбрать строку для удаления", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var person = gv.CurrentRow.DataBoundItem as Domain.Entities.Person;
            if (person == null)
            {
                MessageBox.Show("Ошибка: выбранная строка некорректна.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите удалить {person.LastName} {person.FirstName}?",
                                         "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _personRepository.Delete(person.PersonalId);

                    RefreshGv();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSaveInFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (gv.CurrentRow == null)
                {
                    MessageBox.Show("Необходимо выбрать строку для сохранения", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var person = gv.CurrentRow.DataBoundItem as Domain.Entities.Person;
                if (person == null)
                {
                    MessageBox.Show("Выбранная строка некорректна", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var sfd = new SaveFileDialog())
                {
                    sfd.Filter = "XML файлы (*.xml)|*.xml";
                    sfd.FileName = $"{person.PersonalId}.xml";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        var serializer = new XmlSerializer(typeof(Domain.Entities.Person));
                        using (var fs = new System.IO.FileStream(sfd.FileName, System.IO.FileMode.Create))
                        {
                            serializer.Serialize(fs, person);
                        }

                        MessageBox.Show("Сохранено в XML!", "Готово", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении в файл: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGv()
        {
            gv.DataSource = null;
            gv.DataSource = _personRepository.GetAll();

            gv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            gv.Columns["PersonalId"].HeaderText = "Персональный ID";
            gv.Columns["LastName"].HeaderText = "Фамилия";
            gv.Columns["FirstName"].HeaderText = "Имя";
            gv.Columns["Patronymic"].HeaderText = "Отчество";

            gv.Columns["BirthDate"].HeaderText = "Дата рождения";
            gv.Columns["Email"].HeaderText = "Электронная почта";
            gv.Columns["Phone"].HeaderText = "Телефон";

            gv.Columns["BirthDate"].Visible = false;
            gv.Columns["Email"].Visible = false;
            gv.Columns["Phone"].Visible = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                RefreshGv();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке формы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
