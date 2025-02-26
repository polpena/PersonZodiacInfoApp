using System.Windows.Controls;
using PersonInfoApp.Models;
using PersonInfoApp.ViewModels;

namespace PersonInfoApp.Views
{
    /// <summary>
    /// Interaction logic for PersonDetailPage.xaml
    /// </summary>
    public partial class PersonDetailPage : Page
    {
        public PersonDetailPage(Person selectedUser)
        {
            InitializeComponent();
            DataContext = new PersonDetailViewModel(selectedUser);
        }
    }
}
