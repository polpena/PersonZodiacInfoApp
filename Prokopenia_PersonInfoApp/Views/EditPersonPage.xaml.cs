using System.Windows.Controls;
using PersonInfoApp.Models;
using PersonInfoApp.ViewModels;

namespace PersonInfoApp.Views
{
    /// <summary>
    /// Interaction logic for EditPersonPage.xaml.
    /// </summary>
    public partial class EditPersonPage : Page
    {
        public EditPersonPage(Person selectedUser)
        {
            InitializeComponent();
            DataContext = new EditPersonViewModel(selectedUser);
        }
    }
}