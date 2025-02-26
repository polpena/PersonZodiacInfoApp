using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using PersonInfoApp.Models;
using PersonInfoApp.Services;

namespace PersonInfoApp.ViewModels
{
    /// <summary>
    /// Singleton ViewModel that holds the shared collection of users.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private static MainViewModel _instance;
        public static MainViewModel Instance => _instance ??= new MainViewModel();

        /// <summary>
        /// Shared collection of persons.
        /// </summary>
        public ObservableCollection<Person> Users { get; } = new ObservableCollection<Person>(); 

        private MainViewModel()
        {
            Application.Current.Dispatcher.BeginInvoke(new System.Action(async () =>
            {
                await LoadUsersAsync();
            }));
        }

        /// <summary>
        /// Loads users from persistent storage and updates the shared collection.
        /// </summary>
        private async Task LoadUsersAsync()
        {
            var loadedUsers = await UserDataService.LoadUsersAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Users.Clear();
                foreach (var user in loadedUsers)
                {
                    Users.Add(user);
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
