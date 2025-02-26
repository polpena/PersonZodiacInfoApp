using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using PersonInfoApp.Models;

namespace PersonInfoApp.ViewModels
{
    /// <summary>
    /// ViewModel for editing an existing Person
    /// </summary>
    public class EditPersonViewModel : INotifyPropertyChanged
    {
        // Original person to be edited
        private readonly Person _originalPerson;

        // Editable properties
        private string _firstName;
        public string FirstName { get => _firstName; set { _firstName = value; OnPropertyChanged(nameof(FirstName)); UpdateCanExecute(); } }

        private string _lastName;
        public string LastName { get => _lastName; set { _lastName = value; OnPropertyChanged(nameof(LastName)); UpdateCanExecute(); } }

        private string _email;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(nameof(Email)); UpdateCanExecute(); } }

        private DateTime? _birthDate;
        public DateTime? BirthDate { get => _birthDate; set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); UpdateCanExecute(); } }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditPersonViewModel(Person person)
        {
            _originalPerson = person;
            FirstName = person.FirstName;
            LastName = person.LastName;
            Email = person.Email;
            BirthDate = person.BirthDate;

            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private async System.Threading.Tasks.Task SaveAsync()
        {
            try
            {
                var updatedPerson = new Person(FirstName, LastName, Email, BirthDate.Value);
                updatedPerson.ComputeProperties();

                var users = MainViewModel.Instance.Users;
                int index = users.IndexOf(_originalPerson);
                if (index >= 0)
                {
                    users[index] = updatedPerson;
                    PersonInfoApp.Services.UserDataService.SaveUsers(users.ToList());
                }

                Application.Current.MainWindow.Content = new Views.UsersListPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel() =>
            Application.Current.MainWindow.Content = new Views.UsersListPage();

        private bool CanSave() =>
            !string.IsNullOrWhiteSpace(FirstName) &&
            !string.IsNullOrWhiteSpace(LastName) &&
            !string.IsNullOrWhiteSpace(Email) &&
            BirthDate.HasValue;

        private void UpdateCanExecute() =>
            (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
