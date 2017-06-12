using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;

namespace TestingSqlite
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SQLiteConnection con = new SQLiteConnection(@"Data Source=testingDB.db3");
        int mod = 0;
        public MainWindow()
        {
            InitializeComponent();
            remplir(listEmploye);
            SQLiteCommand cm = new SQLiteCommand("SELECT * FROM Ministere", con);
            open();
            cm.ExecuteNonQuery();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
            DataTable dt = new DataTable(); da.Fill(dt);
            foreach(DataRow dr in dt.Rows){
                cmbministere.Items.Add(dr["nom"].ToString());
            }
            
            close();
        }
        private void open() { if (con.State == ConnectionState.Closed) { con.Open(); } }
        private void close() { if (con.State == ConnectionState.Open) { con.Close(); }}
        private void remplir(StackPanel sp) {
            sp.Children.Clear();
            SQLiteCommand cm = new SQLiteCommand("SELECT * FROM Agent ORDER BY ID DESC", con);
            open();
            cm.ExecuteNonQuery();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
            DataTable dt = new DataTable(); da.Fill(dt);
            foreach (DataRow dr in dt.Rows) {
                Grid grid = new Grid { Height=45};
                ColumnDefinition[] col = { new ColumnDefinition { Width = new GridLength(50) }, new ColumnDefinition { Width = new GridLength(100) }, new ColumnDefinition { Width = new GridLength(120) }, new ColumnDefinition { Width = new GridLength(50) }, new ColumnDefinition { Width = new GridLength(50) }, new ColumnDefinition { Width = new GridLength(90) }, new ColumnDefinition { Width = new GridLength(110) }, new ColumnDefinition { Width = new GridLength(150) }, new ColumnDefinition { Width = new GridLength(90) }, new ColumnDefinition { Width = new GridLength(75) }, new ColumnDefinition { Width = new GridLength(75) } };
                for (int i = 0; i < col.Length; i++) { grid.ColumnDefinitions.Add(col[i]); }
                Label l0 = new Label { Content = dr["ID"], VerticalContentAlignment = VerticalAlignment.Center },
                    l1 = new Label { Content = dr["nom"], VerticalContentAlignment = VerticalAlignment.Center },
                    l2 = new Label { Content = dr["prenom"], VerticalContentAlignment = VerticalAlignment.Center },
                    l3 = new Label { Content = dr["categorie"], VerticalContentAlignment = VerticalAlignment.Center },
                    l4 = new Label { Content = dr["classe"], VerticalContentAlignment = VerticalAlignment.Center },
                    l5 = new Label { Content = dr["echelon"], VerticalContentAlignment = VerticalAlignment.Center },
                    l6 = new Label { Content = dr["ministere"], VerticalContentAlignment = VerticalAlignment.Center },
                    l7 = new Label { Content = dr["libelle"], VerticalContentAlignment = VerticalAlignment.Center },
                    l8 = new Label { Content = dr["datenaiss"], VerticalContentAlignment = VerticalAlignment.Center };
                Button bt0 = new Button { Content = "Afficher" }, bt1 = new Button { Content = "Supprimer" };
                bt0.Click += (o, s) => { afficher(int.Parse(dr["ID"].ToString())); mod = int.Parse(dr["ID"].ToString()); btnUpdate.IsEnabled = true; };
                bt1.Click += (o, s) => { supprimer(int.Parse(dr["ID"].ToString())); btnUpdate.IsEnabled = false; };

                Grid.SetColumn(l0, 0); Grid.SetColumn(l1, 1); Grid.SetColumn(l2, 2); Grid.SetColumn(l3, 3); Grid.SetColumn(l4, 4); Grid.SetColumn(l5, 5); Grid.SetColumn(l6, 6); Grid.SetColumn(l7, 7); Grid.SetColumn(l8, 8); Grid.SetColumn(bt0, 9); Grid.SetColumn(bt1, 10);
                grid.Children.Add(l0); grid.Children.Add(l1); grid.Children.Add(l2); grid.Children.Add(l3); grid.Children.Add(l4); grid.Children.Add(l5); grid.Children.Add(l6); grid.Children.Add(l7); grid.Children.Add(l8); grid.Children.Add(bt0); grid.Children.Add(bt1);
                sp.Children.Add(grid);
            }
            close();
        }
        private void afficher(int i) {
            SQLiteCommand cm = new SQLiteCommand("SELECT * FROM Agent WHERE ID=@id", con);
            cm.Parameters.AddWithValue("@id", i);
            open();
            cm.ExecuteNonQuery();
            SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
            DataTable dt = new DataTable(); da.Fill(dt);
            txtmatricule.Text=dt.Rows[0]["ID"].ToString();
            txtnom.Text=dt.Rows[0]["nom"].ToString();
            txtPrenom.Text=dt.Rows[0]["prenom"].ToString();
            dateNaiss.Text=dt.Rows[0]["datenaiss"].ToString();
            txtcategorie.Text=dt.Rows[0]["categorie"].ToString();
            txtclasse.Text=dt.Rows[0]["classe"].ToString();
            cmbechelon.Text=dt.Rows[0]["echelon"].ToString();
            txtNombreEnfant.Text=dt.Rows[0]["enfant"].ToString();
            txtlibelle.Text = dt.Rows[0]["libelle"].ToString();
            cmbministere.Text = dt.Rows[0]["ministere"].ToString();
            close();
        }
        private void supprimer(int i) {
            if(MessageBox.Show("Confirmer La supression??", "Confirmation", MessageBoxButton.YesNo)==MessageBoxResult.Yes){
                SQLiteCommand cm = new SQLiteCommand("DELETE FROM Agent WHERE ID=@id", con);
                cm.Parameters.AddWithValue("@id", i);
                open();
                cm.ExecuteNonQuery();
                close();
                remplir(listEmploye);
            }
        }
        private void vider() {
            txtmatricule.Text ="";
            txtnom.Text = "";
            txtPrenom.Text = "";
            dateNaiss.Text = "";
            txtcategorie.Text="";
            txtclasse.Text = "";
            cmbechelon.Text ="";
            txtNombreEnfant.Text="";
            txtlibelle.Text = "";
            //cmbministere. ="";
        }
        private void modifier(int i) {
            SQLiteCommand cm = new SQLiteCommand("UPDATE Agent SET nom=@nom, prenom=@prenom, categorie=@cat, classe=@classe, echelon=@echelon, ministere=@min, libelle=@lib, datenaiss=@dn", con);
            cm.Parameters.AddWithValue("@nom", txtnom.Text);
            cm.Parameters.AddWithValue("@prenom", txtPrenom.Text);
            cm.Parameters.AddWithValue("@cat", txtcategorie.Text);
            cm.Parameters.AddWithValue("@classe", txtclasse.Text);
            cm.Parameters.AddWithValue("@echelon", cmbechelon.Text);
            cm.Parameters.AddWithValue("@min",cmbministere.Text);
            cm.Parameters.AddWithValue("@lib", txtlibelle.Text);
            cm.Parameters.AddWithValue("@dn",dateNaiss.Text);
            if (MessageBox.Show("Confirmer La Modification??", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                open();
                cm.ExecuteNonQuery();
                close();
            }
        }
        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            gridMain.Visibility = Visibility.Hidden;
            choiceGrid.Visibility = Visibility.Visible;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cm = new SQLiteCommand("INSERT INTO Agent(nom, prenom, classe, echelon, categorie, datenaiss, enfant, ministere, libelle) VALUES(@nom, @prenom, @classe, @echelon, @categorie, @datenaiss, @enfant, @ministere, @libelle)", con);
            cm.Parameters.AddWithValue("@nom", txtnom.Text);
            cm.Parameters.AddWithValue("@prenom", txtPrenom.Text);
            cm.Parameters.AddWithValue("@classe", txtclasse.Text);
            cm.Parameters.AddWithValue("@echelon", cmbechelon.Text);
            cm.Parameters.AddWithValue("@categorie", txtcategorie.Text);
            cm.Parameters.AddWithValue("@datenaiss", dateNaiss.Text);
            cm.Parameters.AddWithValue("@ministere", cmbministere.SelectedItem.ToString());
            cm.Parameters.AddWithValue("@libelle", txtlibelle.Text);
            try
            {
                int g = int.Parse(txtNombreEnfant.Text);
                cm.Parameters.AddWithValue("@enfant", txtNombreEnfant.Text);
                if (MessageBox.Show("Enregistrer??", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    open();
                    cm.ExecuteNonQuery();
                    close();
                    vider();
                    remplir(listEmploye);
                }
            }
            catch (Exception v) { MessageBox.Show("Le nombre d'enfant est incorrect"); txtNombreEnfant.Text = ""; }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (mod != 0)
            {
                if (MessageBox.Show("Modification??", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    modifier(mod);
                    mod = 0;
                    btnUpdate.IsEnabled = false;
                    vider();
                }
            }
            else { MessageBox.Show("Veuillez choisir une valeur"); btnUpdate.IsEnabled = false; }
        }
        private void cmbministere_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SQLiteCommand com = new SQLiteCommand("SELECT * FROM Ministere WHERE nom=@nm", con);
                com.Parameters.AddWithValue("@nm", cmbministere.SelectedItem.ToString());
                open();
                com.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(com);
                DataTable dt = new DataTable(); da.Fill(dt);
                txtlibelle.Text = dt.Rows[0]["libelle"].ToString();
                close();
            }
            catch (Exception et) { }            
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            vider(); mod = 0; btnUpdate.IsEnabled = false;
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cm = new SQLiteCommand("SELECT COUNT(*) as num FROM Users WHERE login=@login AND password=@password", con);
            cm.Parameters.AddWithValue("@login", txtlogin.Text);
            cm.Parameters.AddWithValue("@password", txtpassword.Password);
            try
            {
                open();
                cm.ExecuteNonQuery();
                SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
                DataTable dt = new DataTable(); da.Fill(dt);
                if (int.Parse(dt.Rows[0]["num"].ToString()) != 0)
                {
                    authGrid.Visibility = Visibility.Hidden;
                    choiceGrid.Visibility = Visibility.Visible;
                }
                else { MessageBox.Show("Informations incorrectes !!! "); }
                close();
            }
            catch (Exception ec) { MessageBox.Show(ec.ToString()); }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            SQLiteCommand cm = new SQLiteCommand("SELECT * FROM Agent WHERE ID=@mat", con);
            if (txtSearch.Text != "") { cm.Parameters.AddWithValue("@mat", txtSearch.Text);
            Grid grid = new Grid { Height = 45 };
            open(); cm.ExecuteNonQuery();
            ColumnDefinition[] col = { new ColumnDefinition { Width = new GridLength(50) }, new ColumnDefinition { Width = new GridLength(100) }, new ColumnDefinition { Width = new GridLength(120) }, new ColumnDefinition { Width = new GridLength(50) }, new ColumnDefinition { Width = new GridLength(50) }, new ColumnDefinition { Width = new GridLength(90) }, new ColumnDefinition { Width = new GridLength(110) }, new ColumnDefinition { Width = new GridLength(150) }, new ColumnDefinition { Width = new GridLength(90) } };
            for (int i = 0; i < col.Length; i++) { grid.ColumnDefinitions.Add(col[i]); }
            SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
            DataTable dt = new DataTable(); da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Label l0 = new Label { Content = dt.Rows[0]["ID"], VerticalContentAlignment = VerticalAlignment.Center },
                    l1 = new Label { Content = dt.Rows[0]["nom"], VerticalContentAlignment = VerticalAlignment.Center },
                    l2 = new Label { Content = dt.Rows[0]["prenom"], VerticalContentAlignment = VerticalAlignment.Center },
                    l3 = new Label { Content = dt.Rows[0]["categorie"], VerticalContentAlignment = VerticalAlignment.Center },
                    l4 = new Label { Content = dt.Rows[0]["classe"], VerticalContentAlignment = VerticalAlignment.Center },
                    l5 = new Label { Content = dt.Rows[0]["echelon"], VerticalContentAlignment = VerticalAlignment.Center },
                    l6 = new Label { Content = dt.Rows[0]["ministere"], VerticalContentAlignment = VerticalAlignment.Center },
                    l7 = new Label { Content = dt.Rows[0]["libelle"], VerticalContentAlignment = VerticalAlignment.Center },
                    l8 = new Label { Content = dt.Rows[0]["datenaiss"], VerticalContentAlignment = VerticalAlignment.Center };
                Grid.SetColumn(l0, 0); Grid.SetColumn(l1, 1); Grid.SetColumn(l2, 2); Grid.SetColumn(l3, 3); Grid.SetColumn(l4, 4); Grid.SetColumn(l5, 5); Grid.SetColumn(l6, 6); Grid.SetColumn(l7, 7); Grid.SetColumn(l8, 8);
                grid.Children.Add(l0); grid.Children.Add(l1); grid.Children.Add(l2); grid.Children.Add(l3); grid.Children.Add(l4); grid.Children.Add(l5); grid.Children.Add(l6); grid.Children.Add(l7); grid.Children.Add(l8);
                listSearch.Children.Add(grid);
            }
            else {
                listSearch.Children.Add(new Label { Content="Aucun résultat", FontSize=40});
            }
            } else { MessageBox.Show("Entrer un matricule!!!"); }

        }

        private void btnSearch10_Click(object sender, RoutedEventArgs e)
        {
            choiceGrid.Visibility = Visibility.Hidden;
            gridSearch.Visibility = Visibility.Visible;
        }

        private void btnOp_Click(object sender, RoutedEventArgs e)
        {
            choiceGrid.Visibility = Visibility.Hidden;
            gridMain.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            choiceGrid.Visibility = Visibility.Hidden;
            authGrid.Visibility = Visibility.Visible;
            txtlogin.Clear(); txtpassword.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            gridSearch.Visibility = Visibility.Hidden;
            choiceGrid.Visibility = Visibility.Visible;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtpassword_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter){
                SQLiteCommand cm = new SQLiteCommand("SELECT COUNT(*) as num FROM Users WHERE login=@login AND password=@password", con);
                cm.Parameters.AddWithValue("@login", txtlogin.Text);
                cm.Parameters.AddWithValue("@password", txtpassword.Password);
                try
                {
                    open();
                    cm.ExecuteNonQuery();
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    if (int.Parse(dt.Rows[0]["num"].ToString()) != 0)
                    {
                        authGrid.Visibility = Visibility.Hidden;
                        choiceGrid.Visibility = Visibility.Visible;
                    }
                    else { MessageBox.Show("Informations incorrectes !!! "); }
                    close();
                }
                catch (Exception ec) { MessageBox.Show(ec.ToString()); }
            }
        }
    }
}
