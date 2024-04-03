using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для CatsEFPage.xaml
    /// </summary>
    public partial class CatsEFPage : Page
    {
        CatCafeEntities context = new CatCafeEntities();
        private Dictionary<string, int> sittersSurnames = new Dictionary<string, int>();
        public CatsEFPage()
        {
            InitializeComponent();
            catsDataGrid.ItemsSource = context.cats.ToList();
            catGenderComboBox.ItemsSource = new List<string> { "мальчик", "девочка" };
            var pairs = context.catsitter.ToList().Select(row
                => new KeyValuePair<string, int>(
                    row.surname,
                    row.ID_catsitter));
            foreach (var pair in pairs)
                sittersSurnames.Add(pair.Key, pair.Value);

            catCatsitterComboBox.ItemsSource = sittersSurnames.Keys;
        }

        private void createItemButton_Click(object sender, RoutedEventArgs e)
        {
            cats cat = new cats();
            cat.nickname = catNameTextBox.Text;
            cat.age = Convert.ToInt32(catAgeTextBox.Text);
            cat.gender = catGenderComboBox.Text;
            cat.takehomestatus = (bool)catStatusCheckBox.IsChecked ? "да" : "нет";
            cat.ID_catsitter = sittersSurnames[catCatsitterComboBox.SelectedItem.ToString()];

            context.cats.Add(cat);

            context.SaveChanges();
            catsDataGrid.ItemsSource = context.cats.ToList();
        }

        private void editItemButton_Click(object sender, RoutedEventArgs e)
        {

            if (catsDataGrid.SelectedItem != null)
            {
                var selected = catsDataGrid.SelectedItem as cats;

                selected.nickname = catNameTextBox.Text;
                selected.age = Convert.ToInt32(catAgeTextBox.Text);
                selected.gender = catGenderComboBox.Text;
                selected.takehomestatus = (bool)catStatusCheckBox.IsChecked ? "да" : "нет";
                selected.ID_catsitter = sittersSurnames[catCatsitterComboBox.SelectedItem.ToString()];

                context.SaveChanges();
                catsDataGrid.ItemsSource = context.cats.ToList();
            }
            
        }

        private void deleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (catsDataGrid.SelectedItem != null)
            {
                var selectedCat = catsDataGrid.SelectedItem as cats; 
                context.cats.Remove(selectedCat); 
                context.SaveChanges();
                catsDataGrid.ItemsSource = context.cats.ToList();
            }
        }
        private void catsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (catsDataGrid.SelectedItem != null)
            {
                var selected = catsDataGrid.SelectedItem as cats;

                catNameTextBox.Text = selected.nickname;
                catAgeTextBox.Text = selected.age.ToString();
                catGenderComboBox.SelectedIndex = selected.gender == "мальчик"
                    ? 0 : 1;
                catStatusCheckBox.IsChecked = selected.takehomestatus == "да"
                    ? true : false;
                catsDataGrid.SelectedItem = selected.ID_catsitter;
                var catSitterSurname = sittersSurnames.FirstOrDefault(x => x.Value == selected.ID_catsitter).Key;
                for (int i = 0; catCatsitterComboBox.Items.Count > 0; i++)
                {
                    if (catCatsitterComboBox.Items[i].ToString() == catSitterSurname)
                    {
                        catCatsitterComboBox.SelectedIndex = i;
                        break;
                    }
                }

            }
        }
    }
}
