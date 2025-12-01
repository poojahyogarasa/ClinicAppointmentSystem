using System.Windows.Controls;

namespace Clinic.App.Views
{
    public partial class DoctorsView : UserControl
    {
        public DoctorsView() { InitializeComponent(); }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is Clinic.App.ViewModels.DoctorsViewModel vm)
                vm.LoadFromSelection();
        }
    }
}
