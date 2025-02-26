using System.ComponentModel;
using System.Windows.Input;
using PersonInfoApp.Models;
using System.Windows;

namespace PersonInfoApp.ViewModels
{
    /// <summary>
    /// ViewModel for displaying details of a selected person.
    /// </summary>
    public class PersonDetailViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the selected person.
        /// </summary>
        public Person SelectedUser { get; }

        /// <summary>
        /// Command to navigate back to the users list.
        /// </summary>
        public ICommand BackToListCommand { get; }

        public PersonDetailViewModel(Person selectedUser)
        {
            SelectedUser = selectedUser;
            BackToListCommand = new RelayCommand(_ => BackToList());
        }

        /// <summary>
        /// Navigates back to the users list page.
        /// </summary>
        private void BackToList() =>
            Application.Current.MainWindow.Content = new Views.UsersListPage();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
