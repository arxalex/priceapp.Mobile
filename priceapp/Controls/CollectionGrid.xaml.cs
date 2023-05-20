using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using priceapp.Controls.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace priceapp.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CollectionGrid
{
    public CollectionGrid()
    {
        InitializeComponent();
        CollectionView.SetBinding(ItemsView.RemainingItemsThresholdProperty, new Binding("RemainingItemsThreshold", source:this));
        CollectionView.RemainingItemsThresholdReached += CollectionViewOnRemainingThresholdReached;
        var size = Measure(Double.PositiveInfinity, Double.PositiveInfinity);
        ItemWidth += (size.Request.Width - 45) / 2;
    }

    public event EventHandler RemainingItemsThresholdReached;

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(ObservableCollection<ImageButtonModel>),
            typeof(CollectionGrid), propertyChanged: PropertyChanged);

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
    
    public ObservableCollection<ObservableCollection<ImageButtonModel>> ItemsSourcePerRow { get; set; } = new();
    public double ItemWidth { get; set; } = 0;
    
    private void CollectionViewOnRemainingThresholdReached(object sender, EventArgs e)
    {
        RemainingItemsThresholdReached?.Invoke(this, e);
    }
    
    private static void PropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        ((CollectionGrid) bindable).ItemsSourcePerRow.Clear();
        ((ObservableCollection<ImageButtonModel>)newvalue).CollectionChanged += ((CollectionGrid) bindable).ValueOnCollectionChanged;
    }

    private void ValueOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Reset)
        {
            ItemsSourcePerRow.Clear();
            return;
        }
        
        var oldList = e.OldItems != null
            ? e.OldItems.Cast<ImageButtonModel>().ToList()
            : new List<ImageButtonModel>();
        var newList = e.NewItems != null
            ? e.NewItems.Cast<ImageButtonModel>().ToList()
            : new List<ImageButtonModel>();
        var newListCopy = newList.ToList();
        newList.RemoveAll(y => oldList.Any(x => x.Equals(y)));
        oldList.RemoveAll(y => newListCopy.Any(x => x.Equals(y)));

        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Replace:
            {
                if (oldList.Count != newList.Count)
                {
                    return;
                }
                
                for (var i = 0; i < newList.Count; i++)
                {
                    var rowToChange = ItemsSourcePerRow.FirstOrDefault(x => x.Any(y => y.Equals(oldList[i])));
                    if (rowToChange == null)
                    {
                        continue;
                    }
                    var index = rowToChange.IndexOf(oldList[i]);
                    rowToChange[index] = newList[i];
                }
            
                return;
            }
            case NotifyCollectionChangedAction.Remove:
            {
                foreach (var itemToRemove in oldList)
                {
                    var rowToChange = ItemsSourcePerRow.FirstOrDefault(x => x.Any(y => y.Equals(itemToRemove)));
                    if (rowToChange == null)
                    {
                        continue;
                    }

                    rowToChange.Remove(itemToRemove);
                }

                return;
            }
            case NotifyCollectionChangedAction.Add:
            {
                if (ItemsSourcePerRow.Count > 0)
                {
                    var lastRow = ItemsSourcePerRow.Last();
                    if (lastRow.Count % 2 != 0)
                    {
                        var itemToAdd = newList.First();
                        newList.Remove(itemToAdd);
                        lastRow.Add(itemToAdd);
                    }
                }

                var newRows = newList
                    .Select((value, index) => (value, index))
                    .GroupBy(x => x.index / 2)
                    .Select(x => x.Select(y => y.value).ToList());
            
                newRows.ForEach(x => ItemsSourcePerRow.Add(new ObservableCollection<ImageButtonModel>(x)));
            
                return;
            }
        }
    }
}