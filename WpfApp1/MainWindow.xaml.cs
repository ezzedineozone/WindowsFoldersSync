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
            beginBackup.Click += backupScript.BeginBackup;
        }
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (sourceBox.Text == "" || destinationBox.Text == "")
            {
                MessageBox.Show("Please enter both a source and a destination folder");
            }
            else
            {
                if(CheckIfEligible(destinationBox.Text))
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
                }
                else
                {
                    MessageBox.Show("destination already exists in one of the save slots");
                }

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
                    sourceBox.Text = tempSaveSlot.Source;
                    destinationBox.Text = tempSaveSlot.Destination;
                    submitButton.Click += submitButton_Click;
                    submitButton.Click -= SaveChanges;
                    submitButton.Content = "Add";
                }
                else
                {
                    sourceBox.Text = tempSaveSlot.Source;
                    destinationBox.Text = tempSaveSlot.Destination;
                    MessageBox.Show("Please enter both a source and a destination folder");
                }
            }
            else
            {
                if(CheckIfEligible(destinationBox.Text))
                {
                    submitButton.Content = "Add";
                    var saveSlot = new SaveSlot() { Destination = destinationBox.Text.ToString(), Source = sourceBox.Text.ToString(), Order = tempBtn.Content.ToString() };
                    backupScript.slots.RemoveAt(Convert.ToInt32(tempBtn.Content) - 1);
                    backupScript.slots.Insert(Convert.ToInt32(tempBtn.Content) - 1, saveSlot);
                    Refresh();
                    sourceBox.Text = "";
                    destinationBox.Text = "";
                    submitButton.Click += submitButton_Click;
                    submitButton.Click -= SaveChanges;
                    submitButton.Content = "Add";
                }
                else
                {
                    MessageBox.Show("Destination already exists in one of the save slots");
                    submitButton.Content = "Add";

                }

            }
            
        }
        private void Refresh()
        {
            var updatedList = new List<SaveSlot>();
            foreach(var slot in backupScript.slots)
            {
                slot.Order = (updatedList.Count + 1).ToString();
                updatedList.Add(slot);
            }
            backupScript.slots = updatedList;
            grid = new Grid();
            foreach(var slot in backupScript.slots)
            {
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                var btn = new Button() { Content = slot.Order};
                btn.Click += ReplaceTextBoxContent;
                grid.Children.Add(btn);
                Grid.SetRow(btn, grid.RowDefinitions.Count - 1);
            }
            saveSlotViewer.Content = grid;
        }
        private bool CheckIfEligible(string destination)
        {
            bool eligible = true;
            foreach (var slot in backupScript.slots)
            {
                if(slot != tempSaveSlot)
                {
                    if (slot.Destination == destination)
                    {
                        eligible = false;
                    }
                }
            }
            return eligible;
        }
    }
}
