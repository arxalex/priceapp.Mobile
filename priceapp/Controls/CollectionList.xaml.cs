using System.Collections.ObjectModel;
using priceapp.Controls.Models;

namespace priceapp.Controls;

public partial class CollectionList
{
    public CollectionList()
    {
        InitializeComponent();
        CollectionView.SetBinding(ItemsView.RemainingItemsThresholdProperty, new Binding("RemainingItemsThreshold", source:this));
        CollectionView.RemainingItemsThresholdReached += CollectionViewOnRemainingThresholdReached;
    }

    public event EventHandler? RemainingItemsThresholdReached;

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<ImageButtonModel>),
            typeof(CollectionList));

    public ObservableCollection<ImageButtonModel> ItemsSource
    {
        get => (ObservableCollection<ImageButtonModel>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    
    public static readonly BindableProperty RemainingItemsThresholdProperty =
        BindableProperty.Create(nameof(RemainingItemsThreshold), typeof(int), typeof(ItemsView), -1, validateValue: (bindable, value) => (int)value >= -1);

    public int RemainingItemsThreshold
    {
        get => (int)GetValue(RemainingItemsThresholdProperty);
        set => SetValue(RemainingItemsThresholdProperty, value);
    }

    private void CollectionViewOnRemainingThresholdReached(object sender, EventArgs e)
    {
        RemainingItemsThresholdReached?.Invoke(this, e);
    }
}