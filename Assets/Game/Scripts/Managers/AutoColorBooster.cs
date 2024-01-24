using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class AutoColorBooster : MonoBehaviour
{
    public static AutoColorBooster Instance;
    static Color defaultColor = Color.white;

    public Dictionary<char, Color> colorStates = new Dictionary<char, Color>
    {
        { 'A', defaultColor }, { 'B', defaultColor }, { 'C', defaultColor }, { 'D', defaultColor },
        { 'E', defaultColor }, { 'F', defaultColor }, { 'G', defaultColor }, { 'H', defaultColor },
        { 'I', defaultColor }, { 'J', defaultColor }, { 'K', defaultColor }, { 'L', defaultColor },
        { 'M', defaultColor }, { 'N', defaultColor }, { 'O', defaultColor }, { 'P', defaultColor },
        { 'Q', defaultColor }, { 'R', defaultColor }, { 'S', defaultColor }, { 'T', defaultColor },
        { 'U', defaultColor }, { 'V', defaultColor }, { 'W', defaultColor }, { 'X', defaultColor },
        { 'Y', defaultColor }, { 'Z', defaultColor },
     
    };
    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// Automatically color letters green given a single word and response to evaluate with.
    /// </summary>
    /// <param name="word">The word to evaluate the letter colors with.</param>
    /// <param name="response">The number of letters in "word" that are present in the hidden game word.</param>
    /// <returns>Whether or not a color update has occurred.</returns>
    bool AutoColorGreenSingle(string word, int response)
    {
        bool updated = false;
      //  int count = word.Count(c => colorStates[c] != Color.red);
        int count = word.Count(c => colorStates[c] != WordManager.Instance.concealedColor);

        // If the number of non-excluded letters equals the response then all non-excluded
        // letters must be included.
        if (count == response)
        {
            foreach (char c in word)
            {
                if (colorStates[c] == defaultColor)
                {
                  //  colorStates[c] = Color.green;
                    colorStates[c] = WordManager.Instance.revealedColor;
                    updated = true;
                }
            }
        }

        return updated;
    }

    /// <summary>
    /// Automatically color letters green based on the change in response between 2 words
    /// with nearly identical letters.
    ///
    /// For example:
    ///     P I N E  1
    ///     L I N E  2
    ///     
    /// The response increased to 2 with LINE from 1 with PINE. Since these words differ by
    /// only a single letter and the response changed by only 1, the letter in the higher
    /// response word must be included and the letter in the lower response word must be
    /// excluded. Therefore we would include L and exclude P.
    /// </summary>
    /// <param name="word1">The first word to evaluate the letter colors with.</param>
    /// <param name="response1">The number of letters in "word1" that are present in the hidden game word.</param>
    /// <param name="word2">The second word to evaluate the letter colors with.</param>
    /// <param name="response2">The number of letters in "word2" that are present in the hidden game word.</param>
    /// <returns>Whether or not a color update has occurred.</returns>
    bool AutoColorGreenPair(string word1, int response1, string word2, int response2)
    {
        bool updated = false;
        int n = word1.Length;
        int responseDiff = Math.Abs(response1 - response2);

        if (responseDiff == 0)
        {
            return updated;
        }

        // Identify all the letters which are different between the 2 words.
        var letters1 = word1.OrderBy(c => c).ToArray();
        var letters2 = word2.OrderBy(c => c).ToArray();

        var letters1Diff = new List<char>();
        var letters2Diff = new List<char>();

        int i = 0, j = 0;
        while (i < n && j < n)
        {
            if (letters1[i] < letters2[j])
            {
                letters1Diff.Add(letters1[i]);
                i++;
            }
            else if (letters1[i] > letters2[j])
            {
                letters2Diff.Add(letters2[j]);
                j++;
            }
            else
            {
                i++;
                j++;
            }
        }

        while (i < n)
        {
            letters1Diff.Add(letters1[i]);
            i++;
        }

        while (j < n)
        {
            letters2Diff.Add(letters2[j]);
            j++;
        }

        if (letters1Diff.Count != letters2Diff.Count)
        {
            throw new InvalidOperationException("This should not be possible");
        }

        // Check if the number of different letters is equal to the change in response between the 2 words. If they are 
        // equal then this means the set of different letters from the word with the lower response must be excluded and 
        // the different letters from the word with the higher response must be included.
        if (letters1Diff.Count == responseDiff)
        {
            // Include response 1 letters and exclude response 2 letters    
            if (response1 > response2)
            {
                foreach (char c in letters1Diff)
                {
                    if (colorStates[c] == defaultColor)
                    {
                        //colorStates[c] = Color.green;
                        colorStates[c] = WordManager.Instance.revealedColor;
                        print("Green Color");

                        updated = true;
                    }
                }

                foreach (char c in letters2Diff)
                {
                    if (colorStates[c] == defaultColor)
                    {
                        // colorStates[c] = Color.red;
                        colorStates[c] = WordManager.Instance.concealedColor;
                        updated = true;
                        print("Red Color");

                    }
                }
            }

            // Include response 2 letters and exclude response 1 letters
            else if (response2 > response1)
            {
                foreach (char c in letters1Diff)
                {
                    if (colorStates[c] == defaultColor)
                    {
                       // colorStates[c] = Color.red;
                        colorStates[c] = WordManager.Instance.concealedColor;
                        updated = true;
                        print("Red Color");

                    }
                }

                foreach (char c in letters2Diff)
                {
                    if (colorStates[c] == defaultColor)
                    {
                     //   colorStates[c] = Color.green;
                        colorStates[c] = WordManager.Instance.revealedColor;
                        updated = true;
                        print("Green Color");

                    }
                }
            }
        }

        return updated;
    }

    /// <summary>
    /// Automatically color letters red given a single word and response to evaluate with.
    /// </summary>
    /// <param name="word">The word to evaluate the letter colors with.</param>
    /// <param name="response">The number of letters in "word" that are present in the hidden game word.</param>
    /// <returns></returns>
    bool AutoColorRedSingle(string word, int response)
    {
        bool updated = false;
        int count = 0;
        Dictionary<char, int> letterCounts = new Dictionary<char, int>();

        foreach (char c in word)
        {
            if (colorStates[c] == WordManager.Instance.revealedColor)
            {
                count++;
            }

            if (letterCounts.ContainsKey(c))
            {
                letterCounts[c]++;
            }
            else
            {
                letterCounts[c] = 1;
            }
        }

        // If the number of included letters equals the response then all remaining letters
        // must be excluded.
        if (count == response)
        {
            foreach (char c in word)
            {
                if (colorStates[c] == defaultColor)
                {
                    print("Red Color");
                    //colorStates[c] = Color.red;
                    colorStates[c] = WordManager.Instance.concealedColor;
                    updated = true;
                }
            }
        }

        // If the count for a single letter in the word is greater than the response then
        // this letter cannot be part of the word and should be excluded.
        else
        {
            foreach (char c in word)
            {
                if (letterCounts[c] > response && colorStates[c] == defaultColor)
                {
                //    colorStates[c] = Color.red;
                    colorStates[c] = WordManager.Instance.concealedColor;
                    updated = true;
                    print("Red Color");

                }
            }
        }

        return updated;
    }

    /// <summary>
    /// Automatically updates the color state for letters given the history of guesses and their corresponding 
    /// responses.
    /// </summary>
    /// <param name="guesses">History of guesses and their corresponding responses.</param>
    public void AutoColor()
    {

        List<Tuple<string, int>> guesses = WordManager.Instance.guesses;

        // Check if any color updates can be done based on the new word alone
        Tuple<string, int> newGuess = guesses[guesses.Count - 1];    //guesses.Last();
        string newWord = newGuess.Item1;
        int newResponse = newGuess.Item2;
        bool updated = AutoColorRedSingle(newWord, newResponse) || AutoColorGreenSingle(newWord, newResponse);

        // Check if any letters can be included based on similarities between the new word
        // and previous words
        for (int i = 0; i < guesses.Count; i++)
        {
            var guess = guesses[i];
            updated |= AutoColorGreenPair(newWord, newResponse, guess.Item1, guess.Item2);
        }

        // If any color states have been updated then there may be new information that can
        // be deduced by revisiting word history. If anyone makes an update to a color we
        // need to keep revisiting the word history until no other updates have been made.
        while (updated)
        {
            updated = false;

            for (int i = 0; i < guesses.Count; i++)
            {
                var guess = guesses[i];
                string word = guess.Item1;
                int response = guess.Item2;
                updated = AutoColorRedSingle(word, response) || AutoColorGreenSingle(word, response);

                if (updated)
                {
                    break;
                }
            }
        }
        char character = 'A';
       // print("colorStates.Count : " + colorStates.Count);
        for(int  i=0; i < colorStates.Count;i++)
        {
            if (colorStates[character] != defaultColor)
            {
                UIManager.Instance.ChangeKeyColor(character.ToString(), colorStates[character]);
                KeyboardManager.Instance.ChangeKeyColor(character.ToString(), colorStates[character]);
            }//   print(character +" "+colorStates[character]);
            character++;
        }
    }
    public void OnClick_AutoColorButton()
    {

        if (BoosterManager.Instance.autocolorBoosterCount > 0)
        {
            BoosterManager.Instance.isAutoColor = true;
            BoosterManager.Instance.autocolorBoosterCount--;
            UIManager.Instance.ChangeAllKeyColorToDefault();
            KeyboardManager.Instance.ClearKeyColor();
            UIManager.Instance.UpdateAutoColorUI(BoosterManager.Instance.autocolorBoosterCount);
            AutoColor();
        }
        else
        {
            print("auto color Ads");
        }
    }





}
