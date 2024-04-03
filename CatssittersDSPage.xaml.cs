using DataBaseLab2.CatCafeDataSetTableAdapters;
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
    /// Логика взаимодействия для CatssittersDSPage.xaml
    /// </summary>
    public partial class CatssittersDSPage : Page
    {
        private catsTableAdapter cats = new catsTableAdapter();
        private catsitterTableAdapter catSitters = new catsitterTableAdapter();
        public CatssittersDSPage()
        {
            InitializeComponent();
            catsSittersDataGrid.ItemsSource = catSitters.GetData();
        }

        private void createItemButton_Click(object sender, RoutedEventArgs e)
        {
            var firstname = firstNameTextBox.Text;
            var surname = surnameTextBox.Text;
            var midlename = midleNameTextBox.Text;
            var workexperience = workExpirienceTextBox.Text;

            catSitters.InsertQuery(firstname, surname, midlename, Convert.ToInt32(workexperience));

            catsSittersDataGrid.ItemsSource = catSitters.GetData();
        }

        private void editItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (catsSittersDataGrid.SelectedItem == null)
            {
                return;
            }

            var original_ID = Convert.ToInt32((catsSittersDataGrid.SelectedItem as DataRowView).Row[0]);
            var firstname = firstNameTextBox.Text;
            var surname = surnameTextBox.Text;
            var midlename = midleNameTextBox.Text;
            var workexperience = workExpirienceTextBox.Text;
            catSitters.UpdateQuery(firstname, surname, midlename, Convert.ToInt32(workexperience), original_ID);
            catsSittersDataGrid.ItemsSource = catSitters.GetData();
        }

        private void deleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (catsSittersDataGrid.SelectedItem == null)
            {
                return;
            }

            var catSitter_ID = Convert.ToInt32((catsSittersDataGrid.SelectedItem as DataRowView).Row[0]);

            var catsTable = cats.GetData().AsEnumerable()
                .Where(x => x.Field<int>("ID_catsitter") == catSitter_ID);

            var IDs = catSitters.GetData().AsEnumerable()
                .Select(x => x.Field<int>("ID_catsitter"))
                .Where(x => x != catSitter_ID)
                .ToList();

            foreach (var cat in catsTable)
            {
                var original_ID = cat.Field<int>("ID_cats");
                var nickname = cat.Field<string>("nickname");
                var age = cat.Field<int>("age");
                var gender = cat.Field<string>("gender");
                var takehomestatus = cat.Field<string>("takehomestatus");
                var ID_sitter = IDs[new Random().Next(0, IDs.Count)];
                cats.UpdateQuery(nickname, age, gender, takehomestatus, ID_sitter, original_ID);
            }

            catSitters.DeleteQuery(catSitter_ID);
            catsSittersDataGrid.ItemsSource = catSitters.GetData();
            CatsDSPage.OnCatsDataGridChanged?.Invoke(cats.GetData());
        }

        private void catsSittersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (catsSittersDataGrid.SelectedItem == null)
            {
                return;
            }
            firstNameTextBox.Text = (catsSittersDataGrid.SelectedItem as DataRowView).Row[1].ToString();
            surnameTextBox.Text = (catsSittersDataGrid.SelectedItem as DataRowView).Row[2].ToString();
            midleNameTextBox.Text = (catsSittersDataGrid.SelectedItem as DataRowView).Row[3].ToString();
            workExpirienceTextBox.Text = (catsSittersDataGrid.SelectedItem as DataRowView).Row[4].ToString();
        }
    }
}
