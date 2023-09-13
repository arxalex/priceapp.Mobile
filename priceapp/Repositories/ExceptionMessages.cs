namespace priceapp.Repositories;

public static class ExceptionMessages
{
    public const string UsernameIncorrect = "Username or password invalid";
    public const string EmailIncorrect = "Email or password invalid";
    public const string PasswordIncorrect = "Password is incorrect";
    public const string UsernameRegisterIncorrect = "Username, email or password invalid";
    public const string UserAlreadyExists = "User with this username or email already exists";
    public const string UserAlreadyExists2 = "This username or email already registered";
    public const string ConfirmationIncorrect = "Confirmation token does not exist";
    public const string ChangePasswordIncorrect = "Old or new password is invalid";
    public const string UserDoesNotExists = "User does not exist";
    public const string SomethingWentWrong = "Something went wrong";
    public const string ProtectedUser = "You cant change protected user";
}

public static class ExceptionMessagesTranslated
{
    public const string UsernameIncorrect = "Імʼя користувача чи пароль невірні";
    public const string EmailIncorrect = "E-mail чи пароль невірні";
    public const string PasswordIncorrect = "Пароль неправильний";
    public const string UsernameRegisterIncorrect = "Імґя користувача, e-mail або пароль невірні";
    public const string UserAlreadyExists = "Користувач з таким імʼям вже існує";
    public const string ConfirmationIncorrect = "Токен підтвердження не існує";
    public const string ChangePasswordIncorrect = "Старий або новий паролі невірні";
    public const string UserDoesNotExists = "Такого користувача не існує";
    public const string SomethingWentWrong = "Щось пішло не так";
    public const string ProtectedUser = "Ви не можете змінювати захищеного користувача";
}