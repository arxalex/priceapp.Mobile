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
    public const string UsernameDoesNotExist = "User with Username /ph/ does not exsists";
    public const string EmailDoesNotExist = "User with Email /ph/ does not exists";
    public const string UnableToRegister = "Something went wrong: unable to register";
    public const string DeletingWentWrong = "Deleting user went wrong";
    public const string UpdatingPasswordWentWrong = "Updating password went wrong";
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
    public const string UsernameDoesNotExist = "Користувач з імʼям /ph/ не існує";
    public const string EmailDoesNotExist = "Користувач з Email /ph/ не існує";
    public const string UnableToRegister = "Щось пішло не так: неможливо зареєструватись";
    public const string DeletingWentWrong = "Під час видалення щось пішло не так";
    public const string UpdatingPasswordWentWrong = "Під час оновлення паролю щось пішло не так";
}

public static class TranslateUtill
{
    public static string TranslateException(string exception, string username = null, string email = null)
    {
        if (username != null)
        {
            if (ExceptionMessages.UsernameDoesNotExist.Replace("/ph/", username) == exception)
            {
                return ExceptionMessagesTranslated.UsernameDoesNotExist.Replace("/ph/", username);
            }
        }

        if (email != null)
        {
            if (ExceptionMessages.EmailDoesNotExist.Replace("/ph/", email) == exception)
            {
                return ExceptionMessagesTranslated.EmailDoesNotExist.Replace("/ph/", email);
            }
        }
        string result;
        switch (exception)
        {
            case ExceptionMessages.UsernameIncorrect:
                result = ExceptionMessagesTranslated.UsernameIncorrect;
                break;
            case ExceptionMessages.EmailIncorrect:
                result = ExceptionMessagesTranslated.EmailIncorrect;
                break;
            case ExceptionMessages.PasswordIncorrect:
                result = ExceptionMessagesTranslated.PasswordIncorrect;
                break;
            case ExceptionMessages.ProtectedUser:
                result = ExceptionMessagesTranslated.ProtectedUser;
                break;
            case ExceptionMessages.UsernameRegisterIncorrect:
                result = ExceptionMessagesTranslated.UsernameRegisterIncorrect;
                break;
            case ExceptionMessages.UserAlreadyExists:
                result = ExceptionMessagesTranslated.UserAlreadyExists;
                break;
            case ExceptionMessages.UserAlreadyExists2:
                result = ExceptionMessagesTranslated.UserAlreadyExists;
                break;
            case ExceptionMessages.ConfirmationIncorrect:
                result = ExceptionMessagesTranslated.ConfirmationIncorrect;
                break;
            case ExceptionMessages.ChangePasswordIncorrect:
                result = ExceptionMessagesTranslated.ChangePasswordIncorrect;
                break;
            case ExceptionMessages.UserDoesNotExists:
                result = ExceptionMessagesTranslated.UserDoesNotExists;
                break;
            case ExceptionMessages.SomethingWentWrong:
                result = ExceptionMessagesTranslated.SomethingWentWrong;
                break;
            case ExceptionMessages.UnableToRegister:
                result = ExceptionMessagesTranslated.UnableToRegister;
                break;
            case ExceptionMessages.DeletingWentWrong:
                result = ExceptionMessagesTranslated.DeletingWentWrong;
                break;
            case ExceptionMessages.UpdatingPasswordWentWrong:
                result = ExceptionMessagesTranslated.UpdatingPasswordWentWrong;
                break;
            default:
                result = ExceptionMessagesTranslated.SomethingWentWrong;
                break;
        }

        return result;
    }
}