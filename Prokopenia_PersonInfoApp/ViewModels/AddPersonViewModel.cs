using System.Windows;
using System.Windows.Input;
using PersonInfoApp.Models;
using System.ComponentModel;
using PersonInfoApp.Services;

namespace PersonInfoApp.ViewModels
{
    /// <summary>
    /// ViewModel for adding a new Person.
    /// </summary>
    public class AddPersonViewModel : INotifyPropertyChanged
    {
        private string _firstName, _lastName, _email;
        private DateTime? _birthDate;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); UpdateCanExecute(); }
        }
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); UpdateCanExecute(); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); UpdateCanExecute(); }
        }
        public DateTime? BirthDate
        {
            get => _birthDate;
            set { _birthDate = value; OnPropertyChanged(nameof(BirthDate)); UpdateCanExecute(); }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand NavigateToListCommand { get; }

        public AddPersonViewModel()
        {
            AddPersonCommand = new RelayCommand(async _ => await AddPersonAsync(), _ => CanAddPerson());
            NavigateToListCommand = new RelayCommand(_ => NavigateToList());
        }

        /// <summary>
        /// Adds a person to the shared collection and saves them.
        /// </summary>
        private async Task AddPersonAsync()
        {
            try
            {
                var person = new Person(FirstName, LastName, Email, BirthDate.Value);

                MainViewModel.Instance.Users.Add(person);

                UserDataService.SaveUsers(MainViewModel.Instance.Users.ToList());

                FirstName = LastName = Email = string.Empty;
                BirthDate = null;

                NavigateToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Navigates to the users list page.
        /// </summary>
        private void NavigateToList() =>
            Application.Current.MainWindow.Content = new Views.UsersListPage();

        /// <summary>
        /// Determines whether a new person can be added.
        /// </summary>
        private bool CanAddPerson() =>
           !string.IsNullOrWhiteSpace(FirstName) &&
           !string.IsNullOrWhiteSpace(LastName) &&
           !string.IsNullOrWhiteSpace(Email) &&
           BirthDate.HasValue;

        /// <summary>
        /// Updates the command's executable state.
        /// </summary>
        private void UpdateCanExecute() =>
            (AddPersonCommand as RelayCommand)?.RaiseCanExecuteChanged();
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) => 
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(prop));
    }
}
