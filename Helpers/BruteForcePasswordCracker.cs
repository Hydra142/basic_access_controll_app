using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
namespace SafeMessenge.Helpers;

public class BruteForcePasswordCracker
{
    private string _password;
    private string _result;

    private bool _isMatched = false;
    private int _charactersToTestLength = 0;
    private long _computedKeys = 0;
    private long _previousComputedKeys = 0;
    private int _minPasswordLength = 1;
    private int _maxPasswordLength = 1;
    private static char[] _characters = {};
    private Timer _update_timer;
    private bool _isStatusDisplayed = false;
    public BruteForceStatus Status { get; set; } = new();

    public BruteForcePasswordCracker(BruteForceSettings settings)
    {
        _password = settings.Password;
        // якщо довжина не відома обираємо стандартну - 15 символів
        settings.Length = settings.Length == 0 || !settings.IsLengthKnown ? 15 : settings.Length;
        // встановлюємо макс і мін довжину базуючись на довжині пароля та чи містить вона похибку +-1
        _maxPasswordLength = 
            settings.IsLengthApproximate && settings.IsLengthKnown 
            ? settings.Length + 1 
            : settings.Length;
        _minPasswordLength = settings.IsLengthApproximate && settings.Length > 0
            ? settings.Length - 1 
            : settings.IsLengthKnown 
                ? settings.Length 
                : 1;
        //встановлюємо довжину массиву символів для оптимізації швидкості
        _charactersToTestLength = settings.Characters.Length;
        _characters = settings.Characters.ToCharArray();
        // обраховуємо к-ть кобмінацій
        Status.TotalCombinations = Math.Pow(_charactersToTestLength, settings.Length);
        if (settings.IsLengthApproximate && settings.IsLengthKnown)
        {
            Status.TotalCombinations += Math.Pow(_charactersToTestLength, _minPasswordLength) + Math.Pow(_charactersToTestLength, _maxPasswordLength);
        }
    }

    public async Task<BruteForceStatus> Start(CancellationToken cancellationToken)
    {
        var timeStarted = DateTime.Now;
        // через 1 секунду після старту буде запущено UpdateStatus
        // яка обрахує швидкість та очікуваний час завершення підбору
        _update_timer = new Timer(UpdateStatus, null, 1000, Timeout.Infinite);
        try
        {
            //підбір почнеться з комбінації мінімальної довжини
            var passLength = _minPasswordLength;
            //Підбираємо пароль доки не пройдемо всі кобмінації можливих довжин
            while (!_isMatched && passLength <= _maxPasswordLength)
            {
                startBruteForce(passLength, cancellationToken);
                //збільшуємо довжину
                passLength++;
            }
        }
        catch (OperationCanceledException)
        {
        }
        // повертаємо результати
        Random rnd = new Random();
        var min = 0;
        var sec = 0;//rnd.Next(0, 30);
        var msec = 0;//rnd.Next(124,899);
        var now = DateTime.Now;
        Status.TimeSpent = $"{now.AddMinutes(min).AddSeconds(sec).AddMilliseconds(msec).Subtract(timeStarted).TotalSeconds}s";
        Status.FoundPassword = _result;
        Status.Message = !cancellationToken.IsCancellationRequested
            ? _result.IsNullOrEmpty()
                ? "Невдача"
                : "Успіх"
            : "Скасовано";
        return Status;
    }


    private void startBruteForce(int keyLength, CancellationToken cancellationToken)
    {
        var keyChars = createCharArray(keyLength, _characters[0]);
        var indexOfLastChar = keyLength - 1;
        // запуск підбору починаючи з першого символу набору 
        createNewKey(0, keyChars, keyLength, indexOfLastChar, cancellationToken);
    }

    private char[] createCharArray(int length, char defaultChar)
    {
        return (from c in new char[length] select defaultChar).ToArray();
    }


    private void createNewKey(
        int currentCharPosition,
        char[] keyChars,
        int keyLength,
        int indexOfLastChar,
        CancellationToken cancellationToken)
    {
        // перехід до наступного символу
        var nextCharPosition = currentCharPosition + 1;
        // проходження через кожен символ набору
        for (int i = 0; i < _charactersToTestLength; i++)
        {
            //перевірка чи підбір було скасовано
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            /* Символ у currentCharPosition буде замінено на a
              * новий символ із масиву _characters => буде створено нову комбінацю */
            keyChars[currentCharPosition] = _characters[i];

            // метод буде викликати сам себе доки не перебере всі можливі комбінації або не знайде пароль
            if (currentCharPosition < indexOfLastChar && !_isMatched)
            {
                createNewKey(nextCharPosition, keyChars, keyLength, indexOfLastChar, cancellationToken);
            }
            else
            {
                // обраховуємо швидкість тільки якщо вона ще не обрахована
                if (!_isStatusDisplayed)
                {
                    Interlocked.Increment(ref _computedKeys);
                } else
                {
                    //_isMatched = true;
                    //_result = _password;
                }
                //перевіряємо чи поточний набір символів відповідає паролю
                if ((new String(keyChars)) == _password)
                {
                    if (!_isMatched)
                    {
                        _isMatched = true;
                        _result = new String(keyChars);
                    }
                    return;
                }
            }
        }
    }

    private void UpdateStatus(object state)
    {
        CalculateSpeed(state);
        CalculateEstimated();
        _isStatusDisplayed = true;
        _update_timer.Dispose();
    }

    private void CalculateSpeed(object state)
    {
        long currentComputedKeys = Interlocked.Read(ref _computedKeys);
        long keysPerSecond = currentComputedKeys - _previousComputedKeys;
        _previousComputedKeys = currentComputedKeys;
        Status.Speed = keysPerSecond;
    }

    private void CalculateEstimated()
    {
        if (Status.Speed != 0)
        {
            // обріховуємо макс час виконання збільшуючи швидкість на 10%
            // так як збільшення лічильника уповільнює процес на 10-20%
            double totalTimeInSeconds = (Status.TotalCombinations) / (Status.Speed * (1 + (10 / 100)));
            TimeSpan totalTime = new();
            var prefix = string.Empty;
            try
            {
                totalTime = TimeSpan.FromDays(((totalTimeInSeconds / 60) / 60) / 24);

            }
            catch (Exception)
            {
                // знайдений час може не вміститись в TimeSpan
                // тому відобразимо максимальне його значення
                prefix = "більше ";
                totalTime = TimeSpan.MaxValue;
            }            
            int days = totalTime.Days;
            int hours = totalTime.Hours;
            int minutes = totalTime.Minutes;
            int seconds = totalTime.Seconds;
            int ms = totalTime.Milliseconds;
            Status.TimeEstimated = $"{prefix} {days}д, {hours}г, {minutes}хв, {seconds}с, {ms}мс";
        }
    }
}

public class BruteForceStatus
{
    public string TimeSpent = string.Empty;
    public string FoundPassword = string.Empty;
    public string Message = string.Empty;
    public string TimeEstimated = string.Empty;
    public double TotalCombinations = 0;
    public long Speed = 0;
}

public class BruteForceSettings
{
    public string Password = string.Empty;
    public string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    public bool IsLengthKnown = false;
    public bool IsLengthApproximate = false;
    public int Length = 1;

    public override string ToString()
    {
        return $"\nдовжина:{(IsLengthKnown ? Length : "невідомо")}{(IsLengthApproximate ? "(+-1)" : "")}, символи:{Characters}";
    }
}
