using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using PersonInfoApp.Models;

namespace PersonInfoApp.ViewModels
{
    /// <summary>
    /// ViewModel for managing and filtering a list of users.
    /// </summary>
    public class UsersListViewModel : INotifyPropertyChanged
    {
        // Shared collection from the singleton MainViewModel
        public ObservableCollection<Person> Users => MainViewModel.Instance.Users;

        private ICollectionView _usersView;
        /// <summary>
        /// Gets the collection view for filtering and sorting
        /// </summary>
        public ICollectionView UsersView
        {
            get => _usersView;
            set { _usersView = value; OnPropertyChanged(nameof(UsersView)); }
        }

        private Person _selectedUser;
        public Person SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }
        }

        // Filter properties
        private string _filterFirstName;
        public string FilterFirstName
        {
            get => _filterFirstName;
            set { _filterFirstName = value; OnPropertyChanged(nameof(FilterFirstName)); UsersView.Refresh(); }
        }

        private string _filterLastName;
        public string FilterLastName
        {
            get => _filterLastName;
            set { _filterLastName = value; OnPropertyChanged(nameof(FilterLastName)); UsersView.Refresh(); }
        }

        private string _filterEmail;
        public string FilterEmail
        {
            get => _filterEmail;
            set { _filterEmail = value; OnPropertyChanged(nameof(FilterEmail)); UsersView.Refresh(); }
        }

        // Nullable bool: null means no filter, otherwise true/false.
        private bool? _filterIsAdult;
        public bool? FilterIsAdult
        {
            get => _filterIsAdult;
            set { _filterIsAdult = value; OnPropertyChanged(nameof(FilterIsAdult)); UsersView.Refresh(); }
        }

        private bool? _filterIsBirthday;
        public bool? FilterIsBirthday
        {
            get => _filterIsBirthday;
            set { _filterIsBirthday = value; OnPropertyChanged(nameof(FilterIsBirthday)); UsersView.Refresh(); }
        }

        // Exact date filter and date range filters
        private DateTime? _filterBirthDateExact;
        public DateTime? FilterBirthDateExact
        {
            get => _filterBirthDateExact;
            set { _filterBirthDateExact = value; OnPropertyChanged(nameof(FilterBirthDateExact)); UsersView.Refresh(); }
        }

        private DateTime? _filterBirthDateStart;
        public DateTime? FilterBirthDateStart
        {
            get => _filterBirthDateStart;
            set { _filterBirthDateStart = value; OnPropertyChanged(nameof(FilterBirthDateStart)); UsersView.Refresh(); }
        }

        private DateTime? _filterBirthDateEnd;
        public DateTime? FilterBirthDateEnd
        {
            get => _filterBirthDateEnd;
            set { _filterBirthDateEnd = value; OnPropertyChanged(nameof(FilterBirthDateEnd)); UsersView.Refresh(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use exact date filtering (true) or a date range (false).
        /// </summary>
        private bool _useExactBirthDateFilter;
        public bool UseExactBirthDateFilter
        {
            get => _useExactBirthDateFilter;
            set { _useExactBirthDateFilter = value; OnPropertyChanged(nameof(UseExactBirthDateFilter)); UsersView.Refresh(); }
        }

        private string _filterSunSign;
        public string FilterSunSign
        {
            get => _filterSunSign;
            set { _filterSunSign = value; OnPropertyChanged(nameof(FilterSunSign)); UsersView.Refresh(); }
        }

        private string _filterChineseSign;
        public string FilterChineseSign
        {
            get => _filterChineseSign;
            set { _filterChineseSign = value; OnPropertyChanged(nameof(FilterChineseSign)); UsersView.Refresh(); }
        }

        // Navigation commands
        public ICommand NavigateToAddCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand EditPersonCommand { get; }
        public ICommand DeletePersonCommand { get; }
        public ICommand ClearFiltersCommand { get; }

        public UsersListViewModel()
        {
            NavigateToAddCommand = new RelayCommand(_ => NavigateToAdd());
            ShowDetailsCommand = new RelayCommand(_ => ShowDetails(), _ => SelectedUser != null);
            EditPersonCommand = new RelayCommand(_ => EditPerson(), _ => SelectedUser != null);
            DeletePersonCommand = new RelayCommand(_ => DeletePerson(), _ => SelectedUser != null);
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());

            UsersView = CollectionViewSource.GetDefaultView(Users);
            UseExactBirthDateFilter = true;
            FilterSunSign = "All";
            FilterChineseSign = "All";
            UsersView.Filter = FilterUsers;
        }

        /// <summary>
        /// Applies all active filters to a Person object.
        /// </summary>
        private bool FilterUsers(object item)
        {
            if (item is Person person)
            {
                if (!string.IsNullOrWhiteSpace(FilterFirstName) && !SafeRegexMatch(person.FirstName, FilterFirstName))
                    return false;
                if (!string.IsNullOrWhiteSpace(FilterLastName) && !SafeRegexMatch(person.LastName, FilterLastName))
                    return false;
                if (!string.IsNullOrWhiteSpace(FilterEmail) && !SafeRegexMatch(person.Email, FilterEmail))
                    return false;

                // Birth date filtering
                if (UseExactBirthDateFilter)
                {
                    if (FilterBirthDateExact.HasValue && person.BirthDate.Date != FilterBirthDateExact.Value.Date)
                        return false;
                }
                else
                {
                    if (FilterBirthDateStart.HasValue && person.BirthDate.Date < FilterBirthDateStart.Value.Date)
                        return false;
                    if (FilterBirthDateEnd.HasValue && person.BirthDate.Date > FilterBirthDateEnd.Value.Date)
                        return false;
                }

                
                if (FilterIsAdult.HasValue && person.IsAdult != FilterIsAdult.Value)
                    return false;
                if (FilterIsBirthday.HasValue && person.IsBirthday != FilterIsBirthday.Value)
                    return false;
                if (!string.IsNullOrWhiteSpace(FilterSunSign) && FilterSunSign != "All" &&
                    (person.SunSign == null || !person.SunSign.Equals(FilterSunSign, StringComparison.OrdinalIgnoreCase)))
                    return false;
                if (!string.IsNullOrWhiteSpace(FilterChineseSign) && FilterChineseSign != "All" &&
                    (person.ChineseSign == null || !person.ChineseSign.Equals(FilterChineseSign, StringComparison.OrdinalIgnoreCase)))
                    return false;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to match a source string against a regular expression pattern. If the pattern is invalid, falls back to a case-insensitive Contains.
        /// </summary>
        private bool SafeRegexMatch(string source, string pattern)
        {
            try
            {
                return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return source.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        /// <summary>
        /// Resets all filter properties to their default values.
        /// </summary>
        private void ClearFilters()
        {
            FilterFirstName = FilterLastName = FilterEmail = string.Empty;
            FilterBirthDateStart = FilterBirthDateEnd = FilterBirthDateExact = null;
            FilterIsAdult = FilterIsBirthday = null;
            FilterSunSign = FilterChineseSign = "All";
        }

        private void NavigateToAdd() =>
            Application.Current.MainWindow.Content = new Views.AddPersonPage();

        private void ShowDetails() =>
           Application.Current.MainWindow.Content = new Views.PersonDetailPage(SelectedUser);

        private void EditPerson() =>
           Application.Current.MainWindow.Content = new Views.EditPersonPage(SelectedUser);

        private void DeletePerson()
        {
            if (SelectedUser == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete {SelectedUser.FirstName} {SelectedUser.LastName}?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                MainViewModel.Instance.Users.Remove(SelectedUser);
                PersonInfoApp.Services.UserDataService.SaveUsers(MainViewModel.Instance.Users.ToList());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string prop) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

