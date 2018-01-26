﻿using LimitManagement.Win;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LimitManagement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = string.Format("额度管理 (Version {0}, Date {1})", Utils.Version, Utils.UpdateDate);
        }

        private void miTotalImport_Click(object sender, RoutedEventArgs e)
        {
            var winImport = new winLimitImport();
            winImport.ShowDialog();
        }

        private void miDeleteImport_Click(object sender, RoutedEventArgs e)
        {
            var winImport = new winLimitImport(true);
            winImport.ShowDialog();
        }
    }
}
