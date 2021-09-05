using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
using Work_01_wpf.Models;

namespace Work_01_wpf
{
   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<string> titles = new List<string>()
            {
                "Md.",
                "Mr.",
                "Miss",
                "Mrs"
            };
            this.cmbTitle.ItemsSource = titles;
            cmbTitle.Text = "Md.";
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Feculty fe = new Feculty();
                fe.Id = Convert.ToInt32(txtId.Text);
                fe.Title = cmbTitle.SelectedItem.ToString();
                fe.FirstName = txtFirstName.Text;
                fe.LastName = txtLastName.Text;
                fe.Email = txtEmail.Text;
                fe.Contact = txtContactNo.Text;


                var newFecultyMember = "{'Id':'" + fe.Id + "','Title':'" + fe.Title + "','FirstName':'" + fe.FirstName + "','LastName':'" + fe.LastName + "','Email':'" + fe.Email + "','Contact':'" + fe.Contact + "'}";

                var json = File.ReadAllText(@"Feculty.json");
                var jsonObj = JObject.Parse(json);
                var fecultyArray = jsonObj.GetValue("Feculty") as JArray;
                var newFeculty = JObject.Parse(newFecultyMember);
                fecultyArray.Add(newFeculty);

                jsonObj["Feculty"] = fecultyArray;
                string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(@"Feculty.json", newJsonResult);

                ShowData();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowData();
        }

        private void ShowData()
        {
            var json = File.ReadAllText(@"Feculty.json");
            var jsonObj = JObject.Parse(json);
            if (jsonObj != null)
            {
                JArray feArray = (JArray)jsonObj["Feculty"];
                List<Feculty> fe = new List<Feculty>();
                foreach (var item in feArray)
                {
                    fe.Add(new Feculty() { Id = Convert.ToInt32(item["Id"]), Title=item["Title"].ToString(), FirstName = item["FirstName"].ToString(), LastName = item["LastName"].ToString(), Email = item["Email"].ToString(), Contact = item["Contact"].ToString() });
                }
                lstFeculty.ItemsSource = fe;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            btnUpdate.Visibility = Visibility.Visible;
            Button b = sender as Button;
            Feculty feBtn = b.CommandParameter as Feculty;
            int feId = feBtn.Id;
            txtId.Text = feId.ToString();
            txtFirstName.Text = feBtn.FirstName.ToString();
            txtLastName.Text = feBtn.LastName.ToString();
            txtEmail.Text = feBtn.Email.ToString();
            txtContactNo.Text = feBtn.Contact.ToString();
            txtId.IsEnabled = false;

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var json = File.ReadAllText(@"Feculty.json");
            var jsonObj = JObject.Parse(json);
            JArray feArray = (JArray)jsonObj["Feculty"];

            Button b = sender as Button;
            Feculty feBtn = b.CommandParameter as Feculty;
            int feId = feBtn.Id;

            if (feId > 0)
            {
                var fecultyToDeleted = feArray.FirstOrDefault(obj => obj["Id"].Value<int>() == feId);
                feArray.Remove(fecultyToDeleted);
                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(@"Feculty.json", output);
                MessageBox.Show("Data deleted successfully!!!");
                ShowData();
            }
            else
            {
                MessageBox.Show("Not deleted....");
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var jsonU = File.ReadAllText(@"Feculty.json");
            var jsonObj = JObject.Parse(jsonU);
            JArray feUpdateArray = (JArray)jsonObj["Feculty"];

            var Id = Convert.ToInt32(txtId.Text);
            var Title = cmbTitle.SelectedItem.ToString();
            var FirstName = txtFirstName.Text;
            var LastName = txtLastName.Text;
            var Email = txtEmail.Text;
            var Contact = txtContactNo.Text;

            foreach (var feculty in feUpdateArray.Where(obj=>obj["Id"].Value<int>()==Id))
            {
                feculty["Title"] = !string.IsNullOrEmpty(Title) ? Title : "";
                feculty["FirstName"] = !string.IsNullOrEmpty(FirstName) ? FirstName : "";
                feculty["LastName"] = !string.IsNullOrEmpty(LastName) ? LastName : "";
                feculty["Email"] = !string.IsNullOrEmpty(Email) ? Email : "";
                feculty["Contact"] = !string.IsNullOrEmpty(Contact) ? Contact : "";
            }
            jsonObj["Feculty"] = feUpdateArray;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(@"Feculty.json", output);
            ShowData();
        }

        private void btnreferish_Click(object sender, RoutedEventArgs e)
        {
            this.txtContactNo.Clear();
            this.txtEmail.Clear();
            this.txtFirstName.Clear();
            this.txtLastName.Clear();
            this.txtId.Clear();
            
        }
    }
}
