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
using System.Runtime.Serialization.Formatters.Binary;

namespace Cooking
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dish currentDish = new Dish();
        bool formChanged = false;
        Random random = new Random();
        string selectedPathFolder = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        //If the form is changed, the parameter formChanged is set to true
        //if the form has not changed, the form fields take appropriate class value fields
        private void UpdateForm(bool changed)
        {
            if (!changed)
            {
                cookedDish.Text = currentDish.Description;
                familyReaction.Text = currentDish.Results;
                cookingDate.SelectedDate = currentDish.LastUsed;
                resipe.Text = currentDish.Resipe;

                if (!String.IsNullOrEmpty(currentDish.DishPath))
                    lastCookingDate.Text = currentDish.LastUsed.ToString();

                Title = "Cooking";
            }
            else
                Title = "Cooking*";
            formChanged = changed;
        }

        private void cookedDish_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentDish.Description = cookedDish.Text;
            UpdateForm(true);
        }

        private void familyReaction_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentDish.Results = familyReaction.Text;
            UpdateForm(true);
        }

        private void cookingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            currentDish.LastUsed = (DateTime)cookingDate.SelectedDate;
            UpdateForm(true);
        }

        private void resipe_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentDish.Resipe = resipe.Text;
            UpdateForm(true);
        }
        
        private void folder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = selectedPathFolder;
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                selectedPathFolder = folderBrowserDialog.SelectedPath;
                openDish.IsEnabled = true;
                saveDish.IsEnabled = true;
                randomeDish.IsEnabled = true;
            }
            showAllDish();
        }

        private void openDish_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (CheckChanged())
            {
                openFileDialog.InitialDirectory = selectedPathFolder;
                openFileDialog.Filter = "Dish files (*.dish)|*.dish|All files (*.*)|*.*";
                openFileDialog.Title = "Open";
                openFileDialog.FileName = cookedDish.Text + ".dish";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    currentDish = new Dish(openFileDialog.FileName);
                    UpdateForm(false);
                }
            }
        }

        private void saveDish_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = selectedPathFolder;
            saveFileDialog.Filter = "Dish files (*.dish)|*.dish|All files (*.*)|*.*";
            saveFileDialog.Title = "Save as...";

            if (!String.IsNullOrEmpty(cookedDish.Text) && 
                !String.IsNullOrEmpty(familyReaction.Text) &&
                !String.IsNullOrEmpty(resipe.Text))
            {
                saveFileDialog.FileName = cookedDish.Text + ".dish";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    currentDish.Save(saveFileDialog.FileName);
                    UpdateForm(false);
                    System.Windows.Forms.MessageBox.Show("Dish written");
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Please specify dish, result and resipe", 
                    "Unable to save", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            showAllDish();
        }

        private void randomeDish_Click(object sender, RoutedEventArgs e)
        {
            if (CheckChanged())
            {
                currentDish = new Dish(random, selectedPathFolder);
                UpdateForm(false);
            }
        }

        //If the fields are changed and not saved - return MessageBox "Warning"
        private bool CheckChanged()
        {
            if (formChanged)
            {
                DialogResult result = 
                    System.Windows.Forms.MessageBox.Show("The current dish has not been saved. Continue?", 
                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == System.Windows.Forms.DialogResult.No)
                    return false;
            }
            return true;
        }

        //Shows all files with the extension ".dish" in the open folder
        private void showAllDish()
        {
            string[] fileNames = Directory.GetFiles(selectedPathFolder, "*.dish");
            whatCooked.Text = "";
            int number = 1;

            foreach (string name in fileNames)
            {
                currentDish = new Dish(name);
                whatCooked.Text += number + ". " + currentDish.Description + Environment.NewLine;
                number++;
            }
        }
    }
}
