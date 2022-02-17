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
    /// CODE IS STILL UNDERDEVELOPMENT, THE PROGRAM DOES NOT FUNCTION YET
    public partial class MainWindow : Window
    {
        Grid grid;
        readonly BackupScript backupScript;
        SaveSlot? tempSaveSlot;
        Button? tempBtn;
        public MainWindow()
        {
            backupScript = new BackupScript();
            grid = new Grid();
            InitializeComponent();
        }
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (sourceBox.Text == " " || destinationBox.Text == " ")
            {
                MessageBox.Show("Please enter both a source and a destination folder");
            }
            else
            {
                submitButton.Content = "Add";
                var saveSlot = new SaveSlot() { Source = sourceBox.Text, Destination = destinationBox.Text };
                backupScript.AddSlot(saveSlot);
                var button = new Button();
                var order = backupScript.slots[backupScript.slots.Count - 1].Order;
                button.Content = order.ToString();
                button.Click += ReplaceTextBoxContent;
                grid.Children.Add(button);
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                Grid.SetRow(button, grid.RowDefinitions.Count - 1);
                saveSlotViewer.Content = grid;
                var comboBox = new ComboBox();

            }

        }

        private void ReplaceTextBoxContent(object sender, RoutedEventArgs e)
        {
            tempBtn = (Button)sender;
            int order = Convert.ToInt32(tempBtn.Content) - 1;
            tempSaveSlot = backupScript.slots[order];
            sourceBox.Text = tempSaveSlot.Source;
            destinationBox.Text = tempSaveSlot.Destination;
            submitButton.Content = "save";
            submitButton.Click -= submitButton_Click;
            submitButton.Click += SaveChanges;
        }
        private void SaveChanges (object sender, RoutedEventArgs e)
        {
            if(sourceBox.Text == "" || destinationBox.Text == "")
            {
                if(sourceBox.Text == "" && destinationBox.Text == "" )
                {
                    MessageBox.Show("Slot was deleted");
                    backupScript.slots.RemoveAt(Convert.ToInt32(tempBtn.Content) - 1);
                    Refresh();
                    submitButton.Click += submitButton_Click;
                    submitButton.Click -= SaveChanges;
                    submitButton.Content = "Add";
                }
                else
                {
                    MessageBox.Show("Please enter both a source and a destination folder");
                }
            }
            else
            {
                submitButton.Click += submitButton_Click;
                submitButton.Click -= SaveChanges;
                submitButton.Content = "Add";
                var saveSlot = new SaveSlot() { Destination = destinationBox.Text.ToString(), Source = sourceBox.Text.ToString(), Order = tempBtn.Content.ToString() };
                backupScript.slots.RemoveAt(Convert.ToInt32(tempBtn.Content) - 1);
                backupScript.slots.Insert(Convert.ToInt32(tempBtn.Content) - 1, saveSlot);
                Refresh();
            }
            
        }
        // work in progress refresh method to refresh the buttons after one was deleted
        //private void Refresh()
        //{
        //    grid = new Grid();
        //    foreach(SaveSlot slot in backupScript.slots)
        //    {
        //        slot.Order = backupScript.slots.Find(<SaveSlot> SaveSlot)
        //        var rowDef = new RowDefinition() { Height = new GridLength(40) };
        //        grid.RowDefinitions.Add(rowDef);
        //        var btn = new Button();
        //        btn.Content = slot.Order;
        //        grid.Children.Add(btn);
        //        Grid.SetRow(btn, Convert.ToInt32(slot.Order));
        //        saveSlotViewer.Content = grid;
        //    }
        }
    }
}
