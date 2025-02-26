using System.Windows.Controls;
using PersonInfoApp.ViewModels;

namespace PersonInfoApp.Views
{
    /// <summary>
    /// Interaction logic for AddPersonPage.xaml
    /// </summary>
    public partial class AddPersonPage : Page
    {
        public AddPersonPage()
        {
            InitializeComponent();
            DataContext = new AddPersonViewModel();
        }
    }
}
