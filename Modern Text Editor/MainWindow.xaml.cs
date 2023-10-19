using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;


namespace Modern_Text_Editor;


public partial class MainWindow : Window
{
    public string FilePath = string.Empty;
    public string selectedText;
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        foreach (FontFamily fontFamily in Fonts.SystemFontFamilies)
        {
            ComboBoxItem item = new ComboBoxItem();
            item.Content = fontFamily.Source;
            
            FontComboBox.Items.Add(fontFamily);
        }
    }

   

    private void BoldBtn_Click(object sender, RoutedEventArgs e)
    {
        // Text secilmeyibse hamsini bold edir
        if (RichTxtBoxxx.Selection.Text.Length == 0)
            RichTxtBoxxx.FontWeight = FontWeights.Bold;
        // Secilibse yanliz secileni bold edir
        else
            RichTxtBoxxx.Selection.ApplyPropertyValue(TextElement.FontWeightProperty,FontWeights.Bold);
    }

    private void ItalicBtn_Click(object sender, RoutedEventArgs e)
    {
        // Text secilmeyibse hamsini bold edir
        if (RichTxtBoxxx.Selection.Text.Length == 0)
            RichTxtBoxxx.FontStyle = FontStyles.Italic;
        // Secilibse yanliz secileni bold edir
        else
            RichTxtBoxxx.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
    }

    private void DefalultFontBtn_Click(object sender, RoutedEventArgs e)
    {
        if (RichTxtBoxxx.Selection.Text.Length == 0)
        {
            RichTxtBoxxx.FontWeight = FontWeights.Normal;
            RichTxtBoxxx.FontStyle = FontStyles.Normal;
        }
        else
        {
            RichTxtBoxxx.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            RichTxtBoxxx.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);

        }
    }

    private void UnderlinedBtn_Click(object sender, RoutedEventArgs e)
    {
        if (RichTxtBoxxx.Selection.Text.Length > 0)
        {
            RichTxtBoxxx.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }
       
        
    }

    private void StrikeBtn_Click(object sender, RoutedEventArgs e)
    {
        if (RichTxtBoxxx.Selection.Text.Length > 0)
        {
            RichTxtBoxxx.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
        }
    }

    private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        RichTxtBoxxx.FontFamily = new System.Windows.Media.FontFamily(FontComboBox.SelectedItem.ToString());
    }

    private void FontSizeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)FontSizeCombobox.SelectedItem;

       
        RichTxtBoxxx.FontSize = double.Parse(selectedItem.Content.ToString());
    }

    private void LeftAlligmentBtn_Click(object sender, RoutedEventArgs e)
    {
        RichTxtBoxxx.HorizontalContentAlignment = HorizontalAlignment.Left;
    }

    private void CenterAlligmentBtn_Click(object sender, RoutedEventArgs e)
    {
        RichTxtBoxxx.HorizontalContentAlignment = HorizontalAlignment.Center;

    }

    private void RightAlligmentBtn_Click(object sender, RoutedEventArgs e)
    {
        RichTxtBoxxx.HorizontalContentAlignment = HorizontalAlignment.Right;

    }

    private void ColorPickerCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ComboBoxItem selectedItem = (ComboBoxItem)ColorPickerCombobox.SelectedItem;
        RichTxtBoxxx.Foreground = (SolidColorBrush)selectedItem.Background;
    }

    private void SaveBtn_Click(object sender, RoutedEventArgs e)
    {
        SaveFileDialog saveFileDialog = new();
        if (saveFileDialog.ShowDialog() == true)
        {
            using Stream stream = File.Open(saveFileDialog.FileName, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                TextRange textRange = new(RichTxtBoxxx.Document.ContentStart, RichTxtBoxxx.Document.ContentEnd);
                writer.Write(textRange.Text);
            };
        }
        


    }

    private void UploadBtn_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Txt Files|*.txt";
        if (openFileDialog.ShowDialog() == true)
        {
            string file = File.ReadAllText(openFileDialog.FileName.ToString());
            TextRange textRange = new TextRange(RichTxtBoxxx.Document.ContentEnd, RichTxtBoxxx.Document.ContentEnd);
            textRange.Text = file;
            FilePath = openFileDialog.FileName.ToString();
           
        }

    }

    private void RichTxtBoxxx_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (AutoSaveCheckBox.IsChecked == true)
        {
            using Stream stream = File.Open(FilePath, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(stream))
            {
                TextRange textRange = new(RichTxtBoxxx.Document.ContentStart, RichTxtBoxxx.Document.ContentEnd);
                writer.Write(textRange.Text);
            };
        }
    }

    private void SelectAllBtn_Click(object sender, RoutedEventArgs e)
    {
        RichTxtBoxxx.SelectAll();
        selectedText = RichTxtBoxxx.Selection.Text;
    }

    private void PasteBtn_Click(object sender, RoutedEventArgs e)
    {
        if (!selectedText.Equals(string.Empty))
        {
            TextPointer textPointer = RichTxtBoxxx.Document.ContentEnd;
            textPointer.InsertTextInRun(selectedText);
        }
    }

    private void CutBtn_Click(object sender, RoutedEventArgs e)
    {
        selectedText = RichTxtBoxxx.Selection.Text;
        RichTxtBoxxx.Cut();
    }

    private void CopyBtn_Click(object sender, RoutedEventArgs e)
    {
        selectedText = RichTxtBoxxx.Selection.Text;
        RichTxtBoxxx.Copy();
    }

    private void AboutBtn_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Auto savenin islemsei ucun her hansi bir fayli upload etmelisiniz!", "About", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
