using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using priceapp.Annotations;
using priceapp.UI;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(OnboardingViewModel))]

namespace priceapp.ViewModels;

public class OnboardingViewModel : IOnboardingViewModel
{
    private string _nextButtonText;
    private int _position;

    public OnboardingViewModel()
    {
        NextButtonText = "Далі";
        SkipButtonText = "Пропустити";
        Items = new ObservableCollection<OnboardingItem>
        {
            new()
            {
                Title = "Вітаю!",
                Content = "Це невелика підказка по користуванню додатком. Давайте почнемо!",
                ImageUrl = "splash_logo.png"
            },
            new()
            {
                Title = "Пошук продуктів",
                Content = "Натисніть на пошук та введіть назву продукту або перейдіть до потрібної категорії.",
                ImageUrl = "onboarding_1.png"
            },
            new()
            {
                Title = "Оберіть продукт",
                Content = "Оберіть продукт. Ви одразу можете побачити ціни навколо вас.",
                ImageUrl = "onboarding_2.png"
            },
            new()
            {
                Title = "Додайте товар до списку покупок",
                Content = "Натисніть \"Додати\".",
                ImageUrl = "onboarding_3.png"
            },
            new()
            {
                Title = "Перегляньте ціни на мапі",
                Content = "Опустіться нижче щоб побачити ціни навколо вас.",
                ImageUrl = "onboarding_4.png"
            },
            new()
            {
                Title = "Список покупок",
                Content =
                    "Натисніть на \"Кошик\" щоб переглянути товари у списку покупок. Тут відображаються найнижчі ціни та адреси магазинів. Проведіть зверху донизу щоб оновити список.",
                ImageUrl = "onboarding_5.png"
            },
            new()
            {
                Title = "Налаштуйте під себе",
                Content =
                    "Натисніть \"Акаунт\" та перейдіть до налаштувань. Тут ви можете змінити радіус пошуку та змінити спосіб формування списку покупок.",
                ImageUrl = "onboarding_6.png"
            }
        };
    }

    public ObservableCollection<OnboardingItem> Items { get; set; }

    public string NextButtonText
    {
        get => _nextButtonText;
        set
        {
            _nextButtonText = value;
            OnPropertyChanged();
        }
    }

    public string SkipButtonText { get; set; }

    public int Position
    {
        get => _position;
        set
        {
            _position = value;
            NextButtonText = Position == Items.Count - 1 ? "Зрозуміло" : "Далі";
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}