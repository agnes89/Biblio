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
                                   
                                    //czytelnik.imie = zamowienieRow["imie"].ToString(),
                                    //nazwisko = zamowienieRow["nazwisko"].ToString(),
                                    // tytul = zamowienieRow["tytul"].ToString(),
                                    //autor = zamowienieRow["autor"].ToString(),

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

        private void btDodaj_Click(object sender, RoutedEventArgs e)
        {
            ksiazkiKolekcja.Add(new ksiazka { });
            listBoxKsiazki.SelectedIndex = ksiazkiKolekcja.Count - 1;
        }

        private void btZapiszDaneDoBazy_Click(object sender, RoutedEventArgs e)
        {
            //procedura ??
        
        }

        private void btOddaj_Click(object sender, RoutedEventArgs e)
        {
            //kwestia wypozyczen historycznych


          
            //dodac daty
            //nowy zamowienie sptro
        }

     
        private void LadujCzytelnika(object sender, SelectionChangedEventArgs e)
        {
            string czytelnikString = "";
            czytelnik nowy = new czytelnik();
            nowy = listBoxWypozyczenia.SelectedItem as czytelnik;
            foreach (var item in czytelnicyKolekcja)
            {
                //if (item.id_czytelnik == tex)t
                {
                    czytelnikString = nowy.imie + "" + nowy.nazwisko;
                }

            }

            textBoxCzytelnik.Text = czytelnikString;
        }

        private void Wypozycz(object sender, RoutedEventArgs e)
        {
            wypozyczenieNowe nowe = new wypozyczenieNowe();
            if (nowe.ShowDialog()==true)
            {
                //textBoxCzytelnik.Text = "d";
            }
            
        }
    }
}
