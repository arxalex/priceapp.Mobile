using System.Windows.Input;
using Xamarin.Forms;

namespace priceapp.Controls.Models;

public class ImageButtonModel
{
    public int Id { get; set; }
    public string PrimaryText { get; set; }
    public string SecondaryText { get; set; }
    public string AccentText { get; set; }
    public string AdditionalText { get; set; }
    public Color AdditionalTextColor { get; set; }
    public ImageSource Image { get; set; }
    public ICommand Command { get; set; }
}