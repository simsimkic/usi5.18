using System.IO;
using System.Windows;
using ZdravoCorp.Views.LoginViews;

namespace ZdravoCorp.Views.ManagerViews;

public partial class ManagerLandingPage : Window
{
    public ManagerLandingPage()
    {
        InitializeComponent();
    }

    private void _openEquipmentManagementWindow(object sender, RoutedEventArgs e)
    {
        var equipmentManagementWindow = new EquipmentManagementWindow();
        equipmentManagementWindow.Show();
        this.Close();
    }
    
    private void _openDynamicEquipmentRequestWindow(object sender, RoutedEventArgs e)
    {
        var dynamicEquipmentRequestWindow = new DynamicEquipmentRequestWindow();
        dynamicEquipmentRequestWindow.Show();
        this.Close();
    }
    
    private void _openEquipmentAllocationWindow(object sender, RoutedEventArgs e)
    {
        var equipmentAllocationWindow = new EquipmentAllocationWindow();
        equipmentAllocationWindow.Show();
        this.Close();
    }
    private void _logout_Click(object sender, RoutedEventArgs e){
        LoginScreen ls = new LoginScreen();
        ls.Show();
        this.Close();
    }
}