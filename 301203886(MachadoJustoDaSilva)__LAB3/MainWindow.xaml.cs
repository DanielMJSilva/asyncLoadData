using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace _301203886_MachadoJustoDaSilva___LAB3
{
    
    public partial class MainWindow : Window
    {

        private List<Stock> data;
        public MainWindow()
        {
            InitializeComponent();
            combobox_symbol.IsEnabled = false;
            lbl_search.IsEnabled = false;
            btn_clear_search.IsEnabled = false;
            btn_clear_data.IsEnabled = false;
            btn_search.IsEnabled = false;
        }

        // Load data async
        private async void btn_load_data_Click(object sender, RoutedEventArgs e)
        {

            await Task.Run(() =>
            {
                Stocks stocks = new Stocks();
                data = stocks.GetStocks();

            });
            datagrid.ItemsSource = data.OrderBy(s => s.Date);
            combobox_symbol.ItemsSource = data.Select(s => s.Symbol).Distinct();
            combobox_symbol.IsEnabled = true;
            lbl_search.IsEnabled = true;
            btn_search.IsEnabled = true;
            btn_clear_search.IsEnabled = true;
            btn_clear_data.IsEnabled = true;
        }

        //Clear loaded data
        private void btn_clear_data_Click(object sender, RoutedEventArgs e)
        {
            data.Clear();
            datagrid.ItemsSource = data;
            combobox_symbol.ItemsSource = data;
            combobox_symbol.IsEnabled = false;
            lbl_search.IsEnabled = false;
            btn_clear_search.IsEnabled = false;
            btn_clear_data.IsEnabled = false;
            btn_search.IsEnabled = false;
            //progressBar.Value = 0;
            //progressLabel.Text = "";
        }

        //Search for company
        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            if (combobox_symbol.SelectedIndex != -1)
            {
                datagrid.ItemsSource = data.Where(s => s.Symbol == combobox_symbol.SelectedItem.ToString()).OrderBy(s => s.Date);
                btn_clear_search.IsEnabled = true;
            }
        }

        //Clear data searched
        private void btn_clearSearch_Click(object sender, RoutedEventArgs e)
        {
            datagrid.ItemsSource = data.OrderBy(s => s.Date);
            combobox_symbol.SelectedIndex = -1;
            btn_clear_search.IsEnabled = false;
        }

        //Calculate async factorial
        private static Task<int> FactorialAsync(int n)
        {
            return Task.Run(() => CalcFact(n));

            int CalcFact(int i)  //Local function  
            {
                if (i == 1)
                {
                    return 1;
                }

                return i * CalcFact(i - 1);
            }
        }

        //call Async factorial
        private async void btn_calculate_fatorial_Click(object sender, RoutedEventArgs e)
        {
            int num = Convert.ToInt32(txt_fatorial.Text);
            long factorial = await FactorialAsync(num);

            MessageBox.Show("Factorial of " + num + " = " + factorial +".", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                
        }

       
}
    public class Stock
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }

    }

  
    public class Stocks
    {
        public List<Stock> GetStocks()
        {
            var file = @"C:\Users\DMJS\Desktop\Daniel\Software Eng\COMP212 - Programming 3\Assignment 3\lab3.package\stockData.csv";
            var list = new List<Stock>();

            using (TextFieldParser csvParser = new TextFieldParser(file))
            {
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip first row with column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                
                    string[] fields = csvParser.ReadFields();
                    char[] trimChars = { '$' };
                    Stock stock = new Stock();
                    stock.Symbol = fields[0];
                    stock.Date = Convert.ToDateTime(fields[1]);
                    stock.Open = Decimal.Parse(fields[2].Trim(trimChars), System.Globalization.NumberStyles.Currency);
                    stock.High = Decimal.Parse(fields[3].Trim(trimChars), System.Globalization.NumberStyles.Currency);
                    stock.Low = Decimal.Parse(fields[4].Trim(trimChars), System.Globalization.NumberStyles.Currency);
                    stock.Close = Decimal.Parse(fields[5].Trim(trimChars), System.Globalization.NumberStyles.Currency);

                    //remove negative values while the data is loading
                    if (stock.Open >= 0 && stock.High >= 0 && stock.Low >= 0 && stock.Close >= 0)
                    {
                        list.Add(stock);
                    }

                }
            }
            return list;
     
        } //End GetStocks
      
    } //End Stocks   


}//end namespace
