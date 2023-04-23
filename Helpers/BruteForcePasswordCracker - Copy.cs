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
    // the secret password which we will try to find via brute force
    private string _password = "qwerty";
    private string _result;

    private bool _isMatched = false;
    private int _charactersToTestLength = 0;
    private long _computedKeys = 0;
    private long _previousComputedKeys = 0;
    private int _length_variation = 0;
    private int _estimatedPasswordLength = 6;
    private Timer _update_timer;
    private bool _isStatusDisplayed = false;
    public BruteForceStatus Status { get; set; } = new();



    /* An array containing the characters which will be used to create the brute force keys,
     * if less characters are used (e.g. only lower case chars) the faster the password is matched  */
    private static char[] charactersToTest =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z'/*,'A','B','C','D','E',
        'F','G','H','I','J','K','L','M','N','O','P','Q','R',
        'S','T','U','V','W','X','Y','Z','1','2','3','4','5',
        '6','7','8','9','0','!','$','#','@','-'*/
    };

    public async Task<BruteForceStatus> Start(CancellationToken cancellationToken/*, Action<BruteForcePasswordCrackerResult> onOperationCompleted*/)
    {
        var timeStarted = DateTime.Now;
        // The length of the array is stored permanently during runtime
        _charactersToTestLength = charactersToTest.Length;
        Status.TotalCombinations = Math.Pow(_charactersToTestLength, _estimatedPasswordLength + _length_variation);
        
        // The length of the password is unknown, so we have to run trough the full search space

        _update_timer = new Timer(UpdateStatus, null, 1000, Timeout.Infinite);
        try
        {
            var passLength = _estimatedPasswordLength != 0 ? _estimatedPasswordLength - _length_variation : _estimatedPasswordLength;
            while (!_isMatched && passLength <= _estimatedPasswordLength + _length_variation)
            {
                /* The estimated length of the password will be increased and every possible key for this
                 * key length will be created and compared against the password */

                startBruteForce(passLength, cancellationToken);
                passLength++;
            }
        }
        catch (OperationCanceledException)
        {
        }
        Status.TimeSpent = $"{DateTime.Now.Subtract(timeStarted).TotalSeconds}s";
        Status.FoundPassword = _result;
        Status.Message = !cancellationToken.IsCancellationRequested ? _result.IsNullOrEmpty() ? "Невдача" : "Успіх" : "Скасовано";
        return Status;
    }


    /// <summary>
    /// Starts the recursive method which will create the keys via brute force
    /// </summary>
    /// <param name="keyLength">The length of the key</param>
    private void startBruteForce(int keyLength, CancellationToken cancellationToken)
    {
        var keyChars = createCharArray(keyLength, charactersToTest[0]);
        // The index of the last character will be stored for slight perfomance improvement
        var indexOfLastChar = keyLength - 1;
        createNewKey(0, keyChars, keyLength, indexOfLastChar, cancellationToken);
    }

    /// <summary>
    /// Creates a new char array of a specific length filled with the defaultChar
    /// </summary>
    /// <param name="length">The length of the array</param>
    /// <param name="defaultChar">The char with whom the array will be filled</param>
    /// <returns></returns>
    private char[] createCharArray(int length, char defaultChar)
    {
        return (from c in new char[length] select defaultChar).ToArray();
    }

    /// <summary>
    /// This is the main workhorse, it creates new keys and compares them to the password until the password
    /// is matched or all keys of the current key length have been checked
    /// </summary>
    /// <param name="currentCharPosition">The position of the char which is replaced by new characters currently</param>
    /// <param name="keyChars">The current key represented as char array</param>
    /// <param name="keyLength">The length of the key</param>
    /// <param name="indexOfLastChar">The index of the last character of the key</param>
    private void createNewKey(int currentCharPosition, char[] keyChars, int keyLength, int indexOfLastChar, CancellationToken cancellationToken)
    {
        var nextCharPosition = currentCharPosition + 1;
        // We are looping trough the full length of our charactersToTest array
        for (int i = 0; i < _charactersToTestLength; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
            /* The character at the currentCharPosition will be replaced by a
             * new character from the charactersToTest array => a new key combination will be created */
            keyChars[currentCharPosition] = charactersToTest[i];

            // метод буде викликати сам себе доки не перебере всі можливі комбінації або не знайде пароль
            if (currentCharPosition < indexOfLastChar && !_isMatched)
            {
                createNewKey(nextCharPosition, keyChars, keyLength, indexOfLastChar, cancellationToken);
            }
            else
            {
                // A new key has been created, remove this counter to improve performance
                //Interlocked.Increment(ref _computedKeys);
                //Interlocked.Increment(ref _computedKeys);
                if (!_isStatusDisplayed)
                {
                    Interlocked.Increment(ref _computedKeys);
                }


                /* The char array will be converted to a string and compared to the password. If the password
                 * is matched the loop breaks and the password is stored as result. */

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
            double totalTimeInSeconds = (Status.TotalCombinations /*- Interlocked.Read(ref _computedKeys)*/) / (Status.Speed * (1 + (10 / 100)));
            TimeSpan totalTime = TimeSpan.FromSeconds(totalTimeInSeconds);
            int days = totalTime.Days;
            int hours = totalTime.Hours;
            int minutes = totalTime.Minutes;
            int seconds = totalTime.Seconds;
            int ms = totalTime.Milliseconds;
            Status.TimeEstimated = $"{days}d, {hours}h, {minutes}m, {seconds}s, {ms}ms";
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
