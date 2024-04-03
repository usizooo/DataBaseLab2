using DataBaseLab2.CatCafeDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для CatsDSPage.xaml
    /// </summary>
    public partial class CatsDSPage : Page
    {
        catsTableAdapter cats = new catsTableAdapter();
        catsitterTableAdapter catsSitter = new catsitterTableAdapter();
        private Dictionary<string, int> sittersSurnames = new Dictionary<string, int>();

        public static Action<CatCafeDataSet.catsDataTable> OnCatsDataGridChanged;

        public CatsDSPage()
        {
            InitializeComponent();
            catsDataGrid.ItemsSource = cats.GetData();
            catGenderComboBox.ItemsSource = new List<string> { "мальчик", "девочка" };
            var pairs = catsSitter.GetData().AsEnumerable().Select(row
                    => new KeyValuePair<string, int>(
                        row.Field<string>("surname"),
                        row.Field<int>("ID_catsitter")));

            foreach (var pair in pairs)
                sittersSurnames.Add(pair.Key, pair.Value);

            catCatsitterComboBox.ItemsSource = sittersSurnames.Keys;

            OnCatsDataGridChanged += UpdateCatsDataGrid;
        }

        private void createItemButton_Click(object sender, RoutedEventArgs e)
        {
            var nickname = catNameTextBox.Text;
            var age = Convert.ToInt32(catAgeTextBox.Text);
            var gender = catGenderComboBox.Text;
            var takehomestatus = (bool)catStatusCheckBox.IsChecked ? "да" : "нет";
            var ID_catsitter = sittersSurnames[catCatsitterComboBox.SelectedItem.ToString()];
            cats.InsertQuery(nickname, age, gender, takehomestatus, ID_catsitter);
            catsDataGrid.ItemsSource = cats.GetData();
        }

        private void editItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (catsDataGrid.SelectedItem == null)
            {
                return;
            }

            var original_ID = Convert.ToInt32((catsDataGrid.SelectedItem as DataRowView).Row[0]);
            var nickname = catNameTextBox.Text;
            var age = Convert.ToInt32(catAgeTextBox.Text);
            var gender = catGenderComboBox.Text;
            var takehomestatus = (bool)catStatusCheckBox.IsChecked ? "да" : "нет";
            var ID_sitter = sittersSurnames[catCatsitterComboBox.SelectedItem.ToString()];
            cats.UpdateQuery(nickname, age, gender, takehomestatus, ID_sitter, original_ID);
            catsDataGrid.ItemsSource = cats.GetData();
        }

        private void deleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (catsDataGrid.SelectedItem == null)
            {
                return;
            }

            var ID_cat = Convert.ToInt32((catsDataGrid.SelectedItem as DataRowView).Row[0]);
            cats.DeleteQuery(ID_cat);
            catsDataGrid.ItemsSource = cats.GetData();
        }

        private void catsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (catsDataGrid.SelectedItem == null)
            {
                return;
            }
            catNameTextBox.Text = (catsDataGrid.SelectedItem as DataRowView).Row[1].ToString();
            catAgeTextBox.Text = (catsDataGrid.SelectedItem as DataRowView).Row[2].ToString();
            catGenderComboBox.SelectedIndex
                = (catsDataGrid.SelectedItem as DataRowView).Row[3].ToString() == "мальчик"
                ? 0 : 1;
            catStatusCheckBox.IsChecked
                = (catsDataGrid.SelectedItem as DataRowView).Row[4].ToString() == "да"
                ? true : false;
            var ID_catsitter = Convert.ToInt32((catsDataGrid.SelectedItem as DataRowView).Row[5]);
            var catSitterSurname = sittersSurnames.Where(x => x.Value == ID_catsitter).FirstOrDefault().Key;
            for (int i = 0; catCatsitterComboBox.Items.Count > 0; i++)
            {
                if (catCatsitterComboBox.Items[i].ToString() == catSitterSurname)
                {
                    catCatsitterComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        private void UpdateCatsDataGrid(CatCafeDataSet.catsDataTable values)
        {
            catsDataGrid.ItemsSource = values;
        }
    }
}
