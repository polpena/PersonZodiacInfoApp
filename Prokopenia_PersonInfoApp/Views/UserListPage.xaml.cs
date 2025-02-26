using System.Windows.Controls;
using PersonInfoApp.ViewModels;

namespace PersonInfoApp.Views
{
    /// <summary>
    /// Interaction logic for UserListPage.xaml
    /// </summary>
    public partial class UsersListPage : Page
    {
        public UsersListPage()
        {
            InitializeComponent();
            DataContext = new UsersListViewModel();
        }
    }
}

