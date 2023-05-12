using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZdravoCorp.Domain;

namespace ZdravoCorp.Views.ManagerViews
{
    public partial class EquipmentManagementWindow : Window
    {
        private Hospital _hospital;

        public List<Equipment> currentDataList;
        public EquipmentManagementWindow()
        {
            InitializeComponent();
            _initializeHospital();
            _displayAllEquipment();
        }

        private void _initializeHospital()
        {
            _hospital = new Hospital();
        }
        
        private void _backToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var landingPageWindow = new ManagerLandingPage();
            landingPageWindow.Show();
            this.Close();
        }

        private void _updateTable(List<Equipment> newData)
        {
            currentDataList = newData;
            EquipmentDataGrid.ItemsSource = newData;
        }
        
        private void _displayAllEquipment()
        {
            _updateTable(_hospital.EquipmentService.GetAllEquipment());
        }

        private void _numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Use a regex pattern to only allow numbers
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

            // (if found any non-digit chars)
            if (e.Handled)
            {
                MessageBox.Show("Please enter only numbers.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);    
            }
        }

        private void _searchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchBox.Text;
            var searchResults = _hospital.EquipmentService.SearchEquipment(searchText, currentDataList);
            _updateTable(searchResults);
        }

        private void _filterButton_Click(object sender, RoutedEventArgs e)
        {
            RoomType? roomTypeParameter = null;
            EquipmentType? equipmentTypeParameter = null;
            int? minEquipmentQuantity = null;
            int? maxEquipmentQuantity = null;
            bool excludeStorage = false;
            
            // Room type 
            if (OperatingRoomCheckBox.IsChecked == true)
            { roomTypeParameter = RoomType.OperatingRoom; }

            if (ExaminationRoomCheckBox.IsChecked == true)
            { roomTypeParameter = RoomType.ExaminationRoom; }

            if (PatientRoomCheckBox.IsChecked == true)
            { roomTypeParameter = RoomType.PatientRoom; }

            if (OperatingRoomCheckBox.IsChecked == true)
            { roomTypeParameter = RoomType.OperatingRoom; }

            if (WaitingRoomCheckbox.IsChecked == true)
            { roomTypeParameter = RoomType.WaitingRoom; }

            if (ExcludeStorage.IsChecked == true)
            { excludeStorage = true; }

            // Equipment type
            if (OperationTypeCheckBox.IsChecked == true)
            { equipmentTypeParameter = EquipmentType.Operation; } 

            if (ExaminationTypeCheckBox.IsChecked == true)
            { equipmentTypeParameter = EquipmentType.Examination; } 

            if (FurnitureTypeCheckBox.IsChecked == true)
            { equipmentTypeParameter = EquipmentType.Furniture; } 

            if (HallwayTypeCheckBox.IsChecked == true)
            { equipmentTypeParameter = EquipmentType.Hallway; } 
            
            if (DynamicTypeCheckBox.IsChecked == true)
            { equipmentTypeParameter = EquipmentType.Dynamic; } 

            // Quantity
            if (MinQuantityTextBox.Text.Length > 0)
            { minEquipmentQuantity = int.Parse(MinQuantityTextBox.Text); }

            if (MaxQuantityTextBox.Text.Length > 0)
            { maxEquipmentQuantity = int.Parse(MaxQuantityTextBox.Text); }

            var filteredEquipment = _hospital.EquipmentService.FilterEquipment(
                roomTypeParameter, equipmentTypeParameter,
                minEquipmentQuantity, maxEquipmentQuantity, excludeStorage);
            
            _updateTable(filteredEquipment);
        }
        
        private void _filterRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (EquipmentDataGrid == null) return; // To avoid null reference exception during the initial loading

            var radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                var filteredEquipment = _hospital.EquipmentService.GetAllEquipment();
                switch (radioButton.Name)
                {
                    case "DynamicEquipmentRadioButton":
                        filteredEquipment = _hospital.EquipmentService.FilterEquipment(equipmentTypeFilter: EquipmentType.Dynamic);
                        break;
                    
                    case "StandardEquipmentRadioButton":
                        filteredEquipment = new List<Equipment>();
                        filteredEquipment.AddRange(_hospital.EquipmentService.FilterEquipment(equipmentTypeFilter: EquipmentType.Examination));
                        filteredEquipment.AddRange(_hospital.EquipmentService.FilterEquipment(equipmentTypeFilter: EquipmentType.Operation));
                        filteredEquipment.AddRange(_hospital.EquipmentService.FilterEquipment(equipmentTypeFilter: EquipmentType.Furniture));
                        filteredEquipment.AddRange(_hospital.EquipmentService.FilterEquipment(equipmentTypeFilter: EquipmentType.Hallway));
                        break;
                    
                    case "BothEquipmentRadioButton":
                        break;
                }

                _updateTable(filteredEquipment);
            }
        }
    }
}
