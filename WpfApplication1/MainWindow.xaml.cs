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
using System.Runtime.InteropServices;
using System.Data;
using System.IO;
using Microsoft.Win32;
using Microsoft.Office.Interop.Excel;
using System.Windows.Controls.Primitives;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {



        //create Contact object
        private Contact co1 = new Contact();



        public MainWindow()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                textBox.Text = "";

                using (ContactContext db = new ContactContext())
                {
                    dataGrid2.ItemsSource = db.Contacts.ToList();

                    displayGrid();

                    //dataGrid2.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Close();
            }
        }



        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                textBox.Text = "";

                using (ContactContext db = new ContactContext())
                {
                    dataGrid2.ItemsSource = db.Contacts.ToList();

                    displayGrid();

                    //dataGrid2.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                Close();
            }
        }



        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchValue = textBox.Text;

                using (ContactContext db = new ContactContext())
                {

                    var matches = from m in db.Contacts
                                  where
            m.fname.Contains(searchValue) ||
            m.lname.Contains(searchValue) ||
            m.email.Contains(searchValue) ||
            m.mobilephone.Contains(searchValue) ||
            m.birthdate.Contains(searchValue) ||
            m.address.Contains(searchValue) ||
            m.description.Contains(searchValue)
                                  select m;

                    dataGrid2.ItemsSource = matches.ToList();

                    displayGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                Close();
            }
        }



        private void MenuItem_Click_0(object sender, RoutedEventArgs e)
        {
            Window1 f2 = new Window1();

            f2.ShowDialog();
        }



        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (dataGrid2.SelectedIndex != -1)
            {
                try
                {
                    int entryid = Convert.ToInt32((dataGrid2.SelectedItem as Contact).contactID.ToString());

                    using (ContactContext db = new ContactContext())
                    {
                        co1 = db.Contacts.Find(entryid);

                        db.Contacts.Remove(co1);

                        db.SaveChanges();

                        MessageBox.Show("Deleted Successfully", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());

                    Close();
                }
            }
            else
            {
                MessageBox.Show("No row selected", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            //expot2XLS();

            expot2XLS2();
        }



        private void expot2XLS()
        {

            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Workbook workbook = excelapp.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            try
            {

                for (int j = 0; j < dataGrid2.Columns.Count - 1; j++)
                {
                    Range myRange = (Range)sheet1.Cells[1, j + 1];
                    sheet1.Cells[1, j + 1].Font.Bold = true;
                    sheet1.Columns[j + 1].ColumnWidth = 20;
                    myRange.Value2 = dataGrid2.Columns[j + 1].Header;
                }

                //Loop through each column and read value from each row.

                for (int j = 0; j < dataGrid2.Columns.Count - 1; j++)
                {
                    for (int i = 0; i < dataGrid2.Items.Count; i++)
                    {
                        TextBlock b = dataGrid2.Columns[j + 1].GetCellContent(dataGrid2.Items[i]) as TextBlock;
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[i + 2, j + 1];
                        myRange.Value2 = b.Text;
                    }
                }

                //Getting the location and file name of the excel to save from user.

                SaveFileDialog saveDialog = new SaveFileDialog();

                saveDialog.Filter = "Excel 97-2003 (*.xls)|*.xls|Excel (*.xlsx)|*.xlsx";

                saveDialog.FilterIndex = 1;

                if (saveDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Exported Successfully", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }

            finally
            {

                workbook.Close(0);

                excelapp.Quit();

                //Marshal.FinalReleaseComObject(excelapp);

            }
        }



        private void displayGrid()
        {
            if (dataGrid2.Items.Count == 0)
            {
                menu1.IsEnabled = false;
                menu2.IsEnabled = false;
            }
            else
            {
                menu1.IsEnabled = true;
                menu2.IsEnabled = true;
            }
        }



        private void dataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Window1 f2 = new Window1();

                f2.textBox0.Text = (dataGrid2.SelectedItem as Contact).contactID.ToString();

                f2.textBox1.Text = (dataGrid2.SelectedItem as Contact).fname.ToString();

                f2.textBox2.Text = (dataGrid2.SelectedItem as Contact).lname.ToString();

                f2.textBox3.Text = (dataGrid2.SelectedItem as Contact).email.ToString();

                f2.textBox4.Text = (dataGrid2.SelectedItem as Contact).mobilephone.ToString();

                f2.datepick.Text = (dataGrid2.SelectedItem as Contact).birthdate.ToString();

                f2.textBox5.Text = (dataGrid2.SelectedItem as Contact).address.ToString();

                f2.textBox6.Text = (dataGrid2.SelectedItem as Contact).description.ToString();

                f2.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                Close();
            }
        }



        private void expot2XLS2()
        {

            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Workbook workbook = excelapp.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

            try
            {

                for (int j = 0; j < dataGrid2.Columns.Count; j++)
                {
                    Range myRange = (Range)sheet1.Cells[1, j + 1];
                    sheet1.Cells[1, j + 1].Font.Bold = true;
                    sheet1.Columns[j + 1].ColumnWidth = 20;
                    myRange.Value2 = dataGrid2.Columns[j].Header;
                }

                //Loop through each column and read value from each row.

                for (int j = 0; j < dataGrid2.Columns.Count; j++)
                {
                    for (int i = 0; i < dataGrid2.Items.Count; i++)
                    {
                        TextBlock b = dataGrid2.Columns[j].GetCellContent(dataGrid2.Items[i]) as TextBlock;
                        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[i + 2, j + 1];
                        myRange.Value2 = b.Text;
                    }
                }

                //Getting the location and file name of the excel to save from user.

                SaveFileDialog saveDialog = new SaveFileDialog();

                saveDialog.Filter = "Excel 97-2003 (*.xls)|*.xls|Excel (*.xlsx)|*.xlsx";

                saveDialog.FilterIndex = 1;

                if (saveDialog.ShowDialog() == true)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Exported Successfully", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }

            finally
            {

                workbook.Close(0);

                excelapp.Quit();

                //Marshal.FinalReleaseComObject(excelapp);

            }
        }
    }
}
