using DataComparision.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace DataComparision.Utils
{
    public static class ControlUtils
    {
        const string loadingName = "winLoading";
        internal static void ShowLoading(Loading loading, string LoadingDescription = null)
        {
            loading.Visibility = System.Windows.Visibility.Visible;
            if (!string.IsNullOrEmpty(LoadingDescription) )
            {
                loading.Description = LoadingDescription;
            }
        }

        internal static void ShowLoading(this Window window, string LoadingDescription = null)
        {
            Loading loading = null;
            var ctrl = window.FindName(loadingName);
            if (ctrl == null)
            {
                var grid = window.Content as Grid;
                if (grid != null)
                {
                    loading = new Loading() { Name = loadingName };
                    grid.Children.Add(loading);
                    Grid.SetRowSpan(loading, Math.Max(grid.RowDefinitions.Count, 1) );
                    Grid.SetColumnSpan(loading, Math.Max(grid.ColumnDefinitions.Count, 1));
                }
            }
            else
            {
                loading = ctrl as Loading;
                
            }
            if (loading != null)
            {
                loading.Visibility = Visibility.Visible;
                if (!string.IsNullOrEmpty(LoadingDescription))
                {
                    loading.Description = LoadingDescription;
                }
            }
            
        }

        internal static void HideLoading(Loading loading)
        {
            loading.Visibility = Visibility.Collapsed;
        }

        public static void HideLoading(this Dispatcher dp,Loading loading)
        {
            dp.BeginInvoke(DispatcherPriority.Normal, new Action(() => { HideLoading(loading); }));
        }

        internal static void HideLoading(this Window window, string LoadingDescription = null)
        {
            var ctrl = window.FindName(loadingName) as Loading;
            if (ctrl != null)
            {
                ctrl.Visibility = Visibility.Collapsed;
            }
        }
    }
}
