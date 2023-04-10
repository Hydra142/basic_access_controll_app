using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SafeMessenge.Helpers;

public class BruteForcePasswordCracker
{
    // the secret password which we will try to find via brute force
    private static string password = "qw";
    private static string result;

    private static bool isMatched = false;

    /* The length of the charactersToTest Array is stored in a
     * additional variable to increase performance  */
    private static int charactersToTestLength = 0;
    private static long computedKeys = 0;

    /* An array containing the characters which will be used to create the brute force keys,
     * if less characters are used (e.g. only lower case chars) the faster the password is matched  */
    private static char[] charactersToTest =
    {
        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z','A','B','C','D','E',
        'F','G','H','I','J','K','L','M','N','O','P','Q','R',
        'S','T','U','V','W','X','Y','Z','1','2','3','4','5',
        '6','7','8','9','0','!','$','#','@','-'
    };

    public async Task Start()
    {
        var timeStarted = DateTime.Now;
        System.Diagnostics.Debug.WriteLine("Start BruteForce - {0}", timeStarted.ToString());

        // The length of the array is stored permanently during runtime
        charactersToTestLength = charactersToTest.Length;

        // The length of the password is unknown, so we have to run trough the full search space
        var estimatedPasswordLength = 0;
        var tasks = new Task[3];
        
        for (int i = 0; i < tasks.Length; i++)
        {
            estimatedPasswordLength++;
            var threadName = $"pass len {estimatedPasswordLength} thread";
            tasks[i] = Task.Run(() => startBruteForce(estimatedPasswordLength));
            //tasks[i] = Task.Run( () =>
            //{
            //    Thread.CurrentThread.Name = threadName;
            //    startBruteForce(5);
            //});
        }
        
            

        await Task.WhenAll(tasks);
        //while (!isMatched)
        //{
        //    /* The estimated length of the password will be increased and every possible key for this
        //     * key length will be created and compared against the password */
        //    estimatedPasswordLength++;
        //    startBruteForce(estimatedPasswordLength);
        //}
        System.Diagnostics.Debug.WriteLine($"Password matched. - {DateTime.Now}");
        System.Diagnostics.Debug.WriteLine($"Time passed: {DateTime.Now.Subtract(timeStarted).TotalSeconds}s");
        System.Diagnostics.Debug.WriteLine($"Resolved password: {result}");
        System.Diagnostics.Debug.WriteLine($"Computed keys: {computedKeys}");

    }


    /// <summary>
    /// Starts the recursive method which will create the keys via brute force
    /// </summary>
    /// <param name="keyLength">The length of the key</param>
    private async Task startBruteForce(int keyLength)
    {
        var keyChars = createCharArray(keyLength, charactersToTest[0]);
        // The index of the last character will be stored for slight perfomance improvement
        var indexOfLastChar = keyLength - 1;
        await Task.Run(() => createNewKey(0, keyChars, keyLength, indexOfLastChar));
        //createNewKey(0, keyChars, keyLength, indexOfLastChar);
        
        System.Diagnostics.Debug.WriteLine($"Thread finshed job with no results");
        
    }

    /// <summary>
    /// Creates a new char array of a specific length filled with the defaultChar
    /// </summary>
    /// <param name="length">The length of the array</param>
    /// <param name="defaultChar">The char with whom the array will be filled</param>
    /// <returns></returns>
    private static char[] createCharArray(int length, char defaultChar)
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
    private static void createNewKey(int currentCharPosition, char[] keyChars, int keyLength, int indexOfLastChar)
    {
        var nextCharPosition = currentCharPosition + 1;
        // We are looping trough the full length of our charactersToTest array
        for (int i = 0; i < charactersToTestLength; i++)
        {
            
            /* The character at the currentCharPosition will be replaced by a
             * new character from the charactersToTest array => a new key combination will be created */
            keyChars[currentCharPosition] = charactersToTest[i];
            System.Diagnostics.Debug.WriteLine($"Thread '{Thread.CurrentThread.ManagedThreadId}' check pass: '{new String(keyChars)}'");
            // метод буде викликати сам себе доки не перебере всі можливі комбінації або не знайде пароль
            if (currentCharPosition < indexOfLastChar && !isMatched)
            {
                createNewKey(nextCharPosition, keyChars, keyLength, indexOfLastChar);
            }
            else
            {
                // A new key has been created, remove this counter to improve performance
                computedKeys++;

                /* The char array will be converted to a string and compared to the password. If the password
                 * is matched the loop breaks and the password is stored as result. */
              
                if ((new String(keyChars)) == password)
                {
                    if (!isMatched)
                    {
                        System.Diagnostics.Debug.WriteLine($"res: {new String(keyChars)}");
                        isMatched = true;
                        result = new String(keyChars);
                    }
                    System.Diagnostics.Debug.WriteLine($"Thread '{Thread.CurrentThread.ManagedThreadId}' found password: {new String(keyChars)}");
                    return;
                }
            }
        }
    }
}

