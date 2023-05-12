using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Windows;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Views.ManagerViews
{
    public partial class DynamicEquipmentRequestWindow : Window
    {
        private Hospital _hospital;
        private Dictionary<EquipmentName, int> _lowStockEquipment { get; set; }

        public DynamicEquipmentRequestWindow()
        {
            InitializeComponent();
            _hospital = new Hospital();
            _loadLowStockEquipment();
            _loadRequests();
            _populateEquipmentComboBox();
        }
        
        private void _backToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var landingPageWindow = new ManagerLandingPage();
            landingPageWindow.Show();

            this.Close();
        }
        
        private void _populateEquipmentComboBox()
        {
            if (_lowStockEquipment.Count == 0)
            {
                EquipmentComboBox.Items.Add("No low stock equipment");
                EquipmentComboBox.SelectedItem = "No low stock equipment";
            }
            else
            {
                foreach (var equipment in _lowStockEquipment)
                {
                    EquipmentComboBox.Items.Add($"{equipment.Key}: {equipment.Value}");
                }
            }
        }

        private void _loadLowStockEquipment()
        {
            _lowStockEquipment = _hospital.EquipmentService.GetLowStockEquipmentTotal();
            LowStockEquipmentDataGrid.ItemsSource = _lowStockEquipment;
        }

        private void _loadRequests()
        {
            string jsonFilePath = @"..\..\..\Data\dynamicEquipmentRequests.json";
            
            string jsonData = File.ReadAllText(jsonFilePath);
            var requests = JsonSerializer.Deserialize<List<EquipmentRequest>>(jsonData);

            RequestsDataGrid.ItemsSource = requests;
        }

        private void _sendRequestButton_Click(object sender, RoutedEventArgs e)
        {
            var nameItem = EquipmentComboBox.SelectedItem;

            if (nameItem == null)
            {
                MessageBox.Show("Please select a item if there are any.");
                return;
            }

            if (nameItem.ToString() == "No low stock equipment")
            {
                MessageBox.Show("There is no equipment low in stock.");
                return;
            }

            var qty = QuantityTextBox.Text;

            if (qty == "")
            {
                MessageBox.Show("Please select a quantity.");
                return;
            }

            var eqString = nameItem.ToString().Split(":")[0];
            var eqQty = int.Parse(qty);

            Enum.TryParse(eqString, out EquipmentName eqName);

            _hospital.EquipmentService.CreateEquipmentRequest(eqName, eqQty);

            _loadRequests();
            MessageBox.Show($"Request {eqName}: {eqQty} created successfully.");
        }
        private void _cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}