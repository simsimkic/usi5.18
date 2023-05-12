using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZdravoCorp.Domain;
using ZdravoCorp.Domain.UserClasses;
using ZdravoCorp.Repositories;
using ZdravoCorp.Services;
using ZdravoCorp.Views.DoctorViews;

namespace ZdravoCorp.Views
{
    public partial class DoctorDynamicEquipment : Window
    {
        private RoomService _roomService;
        private EquipmentService _eqService;
        private Hospital _hospital;
        private Room _selectedRoom;

        public DoctorDynamicEquipment(int roomId)
        {
            _hospital = new Hospital();
            InitializeComponent();
            LoadData();

            RoomService rs = new RoomService();
            rs.Refresh();
            _selectedRoom = rs.GetRoomById(roomId);
            tbRoomId.Text = roomId.ToString();
            cbEquipmentType.SelectionChanged += EquipmentTypeComboBox_SelectionChanged;
        }

        private void LoadData()
        {
            _roomService = new RoomService();
            _eqService = new EquipmentService();
            var rooms = _roomService.GetAllRooms();
            cbEquipmentType.ItemsSource = Equipment.GetDynamicNames();
        }
        private void EquipmentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCurrentQuantity();
        }

        private void UpdateCurrentQuantity()
        {
            if (cbEquipmentType.SelectedItem != null)
            {
                var selectedEquipmentName = (EquipmentName)cbEquipmentType.SelectedItem;
                int equipmentCount = _selectedRoom.EquipmentList.Count(e => e.Name == selectedEquipmentName);

                if (equipmentCount != null)
                {
                    tbCurrentQuantity.Text = equipmentCount.ToString();
                }
                else
                {
                    tbCurrentQuantity.Text = "0";
                }
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbUsedQuantity.Text == "" || cbEquipmentType.SelectedIndex == -1)
            {
                MessageBox.Show("You need to enter the value of used items.");
            }
            else if (!int.TryParse(tbUsedQuantity.Text, out _))
            {
                MessageBox.Show("Invalid input.");
            }
            else
            {
                int roomId = int.Parse(tbRoomId.Text);
                var selectedEquipmentName = (EquipmentName)cbEquipmentType.SelectedItem;
                int desiredCount = int.Parse(tbUsedQuantity.Text);

                Room room = _hospital.RoomService.GetRoomById(roomId);
                List<Equipment> equipmentList = room.EquipmentList;

                int currentCount = equipmentList.Count(equipment => equipment.Name == selectedEquipmentName);

                if (currentCount >= desiredCount)
                {
                    for (int i = 0; i < desiredCount; i++)
                    {
                        Equipment equipmentToRemove = equipmentList.Find(equipment => equipment.Name == selectedEquipmentName);
                        equipmentList.Remove(equipmentToRemove);
                        _eqService.DeleteEquipment(equipmentToRemove.Id);
                    }
                    _hospital.RoomService.UpdateRoom(room);
                    MessageBox.Show("Equipment count successfully updated.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (currentCount < desiredCount)
                {
                    MessageBox.Show( "The entered count is greater than the current count. Please enter a smaller value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnFinish_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult confirm = MessageBox.Show("Are you sure you want to finish?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                DoctorAllExaminations dae = new DoctorAllExaminations();
                dae.Show();
                this.Close();
            }
        }
    }
}