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
using MySql.Data.MySqlClient;
using System.Data;

namespace Festmenyek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string constr = "Server=localhost;Database=kkfestmenyek;Uid=root;Pwd=;";
        public MainWindow()
        {
            InitializeComponent();
            adatokbetoltese();
        }

        private void adatokbetoltese()
        {
            string parancs = "SELECT * FROM festmenyek";
            tablazat(parancs);
           

        }

        private void tablazat(string parancs)
        {
            List<adatsor> adatok = new List<adatsor>();
            MySqlConnection con = new MySqlConnection(constr);
            MySqlCommand cmd = con.CreateCommand();
            MySqlDataReader reader;
            cmd.CommandText = parancs;
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                adatsor festmenyek = new adatsor();
                festmenyek.id = reader.GetInt32(0);
                festmenyek.nev = reader.GetString(1);
                festmenyek.festo = reader.GetString(2);
                festmenyek.evszam = reader.GetInt32(3);
                festmenyek.szelesseg = reader.GetDouble(4);
                festmenyek.magassag = reader.GetDouble(5);
                festmenyek.kep = reader.GetString(6);
                adatok.Add(festmenyek);
            }
            dataGrid.ItemsSource = adatok;
            con.Close();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                adatsor kivlasztottSor = (adatsor)dataGrid.SelectedItem;
                nevTb.Text = kivlasztottSor.nev;
                festoTb.Text = kivlasztottSor.festo;
                evszamTb.Text = kivlasztottSor.evszam.ToString();
                szelessegTb.Text = kivlasztottSor.szelesseg.ToString();
                magassagTb.Text = kivlasztottSor.magassag.ToString();
                kepTb.Text = kivlasztottSor.kep;

            if (!string.IsNullOrEmpty(kivlasztottSor.kep))
            {
                string kepEleresiUt = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kepek", kivlasztottSor.kep);
                Uri uri = new Uri(kepEleresiUt, UriKind.Absolute);
                BitmapImage bitmapImage = new BitmapImage(uri);
                image.Source = bitmapImage;
            }
            else
            {
                image.Source = null;
            }
            }
        }

        private void Btmodosit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                adatsor kivalasztottSor = (adatsor)dataGrid.SelectedItem;

                kivalasztottSor.festo = festoTb.Text;
                kivalasztottSor.nev = nevTb.Text;


                bool ok = false;
                try
                {
                    double magassag = double.Parse(magassagTb.Text);
                    double szelesseg = double.Parse(szelessegTb.Text);
                    if (magassag <= 0 && szelesseg <= 0)
                    {
                        MessageBox.Show("Az érték amit megadtál nem valid!");
                        ok = false;
                    }
                    else
                    {
                        ok = true;

                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("A megadott érték nem szám!");
                    ok = false;
                }

                if (ok)
                {
                    kivalasztottSor.magassag = Convert.ToDouble(magassagTb.Text);
                    kivalasztottSor.szelesseg = Convert.ToDouble(szelessegTb.Text);
                    {
                        kivalasztottSor.kep = kepTb.Text;
                    }
                }


                MySqlConnection con = new MySqlConnection(constr);
                MySqlCommand cmd = con.CreateCommand();



                if (festoTb.Text != "" && nevTb.Text != "" && evszamTb.Text != "" && magassagTb.Text != "" && szelessegTb.Text != "" && kepTb.Text != "")
                {
                    if (ok)
                    {
                        try
                        {
                            con.Open();
                            cmd.CommandText = "UPDATE festmenyek SET kk_nev = @nev, kk_festo = @festo, kk_evszam = @evszam, kk_magassag = @magassag, kk_szelesseg = @szelesseg, kk_kep = @kep WHERE kk_id = @id";
                            cmd.Parameters.AddWithValue("@nev", kivalasztottSor.nev);
                            cmd.Parameters.AddWithValue("@festo", kivalasztottSor.festo);
                            cmd.Parameters.AddWithValue("@evszam", kivalasztottSor.evszam);
                            cmd.Parameters.AddWithValue("@magassag", kivalasztottSor.magassag);
                            cmd.Parameters.AddWithValue("@szelesseg", kivalasztottSor.szelesseg);
                            cmd.Parameters.AddWithValue("@kep", kivalasztottSor.kep);
                            cmd.Parameters.AddWithValue("@id", kivalasztottSor.id);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Sikeres módosítás az adatbázisban!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hiba történt a módosítás során: {ex.Message}");
                        }
                        finally
                        {
                            con.Close();
                            adatokbetoltese();
                        }
                    }

                }
                else MessageBox.Show("Nem lehetnek üres adatok");
            }
            else
            {
                MessageBox.Show("Nincs kiválasztva sor a módosításhoz!");
            }

        }

        private void Btujkepmodosit_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Képfájlok (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Mind Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedImagePath = openFileDialog.FileName;

                string fileName = System.IO.Path.GetFileName(selectedImagePath);

                string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kepek", fileName);

                try
                {
                    System.IO.File.Copy(selectedImagePath, destinationPath, true);

                    if (dataGrid.SelectedItem != null)
                    {
                        adatsor kivalasztottSor = (adatsor)dataGrid.SelectedItem;
                        kivalasztottSor.kep = fileName;
                        kepTb.Text = fileName;

                        string kepEleresiUt = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dolgozatkepek", fileName);
                        Uri uri = new Uri(kepEleresiUt, UriKind.Absolute);

                        BitmapImage bitmapImage = new BitmapImage(uri);

                        image.Source = bitmapImage;
                    }

                    MessageBox.Show("Kép sikeresen hozzáadva a mappába!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt a kép másolása során: {ex.Message}");
                }
            }
        }

        private void Bttorles_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                adatsor kivalasztottSor = (adatsor)dataGrid.SelectedItem;
                List<adatsor> adatok = (List<adatsor>)dataGrid.ItemsSource;

                MySqlConnection con = new MySqlConnection(constr);
                MySqlCommand cmd = con.CreateCommand();
                con.Open();

                cmd.CommandText = $"DELETE FROM festmenyek WHERE id = '{kivalasztottSor.id}'";
                cmd.ExecuteNonQuery();
                con.Close();

                adatok.Remove(kivalasztottSor); 
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = adatok;
                adatokbetoltese();
            }
            else
            {
                MessageBox.Show("Nincs kiválasztva sor a törléshez!");
            }
        }

        private void UjAdatRogzites_Click(object sender, RoutedEventArgs e)
        {
            if (ujFestmenyTb.Text != "" && ujFestoTb.Text != "" && ujEvszamTb.Text != "" && ujSzelessegTb.Text != "" && ujMagassagTb.Text != "")
            {
                bool ok = false;
                try
                {
                    int szelesseg = int.Parse(ujSzelessegTb.Text);
                    int magassag = int.Parse(ujMagassagTb.Text);

                    if (szelesseg < 0)
                    {
                        MessageBox.Show("A festmény szélessége nem lehet negatív!");
                        ok = false;
                    }
                    else if(magassag < 0)
                        {
                        MessageBox.Show("A festmény magassága nem lehet negatív!");
                        ok = false;
                    }
                    else ok = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("A festmény szélessége vagy magassága nem szám!");
                    ok = false;
                }
                if (ok)
                {
                    using (var con = new MySqlConnection(constr))
                    {
                        con.Open();
                        string sql = "INSERT INTO festmenyek(kk_nev ,kk_festo, kk_evszam , kk_szelesseg, kk_magassag, kk_kep) " +
                            "VALUES(@nev,@festo,@evszam,@szelesseg,@magassag,@kep)";
                        using (var cmd = new MySqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@nev", ujFestmenyTb.Text);
                            cmd.Parameters.AddWithValue("@festo", ujFestoTb.Text);
                            cmd.Parameters.AddWithValue("@evszam", ujEvszamTb.Text);
                            cmd.Parameters.AddWithValue("@szelesseg", ujSzelessegTb.Text);
                            cmd.Parameters.AddWithValue("@magassag", ujMagassagTb.Text);
                            cmd.Parameters.AddWithValue("@kep", ujKepTb.Text);
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                    MessageBox.Show("Adatrögzítés sikerült");
                    ujFestmenyTb.Text = "";
                    ujFestoTb.Text = "";
                    ujEvszamTb.Text = "";
                    ujSzelessegTb.Text = "";
                    ujMagassagTb.Text = "";
                    ujKepTb.Text = "";
                    adatokbetoltese();
                }
            }
            else MessageBox.Show("Minden adatot kötelező megadni!");
        }

        private void Tbkereses_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keresettadat = "";
            if (tbkereses.Text != "")
            {
                keresettadat = tbkereses.Text;
                kivalasztottadatokkeresese(keresettadat);
            }
            else
            {
                keresettadat = "";
                kivalasztottadatokkeresese(keresettadat);
            }
        }

        private void kivalasztottadatokkeresese(string keresettadat)
        {
            string parancs = $"SELECT * FROM festmenyek WHERE kk_nev LIKE '%{keresettadat}%' OR kk_festo LIKE '%{keresettadat}%'" +
                $" OR kk_evszam LIKE '%{keresettadat}%' OR kk_szelesseg LIKE '%{keresettadat}%' " +
                $"OR kk_magassag LIKE '%{keresettadat}%'";
            tablazat(parancs);
        }

        private void UjKepFeltoltesBt_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Képfájlok (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Mind Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedImagePath = openFileDialog.FileName;

                string fileName = System.IO.Path.GetFileName(selectedImagePath);

                string destinationPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kepek", fileName);

                try
                {
                    System.IO.File.Copy(selectedImagePath, destinationPath, true);

                    ujKepTb.Text = fileName;

                    string kepEleresiUt = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "kepek", fileName);
                    Uri uri = new Uri(kepEleresiUt, UriKind.Absolute);

                    BitmapImage bitmapImage = new BitmapImage(uri);

                    ujKep.Source = bitmapImage;

                    MessageBox.Show("Kép sikeresen hozzáadva a kepek mappába!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt a kép másolása során: {ex.Message}");
                }
            }
        }
    }
}
