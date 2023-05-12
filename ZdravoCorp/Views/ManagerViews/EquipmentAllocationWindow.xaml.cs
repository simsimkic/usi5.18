using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualBasic;
using ZdravoCorp.Domain;
using ZdravoCorp.Repositories;

namespace ZdravoCorp.Views.ManagerViews
{
    public partial class EquipmentAllocationWindow : Window
    {

        private Hospital _hospital { get; set; }
        private int DonorId { get; set; }
        private int TargetId { get; set; }
        private int EquipmentId { get; set; }
        public EquipmentAllocationWindow()
        {
            InitializeComponent();

            _hospital = new Hospital();
            
            _loadLowStockRooms();
            _loadEquipment();

            DonorIdTextBox.TextChanged += _donorIdTextBox_TextChanged;
            TargetIdTextBox.TextChanged += _targetIdTextBox_TextChanged;
            EquipmentIdTextBox.TextChanged += _equipmentIdTextBox_TextChanged;
        }
        
        private void _backToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var landingPageWindow = new ManagerLandingPage();
            landingPageWindow.Show();
            this.Close();
        }

        private void _loadLowStockRooms()
        {
            var lowStockDict = _hospital.EquipmentService.GetLowStockEquipmentPerRoom();
            
            var rows = new List<dynamic>();
            foreach (var kvp in lowStockDict)
            {
                var stock = kvp.Value;
                var stockString = "";

                foreach (var kvp2 in stock)
                {
                    if (kvp2.Value == 0)
                    {
                        stockString += (kvp2.Key + ": " + kvp2.Value).ToUpper() + "\r\n";
                    }
                    else
                    {
                        stockString += kvp2.Key + ": " + kvp2.Value + "\r\n";
                    }
                }
                
                rows.Add(
                    new { RoomId = kvp.Key.Id, RoomType = kvp.Key.Type, LowStock = stockString }
                );
            }
            
            LowStockDataGrid.ItemsSource = rows;
        }

        private void _loadEquipment()
        {
            List<Equipment> equipment = _hospital.EquipmentService.GetAllEquipment();
            AvailableEquipmentDataGrid.ItemsSource = equipment;
        }
        
        private void _datePicker_Loaded(object sender, RoutedEventArgs e)
        {
            TransferDatePicker.DisplayDateStart = DateTime.Today;
            TransferDatePicker.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, DateTime.Today.AddDays(-1)));
        }

        private void _validateRoomIds()
        {
            if (int.TryParse(DonorIdTextBox.Text, out int donorRoomId))
            {
                if (_hospital.RoomService.RoomExists(donorRoomId))
                {
                    DonorId = donorRoomId;
                    DonorIdErrorText.Visibility = Visibility.Collapsed;
                }
                else
                {
                    DonorIdErrorText.Visibility = Visibility.Visible;
                    DonorIdErrorText.Text = "Room Id does not exist!";
                }
            }
            else
            {
                DonorIdErrorText.Visibility = Visibility.Visible;
                DonorIdErrorText.Text = "Invalid Room Id!";
            }
            
            
            if (int.TryParse(TargetIdTextBox.Text, out int targetRoomId))
            {
                if (_hospital.RoomService.RoomExists(targetRoomId))
                {
                    TargetId = targetRoomId;
                    TargetIdErrorText.Visibility = Visibility.Collapsed;
                }
                else
                {
                    TargetIdErrorText.Visibility = Visibility.Visible;
                    TargetIdErrorText.Text = "Target Id does not exist!";
                }
            }
            else
            {
                TargetIdErrorText.Visibility = Visibility.Visible;
                TargetIdErrorText.Text = "Invalid Target Id!";
            }
        }
        
        private void _donorIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _validateRoomIds();
        }
        
        private void _targetIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _validateRoomIds();
        }

        private void _equipmentIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(EquipmentIdTextBox.Text, out int equipmentId))
            {
                EquipmentIdErrorText.Visibility = Visibility.Visible;
                EquipmentIdErrorText.Text = "Invalid Equipment Id!";
                return;
            }
            
            if (!_hospital.EquipmentService.EquipmentExists(equipmentId))
            {
                EquipmentIdErrorText.Visibility = Visibility.Visible;
                EquipmentIdErrorText.Text = "Equipment Id does not exist!";
                return;
            }
            
            var room = _hospital.RoomService.GetRoomById(DonorId);
            if (room.EquipmentList.All(eq => eq.Id != equipmentId))
            {
                EquipmentIdErrorText.Visibility = Visibility.Visible;
                EquipmentIdErrorText.Text = $"That room does not contain equipment with Id {EquipmentIdTextBox.Text}!";
                return;
            }
            
            EquipmentId = equipmentId;
            EquipmentIdErrorText.Visibility = Visibility.Collapsed;
        }
        
        private void _viewTransferRequestsButton_Click(object sender, RoutedEventArgs e)
        {
            TransferRequestsWindow transferRequestsWindow = new TransferRequestsWindow();
            transferRequestsWindow.ShowDialog();
        }
        
        // NOTE: this could be made easier with suggested IDs based on name and/or type
        private void _transferButton_Click(object sender, RoutedEventArgs e)
        {
            if (
                string.IsNullOrEmpty(DonorIdTextBox.Text) ||
                string.IsNullOrEmpty(TargetIdTextBox.Text) ||
                string.IsNullOrEmpty(EquipmentIdTextBox.Text) ||
                (
                    // date can be left empty if the equipment is dynamic
                    !TransferDatePicker.SelectedDate.HasValue &&
                    _hospital.EquipmentService.GetEquipmentById(EquipmentId).Type != EquipmentType.Dynamic
                    )
                )
            {
                MessageBox.Show("Please fill in all fields and select the target room and equipment.");
                return;
            }
            
            int donorId = DonorId;
            int targetId = TargetId;
            int equipmentId = EquipmentId;

            DateTime transferTime = DateTime.Now;
            if (TransferDatePicker.SelectedDate != null)
            {
                transferTime = TransferDatePicker.SelectedDate.Value;
            }

            
            if (_hospital.EquipmentService.TransferRequestExists(equipmentId))
            {
                MessageBoxResult result = MessageBox.Show("Equipment transfer request already exists! Do you want to overwrite it?", "Transfer Request Exists", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }
            
            _transferEquipment(donorId, targetId, equipmentId, transferTime);

            _loadLowStockRooms();
            _loadEquipment();

            if (_hospital.EquipmentService.GetEquipmentById(equipmentId).Type == EquipmentType.Dynamic)
            {
                MessageBox.Show("Equipment transferred successfully.");
            }
            else
            {
                MessageBox.Show("Equipment transfer request created successfully.");
            }
        }

        private void _transferEquipment(int donorId, int targetId, int equipmentId, DateTime transferTime)
        {
            var eq = _hospital.EquipmentService.GetEquipmentById(EquipmentId);
            
            if (eq.Type == EquipmentType.Dynamic)
            {
                var donorRoom = _hospital.RoomService.GetRoomById(donorId);
                var targetRoom = _hospital.RoomService.GetRoomById(targetId);
                
                donorRoom.RemoveEquipment(eq);
                targetRoom.AddEquipment(eq);
                
                _hospital.RoomService.UpdateRoom(targetRoom);
                _hospital.EquipmentService.UpdateEquipmentLocationById(equipmentId, targetRoom);
            }
            else
            {
                _hospital.EquipmentService.CreateTransferRequest(donorId, targetId, equipmentId, transferTime);
            }
        }
    }
}
