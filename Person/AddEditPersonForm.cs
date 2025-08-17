using Person.Infrastructure.Persistence.Repositories;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Person
{
    public partial class AddEditPersonForm : Form
    {
        private AddEditPersonFormEnum _addOrEdit;
        private Domain.Entities.Person _person;
        private PersonRepository _personRepository;

        private int _seconds = 0;
        private int _minutes = 0;
        private int _hours = 0;

        public AddEditPersonForm(AddEditPersonFormEnum addOrEdit, Domain.Entities.Person person, PersonRepository personRepository)
        {
            InitializeComponent();
            _person = person;
            _personRepository = personRepository;
            _addOrEdit = addOrEdit;

            textBoxPersonalId.DataBindings.Add("Text", _person, "PersonalId", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxLastName.DataBindings.Add("Text", _person, "LastName", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxFirstName.DataBindings.Add("Text", _person, "FirstName", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxPatronymic.DataBindings.Add("Text", _person, "Patronymic", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxEmail.DataBindings.Add("Text", _person, "Email", false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxPhone.DataBindings.Add("Text", _person, "Phone", false, DataSourceUpdateMode.OnPropertyChanged);
            dateTimeBirthDate.DataBindings.Add("Value", _person, "BirthDate", false, DataSourceUpdateMode.OnPropertyChanged);

            timer.Start();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_person.PersonalId == Guid.Empty ||
                string.IsNullOrWhiteSpace(_person.LastName) ||
                string.IsNullOrWhiteSpace(_person.FirstName) ||
                string.IsNullOrWhiteSpace(_person.Patronymic) ||
                string.IsNullOrWhiteSpace(_person.Email) ||
                string.IsNullOrWhiteSpace(_person.Phone) ||
                _person.BirthDate == null)
            {
                MessageBox.Show("Все поля обязательны для заполнения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_addOrEdit == AddEditPersonFormEnum.Add)
                {
                    _personRepository.Add(_person);
                }
                else if (_addOrEdit == AddEditPersonFormEnum.Edit)
                {
                    _personRepository.Edit(_person);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                // Собираем все ошибки в одну строку
                var errorMessages = ex.EntityValidationErrors
                                      .SelectMany(eve => eve.ValidationErrors)
                                      .Select(ve => $"{ve.PropertyName}: {ve.ErrorMessage}");

                string fullErrorMessage = string.Join(Environment.NewLine, errorMessages);

                MessageBox.Show($"Ошибки при сохранении данных:\n{fullErrorMessage}",
                                "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _seconds++;
            if (_seconds >= 60)
            {
                _seconds = 0;
                _minutes++;
            }
            if (_minutes >= 60)
            {
                _minutes = 0;
                _hours++;
            }

            lblTimer.Text = $"{_hours:D2}:{_minutes:D2}:{_seconds:D2}";
        }

        private void AddEditPersonForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
        }
    }
}
