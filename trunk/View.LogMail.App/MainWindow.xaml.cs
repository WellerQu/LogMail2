using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ViewModel.LogMail.Business;

namespace View.LogMail.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (this.DtxFormViewModel == null)
                this.DtxFormViewModel = new FormViewModel();

            DateTime today = DateTime.Now;
            int weekday = (int)today.DayOfWeek;

            this.DtxFormViewModel.DateKey = DateTime.Now;
            this.DtxFormViewModel.Load();

            var elements = this.RightSiderPanel.Children;
            foreach (UIElement item in elements)
            {
                Control ctrl = item as Control;
                if (ctrl != null && ctrl.Tag != null)
                {
                    int tagValue = Convert.ToInt32(ctrl.Tag);
                    if (weekday - tagValue > 0)
                        // 3E3E42
                        ctrl.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x3E, 0x3E, 0x42));
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // 保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.DtxFormViewModel.Save();
        }

        // 清空所有
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.DtxFormViewModel.Clear();
        }

        // 日期选择
        private void btnDateSelector_Click(object sender, RoutedEventArgs e)
        {
            DateTime today = DateTime.Now;
            int currentWeekday = (int)today.DayOfWeek;
            Button currentButton = sender as Button;
            int currentTag = Convert.ToInt32(currentButton.Tag);

            DateTime target = today.AddDays(0 - (currentWeekday - currentTag));

            if (this.DtxFormViewModel.DateKey != null)
                this.DtxFormViewModel.Save();

            this.DtxFormViewModel.DateKey = target;
            this.DtxFormViewModel.Load();
        }

        private void TextInput_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt != null)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.S)
                {
                    btnSave.Focus();
                    txt.Focus();

                    this.DtxFormViewModel.Save();
                }
            }
        }
    }
}
