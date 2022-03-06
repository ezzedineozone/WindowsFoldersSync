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
using System.Windows.Forms;
using System.IO;

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
        SaveSlot tempSaveSlot;
        System.Windows.Controls.Button tempBtn;
        FolderBrowserDialog folderBrowserDialog;
        public MainWindow()
        {
            InitializeComponent();
            folderBrowserDialog = new FolderBrowserDialog();
            tempBtn = new System.Windows.Controls.Button();
            tempSaveSlot = new SaveSlot();
            backupScript = new BackupScript();
            grid = new Grid();
            backupScript.slots = SqlLiteDataAccess.GetSaveSlots();
            RefreshDB();
            if(backupScript.slots.Count != 0)
            {
                Refresh();
            }
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (sourceBox.Text == "" || destinationBox.Text == "")
            {
                System.Windows.MessageBox.Show("Please enter both a source and a destination folder");
            }
            else
            {
                if(CheckIfEligible(destinationBox.Text))
                {
                    submitButton.Content = "Add";
                    var saveSlot = new SaveSlot() { Source = sourceBox.Text, Destination = destinationBox.Text };
                    backupScript.AddSlot(saveSlot);
                    var button = new System.Windows.Controls.Button();
                    var order = backupScript.slots[backupScript.slots.Count - 1].Order;
                    button.Content = order.ToString();
                    button.Click += ReplaceTextBoxContent;
                    grid.Children.Add(button);
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                    Grid.SetRow(button, grid.RowDefinitions.Count - 1);
                    saveSlotViewer.Content = grid;
                    RefreshDB();
                    Refresh();
                }
                else
                {
                    System.Windows.MessageBox.Show("destination already exists in one of the save slots");
                }

            }

        }

        private void ReplaceTextBoxContent(object sender, RoutedEventArgs e)
        {
            tempBtn = (System.Windows.Controls.Button)sender;
            int order = Convert.ToInt32(tempBtn.Content) - 1;
            tempSaveSlot = backupScript.slots[order];
            sourceBox.Text = tempSaveSlot.Source;
            destinationBox.Text = tempSaveSlot.Destination;
            submitButton.Content = "Save";
            submitButton.Click -= SaveChanges;
            submitButton.Click -= submitButton_Click;
            submitButton.Click += SaveChanges;
        }
        private void SaveChanges (object sender, RoutedEventArgs e)
        {
            if(sourceBox.Text.ToString() == "" || destinationBox.Text.ToString() == "")
            {
                if(sourceBox.Text.ToString() == "" && destinationBox.Text.ToString() == "" )
                {
                    System.Windows.MessageBox.Show("Slot was deleted");
                    backupScript.slots.RemoveAt(Convert.ToInt32(tempBtn.Content) - 1);
                    sourceBox.Text = tempSaveSlot.Source;
                    destinationBox.Text = tempSaveSlot.Destination;
                    submitButton.Click += submitButton_Click;
                    submitButton.Click -= SaveChanges;
                    submitButton.Content = "Add";
                    RefreshDB();
                    Refresh();
                }
                else
                {
                    sourceBox.Text = tempSaveSlot.Source;
                    destinationBox.Text = tempSaveSlot.Destination;
                    System.Windows.MessageBox.Show("Please enter both a source and a destination folder");
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
                    sourceBox.Text = "";
                    destinationBox.Text = "";
                    submitButton.Click += submitButton_Click;
                    submitButton.Click -= SaveChanges;
                    Refresh();
                    RefreshDB();
                }
                else
                {
                    System.Windows.MessageBox.Show("Destination already exists in one of the save slots");
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
                var btn = new System.Windows.Controls.Button() { Content = slot.Order};
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
        private void ShowBrowser(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            var selectedPath = folderBrowserDialog.SelectedPath;
            var btn = (System.Windows.Controls.Button)sender;
            if(btn.Name == "sourceBrowser")
            {
                sourceBox.Text = selectedPath ;
            }
            else
            {
                if (Directory.GetDirectories(selectedPath).Count() == 0 && Directory.GetFiles(selectedPath).Count() == 0 )
                {
                    destinationBox.Text = folderBrowserDialog.SelectedPath;
                }
                else
                {
                    System.Windows.MessageBox.Show("Your destination folder is not empty, please select an empty folder");
                }
            }
        }
        private void RefreshDB()
        {
            ///code makes sure that The sqlite database is uptodate with the programs backupscript.slots
            SqlLiteDataAccess.DeleteTable();
            foreach (var slot in backupScript.slots)
            {
                SqlLiteDataAccess.AddSaveSlot(slot);
            }
        }

        private void beginBackup_Click(object sender, RoutedEventArgs e)
        {
            backupScript.BeginBackup();
        }

        private void clearDestBox_Click_1(object sender, RoutedEventArgs e)
        {
            destinationBox.Text = "";
        }

        private void clearSourceBox_Click(object sender, RoutedEventArgs e)
        {
            sourceBox.Text = "";
        }
    }
}
