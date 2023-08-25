namespace MaybeMonadConsole.Implementations;

public class MaybeMonad
{
    #region Fields
    
    private readonly static Dictionary<int, User> _users = new()
    {
        {1, new User()
            {
                Name = "Farshad Fahimi",
                Age = Maybe<int>.Some(value: 28),
                Email = Maybe<string>.Some("FarshadFahimi5@gmail.com")
            }
        },
        {2, new User()
            {
                Name = "Martin Fowler",
                Age = Maybe<int>.Some(value: 45),
                Email = Maybe<string>.Some("")
            }
        }
    };

    #endregion

    #region Methods

    public static void SendingEmailExampleUsage(int userId)
    {
        var user = GetUserFromAPI(userId);

        var result = user
            .Select(u => $"{u.Name} (Age: {u.Age.Value}, Email: {u.Email.Value})")
            .SelectMany(SendEmail);

        if (result.HasValue)
        {
            Console.WriteLine("Email sent successfully.");
        }
        else
        {
            Console.WriteLine("Failed to send email.");
        }
    }
    public static void EmailValidationExampleUsage(int userId)
    {
        var user = GetUserFromAPI(userId);

        var result = user
            .Select(u => u.Email.Value)
            .SelectMany(CheckUserEmail);

        if (result.HasValue)
        {
            Console.WriteLine($"User Email is valid : {result.Value}");
        }
        else
        {
            Console.WriteLine("Invalid email address.");
        }
    }

    #endregion

    #region Privates
    private static Maybe<bool> CheckUserEmail(string emailMessage)
    {
        return !string.IsNullOrWhiteSpace(emailMessage)
            ? Maybe<bool>.Some(true)
            : Maybe<bool>.None();
    }

    private static Maybe<User> GetUserFromAPI(int userId)
    {
        // Simulating API call and response
        _users.TryGetValue(userId, out User? response);

        if (response is null)
        {
            return Maybe<User>.None();
        }

        var user = new User()
        {
            Name = response.Name,
            Age = response.Age,
            Email = response.Email
        };
        return Maybe<User>.Some(user);
    }

    private static Maybe<bool> SendEmail(string message)
    {
        // Simulating email sending process
        if (!string.IsNullOrEmpty(message))
        {
            Console.WriteLine($"Email has been sent : {message}");
            return Maybe<bool>.Some(true);
        }
        else
        {
            return Maybe<bool>.None();
        }
    } 
    #endregion
}
