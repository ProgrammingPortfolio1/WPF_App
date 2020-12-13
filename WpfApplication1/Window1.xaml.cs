using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        //create Contact object
        private Contact co = new Contact();

        public Window1()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ID
                if (textBox0.Text != "")
                    co.contactID = Convert.ToInt32(textBox0.Text);

                //First Name
                if (textBox1.Text != "")
                    co.fname = textBox1.Text;
                else
                {
                    MessageBox.Show("First Name cannot be left empty", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //Last Name
                if (textBox2.Text != "")
                    co.lname = textBox2.Text;
                else
                {
                    MessageBox.Show("Last Name cannot be left empty", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Regex reg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match;

                //Email
                if (textBox3.Text != "")
                {
                    match = reg.Match(textBox3.Text);

                    if (match.Success)
                        co.email = textBox3.Text;
                    else
                    {
                        MessageBox.Show("Email not valid", "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                    co.email = textBox3.Text;

                //Phone Number
                co.mobilephone = textBox4.Text;

                //Birth Date
                co.birthdate = datepick.Text;

                //Address
                co.address = textBox5.Text;

                //Description
                co.description = textBox6.Text;

                //create ContactContext object
                using (ContactContext db = new ContactContext())
                {

                    if (textBox0.Text == "") //Create a new database entry
                        db.Contacts.Add(co);
                    else //Update an existing database entry
                        db.Entry(co).State = EntityState.Modified;

                    db.SaveChanges();

                    MessageBox.Show("Database Updated", "INFORMATION", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString());

                MessageBox.Show("Error with the Database", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);

                Close();
            }

            Close();
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");

            MatchCollection matches = regex.Matches(textBox4.Text);
            if (matches.Count > 0)
            {
                int index = textBox4.SelectionStart;
                textBox4.Text = textBox4.Text.Remove(textBox4.SelectionStart - 1, 1);
                textBox4.Select(index - 1, 0);
            }
        }
    }
}
