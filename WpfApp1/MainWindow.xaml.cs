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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid grid;
        readonly BackupScript backupScript;
        public MainWindow()
        {
            backupScript = new BackupScript();
            grid = new Grid();
            InitializeComponent();
        }
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (sourceBox.Text == "" || destinationBox.Text == "")
            {
                MessageBox.Show("Please enter both a source and a destination folder");
            }
            else
            {
                var saveSlot = new SaveSlot() { Source = sourceBox.Text, Destination = destinationBox.Text };
                backupScript.AddSlot(saveSlot);
                var button = new Button();
                button.Content = backupScript.slots[backupScript.slots.Count - 1].Order;
                grid.Children.Add(button);
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                Grid.SetRow(button, grid.RowDefinitions.Count - 1);
                saveSlotViewer.Content = grid;
            }

        }
        private void ReplaceTextBoxContent()
        {
            sourceBox.Text = backupScript.slots[backupScript.slots.Count - 1].Source;
            destinationBox.Text = backupScript.slots[backupScript.slots.Count - 1].Destination;
        }
    }
}
