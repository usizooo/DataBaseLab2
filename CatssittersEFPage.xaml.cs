using System;
using System.Collections.Generic;
using System.Data;
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

namespace DataBaseLab2
{
    /// <summary>
    /// Логика взаимодействия для CatssittersEFPage.xaml
    /// </summary>
    public partial class CatssittersEFPage : Page
    {
        private CatCafeEntities context = new CatCafeEntities();
        public CatssittersEFPage()
        {
            InitializeComponent();
            catsSittersDataGrid.ItemsSource = context.catsitter.ToList();
        }

        private void createItemButton_Click(object sender, RoutedEventArgs e)
        {
            catsitter sitter = new catsitter();
            sitter.firstname = firstNameTextBox.Text;
            sitter.surname = surnameTextBox.Text;
            sitter.midlename = midleNameTextBox.Text;
            sitter.workexperience = Convert.ToInt32(workExpirienceTextBox.Text);

            context.catsitter.Add(sitter);

            context.SaveChanges();
            catsSittersDataGrid.ItemsSource = context.catsitter.ToList();

        }

        private void catsSittersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (catsSittersDataGrid.SelectedItem != null)
            {
                var selected = catsSittersDataGrid.SelectedItem as catsitter;

                firstNameTextBox.Text = selected.firstname;
                surnameTextBox.Text = selected.surname;
                midleNameTextBox.Text = selected.midlename;
                workExpirienceTextBox.Text = selected.workexperience.ToString();
            }
            
        }

        private void editItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (catsSittersDataGrid.SelectedItem != null)
            {
                var selected = catsSittersDataGrid.SelectedItem as catsitter;

                selected.firstname = firstNameTextBox.Text;
                selected.surname = surnameTextBox.Text;
                selected.midlename = midleNameTextBox.Text;
                selected.workexperience = Convert.ToInt32(workExpirienceTextBox.Text);

                context.SaveChanges();
                catsSittersDataGrid.ItemsSource = context.catsitter.ToList();
            }

        }

        private void deleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (catsSittersDataGrid.SelectedItem != null)
            {
                var selectedCatSitter = catsSittersDataGrid.SelectedItem as catsitter; 

                context.catsitter.Remove(selectedCatSitter);

                var catsLinkedToDeletedSitter = context.cats.Where(cat => cat.ID_catsitter == selectedCatSitter.ID_catsitter).ToList();
                foreach (var cat in catsLinkedToDeletedSitter)
                {
                    var remainingSitters = context.catsitter.Where(sitter => sitter.ID_catsitter != selectedCatSitter.ID_catsitter).ToList();
                    var randomSitter = remainingSitters[new Random().Next(0, remainingSitters.Count)];
                    cat.ID_catsitter = randomSitter.ID_catsitter;
                }
                context.SaveChanges();
                catsSittersDataGrid.ItemsSource = context.catsitter.ToList();
            } 
        }
    }
}
