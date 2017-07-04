using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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

namespace biblioteka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ksiazka> ksiazkiKolekcja = new ObservableCollection<ksiazka>();
        private ObservableCollection<czytelnik> czytelnicyKolekcja = new ObservableCollection<czytelnik>();
        private ObservableCollection<zamowienie> zamowieniaKolekcja = new ObservableCollection<zamowienie>();


        public MainWindow()
        {
            InitializeComponent();
            //ListItem add = new ListItem();
            listBoxKsiazki.ItemsSource = ksiazkiKolekcja;
            listBoxCzytelnicy.ItemsSource = czytelnicyKolekcja;
            listBoxWypozyczenia.ItemsSource = zamowieniaKolekcja;
            comboBoxCzytelnik.ItemsSource = czytelnicyKolekcja;
            comboBoxKsiazka.ItemsSource = ksiazkiKolekcja;
            groupBoxNoweWypozyczenie.IsEnabled = false;
          
            //ksiazkiKolekcja.Add(new ksiazka { tytul = "as", autor = "aaga" });

            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBTEST"].ConnectionString))
                {
                    SqlCommand cmd1 =
                      new SqlCommand(
                        "select * from ksiazka");
                    cmd1.Connection = cn;
                    cmd1.CommandType = CommandType.Text;
                    SqlDataAdapter ksiazkiAdapter = new SqlDataAdapter(cmd1);
                    DataSet ksiazkiTable = new DataSet();
                    cn.Open();
                    ksiazkiAdapter.Fill(ksiazkiTable);

                    foreach (DataRow ksiazkaRow in ksiazkiTable.Tables[0].Rows)
                    {
                        ksiazkiKolekcja.Add(new ksiazka
                        {
                            id_ksiazka = int.Parse(ksiazkaRow["id_ksiazka"].ToString()),
                            isbn = ksiazkaRow["isbn"].ToString(),
                            tytul = ksiazkaRow["tytul"].ToString(),
                            autor = ksiazkaRow["autor"].ToString(),
                            wydawnictwo = ksiazkaRow["wydawnictwo"].ToString(),
                            opis = ksiazkaRow["opis"].ToString(),
                            liczba = int.Parse(ksiazkaRow["liczba"].ToString())

                        });

                    }

                    {
                        SqlCommand cmd2 =
                          new SqlCommand(
                            "select * from czytelnik");
                        cmd2.Connection = cn;
                        cmd2.CommandType = CommandType.Text;
                        SqlDataAdapter czytelnicyAdapter = new SqlDataAdapter(cmd2);
                        DataSet czytelnicyTable = new DataSet();
                        czytelnicyAdapter.Fill(czytelnicyTable);

                        foreach (DataRow czytelnikRow in czytelnicyTable.Tables[0].Rows)
                        {
                            czytelnicyKolekcja.Add(new czytelnik
                            {
                                id_czytelnik = int.Parse(czytelnikRow["id_czytelnik"].ToString()),
                                imie = czytelnikRow["imie"].ToString(),
                                nazwisko = czytelnikRow["nazwisko"].ToString(),
                                adres = czytelnikRow["adres"].ToString(),
                                telefon = czytelnikRow["telefon"].ToString(),
                                email = czytelnikRow["email"].ToString(),


                            });

                        }


                        {
                            SqlCommand cmd3 =
                              new SqlCommand(
                                "select z.*, c.imie, c.nazwisko, k.tytul, k.autor from zamowienie z join ksiazka k on k.id_ksiazka=z.id_ksiazka join czytelnik c on c.id_czytelnik = z.id_czytelnik");
                            cmd3.Connection = cn;
                            cmd3.CommandType = CommandType.Text;
                            SqlDataAdapter zamowieniaAdapter = new SqlDataAdapter(cmd3);
                            DataSet zamowieniaTable = new DataSet();
                            zamowieniaAdapter.Fill(zamowieniaTable);

                          //  listBoxWypozyczenia.Items.Add("f");


                            foreach (DataRow zamowienieRow in zamowieniaTable.Tables[0].Rows)
                            {
                                DateTime? data =null ;
                                try
                                {
                                    data = DateTime.Parse(zamowienieRow["data_zwrotu"].ToString());
                                }
                                catch (Exception)
                                {
                                    //throw;
                                }                            
                               // if (Nullable<System.DateTime>.TryParse(zamowienieRow["data_zwrotu"].ToString(), out data)) ;
                                zamowieniaKolekcja.Add(new zamowienie
                                {
                                    id_zamowienie = int.Parse(zamowienieRow["id_zamowienie"].ToString()),
                                    id_czytelnik = int.Parse(zamowienieRow["id_czytelnik"].ToString()),
                                    id_ksiazka = int.Parse(zamowienieRow["id_ksiazka"].ToString()),
                                    data_wypozyczenia = DateTime.Parse(zamowienieRow["data_wypozyczenia"].ToString()),
                                    data_oczekiwana_zwrotu = DateTime.Parse(zamowienieRow["data_oczekiwana_zwrotu"].ToString()),

                                    data_zwrotu = data,
                                    oddany = bool.Parse(zamowienieRow["oddany"].ToString()),
                                   
                                 

                                });

                            }

                        }
                        

                    }
                }
                }
            catch (Exception)
            {

                throw;
            }


        }

        //private void btDodaj_Click(object sender, RoutedEventArgs e)
        //{
        //    ksiazkiKolekcja.Add(new ksiazka { });
        //    listBoxKsiazki.SelectedIndex = ksiazkiKolekcja.Count - 1;
        //}

        //private void btZapiszDaneDoBazy_Click(object sender, RoutedEventArgs e)
        //{
        //    //procedura ??
        
        //}

    

     
        private void LadujWypozyczenie(object sender, SelectionChangedEventArgs e)
        {
            string czytelnikString = "";
            string ksiazkaString = "";
            zamowienie nowy = new zamowienie();
            nowy = listBoxWypozyczenia.SelectedItem as zamowienie;

            foreach (var item in czytelnicyKolekcja)
            {
                if (item.id_czytelnik == nowy.id_czytelnik)
                {
                    czytelnikString = item.imie + " " + item.nazwisko;
                }
            }
            textBoxCzytelnik.Text = czytelnikString;


            foreach (var item in ksiazkiKolekcja)
            {
                if (item.id_ksiazka == nowy.id_ksiazka)
                {
                    ksiazkaString = item.autor + "\n" + item.tytul;
                }
            }
            textBoxKsiazka.Text = ksiazkaString;
        }

    

        private void buttonWypozycz_Click(object sender, RoutedEventArgs e)
        {
            groupBoxNoweWypozyczenie.IsEnabled = true;
            int liczbaWypozyczen = zamowieniaKolekcja.Count + 1;
            textBoxIdNoweWyp.Text = liczbaWypozyczen.ToString();          
            textBoxDataWypozyczenia.Text = DateTime.Today.ToString();
            textBoxOczekDataZwrotu.Text = DateTime.Today.AddMonths(1).ToString();
        }

        private void buttonWypozyczKsiazke_Click(object sender, RoutedEventArgs e)
        {
            czytelnik nowy = new czytelnik();
            nowy = comboBoxCzytelnik.SelectedItem as czytelnik;

            ksiazka nowa = new ksiazka();
            nowa = comboBoxKsiazka.SelectedItem as ksiazka;

            zamowieniaKolekcja.Add(new zamowienie
            {
                id_zamowienie = int.Parse(textBoxIdNoweWyp.Text),
                id_czytelnik = nowy.id_czytelnik,
                id_ksiazka = nowa.id_ksiazka,
                data_wypozyczenia = DateTime.Today,
                data_oczekiwana_zwrotu = DateTime.Today.AddMonths(1),
                oddany = false
           });
            groupBoxNoweWypozyczenie.IsEnabled = false;
            
        }

        private void buttonZwroc_Click(object sender, RoutedEventArgs e)
        {
            dlgCzyZwrocic okno = new dlgCzyZwrocic();
            if (okno.ShowDialog() == false) return;           

            
            //zamowienie nowy = new zamowienie();
           // nowy = listBoxWypozyczenia.SelectedItem as zamowienie;
           // nowy.oddany =true;
           // nowy.data_zwrotu = DateTime.Today;

        }
    }
}
