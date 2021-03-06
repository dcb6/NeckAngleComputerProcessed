﻿using MbientLab.MetaWear.Peripheral;
using static MbientLab.MetaWear.Functions;
using MbientLab.MetaWear.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MbientLab.MetaWear.Template {
    /// <summary>
    /// Blank page where users add their MetaWear commands
    /// </summary>
    public sealed partial class DeviceSetup : Page {
        /// <summary>
        /// Pointer representing the MblMwMetaWearBoard struct created by the C++ API
        /// </summary>
        private IntPtr cppBoard;
        //static AHRS.MadgwickAHRS AHRS = new AHRS.MadgwickAHRS(1f / 256f, 0.1f);

        public DeviceSetup() {
            this.InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            base.OnNavigatedTo(e);

            var mwBoard= MetaWearBoard.getMetaWearBoardInstance(e.Parameter as BluetoothLEDevice);
            cppBoard = mwBoard.cppBoard;

            // cppBoard is initialized at this point and can be used
        }

        /// <summary>
        /// Callback for the back button which tears down the board and navigates back to the <see cref="MainPage"/> page
        /// </summary>
        private void back_Click(object sender, RoutedEventArgs e) {
            mbl_mw_metawearboard_tear_down(cppBoard);

            this.Frame.Navigate(typeof(MainPage));
        }

        private Fn_IntPtr accDataHandler = new Fn_IntPtr(pointer => {
            System.Diagnostics.Debug.WriteLine("Acceleration:");
            Data data = Marshal.PtrToStructure<Data>(pointer);
            System.Diagnostics.Debug.WriteLine(Marshal.PtrToStructure<CartesianFloat>(data.value));
        });

        private Fn_IntPtr gyroDataHandler = new Fn_IntPtr(pointer => {
            System.Diagnostics.Debug.WriteLine("Gyroscope");
            Data data = Marshal.PtrToStructure<Data>(pointer);
            System.Diagnostics.Debug.WriteLine(Marshal.PtrToStructure<CartesianFloat>(data.value));
        });

        private Fn_IntPtr magDataHandler = new Fn_IntPtr(pointer => {
            System.Diagnostics.Debug.WriteLine("Magnetometer");
            Data data = Marshal.PtrToStructure<Data>(pointer);
            System.Diagnostics.Debug.WriteLine(Marshal.PtrToStructure<CartesianFloat>(data.value));
        });

        private void accStart_Click(object sender, RoutedEventArgs e)
        {
            //Accelerometer
            //IntPtr accSignal = mbl_mw_acc_get_acceleration_data_signal(cppBoard);

            //mbl_mw_datasignal_subscribe(accSignal, accDataHandler);
            //mbl_mw_acc_enable_acceleration_sampling(cppBoard);
            //mbl_mw_acc_start(cppBoard);
            // System.Diagnostics.Debug.WriteLine("Start button hit.");


            //Gyroscope
            //IntPtr stateSignal = mbl_mw_gyro_bmi160_get_rotation_data_signal(cppBoard);

            //mbl_mw_datasignal_subscribe(stateSignal, gyroDataHandler);
            //mbl_mw_gyro_bmi160_enable_rotation_sampling(cppBoard);
            //mbl_mw_gyro_bmi160_start(cppBoard);

            //Magnetometer
            IntPtr magSignal = mbl_mw_mag_bmm150_get_b_field_data_signal(cppBoard);

            mbl_mw_datasignal_subscribe(magSignal, magDataHandler);
            mbl_mw_mag_bmm150_enable_b_field_sampling(cppBoard);
            mbl_mw_mag_bmm150_start(cppBoard);

        }

        private void accStop_Click(object sender, RoutedEventArgs e)
        {
            IntPtr accSignal = mbl_mw_acc_get_acceleration_data_signal(cppBoard);

            mbl_mw_acc_stop(cppBoard);
            mbl_mw_acc_disable_acceleration_sampling(cppBoard);
            mbl_mw_datasignal_unsubscribe(accSignal);
        }
    }
}
