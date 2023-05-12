using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Views.ManagerViews
{
    public partial class TransferRequestsWindow : Window
    {
        public TransferRequestsWindow()
        {
            InitializeComponent();
            _loadTransferRequests();
        }

        private void _loadTransferRequests()
        {
            string filePath = @"..\..\..\Data\equipmentTransferRequests.json";
            
            if (!File.Exists(filePath))
            {
                MessageBox.Show("No transfer requests found.");
                return;
            }

            string jsonData = File.ReadAllText(filePath);
            List<EquipmentTransfer> transfers = JsonSerializer.Deserialize<List<EquipmentTransfer>>(jsonData);

            TransferRequestsDataGrid.ItemsSource = transfers;
        }
    }
}