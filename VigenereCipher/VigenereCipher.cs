using System;
using System.Collections.Generic;
using System.Text;

namespace VigenereCipher;

public static class VigenereCipher
{
    public static string Encrypt(string text, string key, string alphabet)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(alphabet))
            return text ?? string.Empty;

        var lowerAlphabet = alphabet.ToLower();
        var lowerKey = key.ToLower();

        Dictionary<char, int> alphabetIndex = lowerAlphabet
            .Select((ch, i) => new { ch, i })
            .ToDictionary(x => x.ch, x => x.i);

        int validCharCount = 0;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char textChar = text[i];
            bool isUpper = char.IsUpper(textChar);
            textChar = isUpper ? char.ToLower(textChar) : textChar;
            if (alphabetIndex.TryGetValue(textChar, out int textIndex))
            {
                int currentKeyIndex = validCharCount % lowerKey.Length;
                validCharCount++;
                char keyChar = lowerKey[currentKeyIndex];
                int keyCharIndex = alphabetIndex.TryGetValue(keyChar, out int kIdx) ? kIdx : 0;
                int encryptedCharIndex = (textIndex + keyCharIndex) % lowerAlphabet.Length;
                char encryptedChar = lowerAlphabet[encryptedCharIndex];
                sb.Append(isUpper ? char.ToUpper(encryptedChar) : encryptedChar);
            } 
            else
            {
                sb.Append(textChar);
            }
        }
        return sb.ToString();
    }

    public static string Decrypt(string text, string key, string alphabet)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(alphabet))
            return text ?? string.Empty;
        var lowerAlphabet = alphabet.ToLower();
        var lowerKey = key.ToLower();
        Dictionary<char, int> alphabetIndex = lowerAlphabet
            .Select((ch, i) => new { ch, i })
            .ToDictionary(x => x.ch, x => x.i);
        int validCharCount = 0;
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char textChar = text[i];
            bool isUpper = char.IsUpper(textChar);
            textChar = isUpper ? char.ToLower(textChar) : textChar;
            if (alphabetIndex.TryGetValue(textChar, out int textIndex))
            {
                int currentKeyIndex = validCharCount % lowerKey.Length;
                validCharCount++;
                char keyChar = lowerKey[currentKeyIndex];
                int keyCharIndex = alphabetIndex.TryGetValue(keyChar, out int kIdx) ? kIdx : 0;
                int decryptedCharIndex = (textIndex - keyCharIndex + lowerAlphabet.Length) % lowerAlphabet.Length;
                char decryptedChar = lowerAlphabet[decryptedCharIndex];
                sb.Append(isUpper ? char.ToUpper(decryptedChar) : decryptedChar);
            }
            else
            {
                sb.Append(textChar);
            }
        }
        return sb.ToString();

    }

    public static string FindKeyWithKnownPlaintext(string cipherText, string knownPlaintext, string alphabet)
    {
        string loopedKeyFragment = TryFindLoopedKeyFragment(cipherText, knownPlaintext, alphabet);
        return TryFindKeyFromLoopedFragment(loopedKeyFragment);
    }

    private static string TryFindKeyFromLoopedFragment(string loopedKeyFragment)
    {
        if (string.IsNullOrEmpty(loopedKeyFragment)) return string.Empty;

        for (int length = 1; length <= loopedKeyFragment.Length; length++)
        {
            string candidate = loopedKeyFragment.Substring(0, length);
            bool isValidKey = true;

            for (int i = 0; i < loopedKeyFragment.Length; i++)
            {
                if (loopedKeyFragment[i] != candidate[i % length])
                {
                    isValidKey = false;
                    break;
                }
            }

            if (isValidKey)
            {
                return candidate;
            }
        }

        return loopedKeyFragment;
    }

    private static string TryFindLoopedKeyFragment(string cipherText, string knownPlaintext, string alphabet)
    {
        var lowerAlphabet = alphabet.ToLower();
        var alphabetIndex = lowerAlphabet
            .Select((ch, i) => (ch, i))
            .ToDictionary(x => x.ch, x => x.i);

        StringBuilder key = new StringBuilder();
        int cipherIndex = 0;

        foreach (char pChar in knownPlaintext)
        {
            char lowerP = char.ToLower(pChar);
            if (!alphabetIndex.ContainsKey(lowerP)) continue;

            while (cipherIndex < cipherText.Length && !alphabetIndex.ContainsKey(char.ToLower(cipherText[cipherIndex])))
            {
                cipherIndex++;
            }

            if (cipherIndex >= cipherText.Length) break;

            int cIdx = alphabetIndex[char.ToLower(cipherText[cipherIndex])];
            int pIdx = alphabetIndex[lowerP];

            int kIdx = (cIdx - pIdx + lowerAlphabet.Length) % lowerAlphabet.Length;
            key.Append(lowerAlphabet[kIdx]);

            cipherIndex++;
        }

        return key.ToString();
    }
    public static string[] BuildCirculantTable(string alphabet)
    {
        var lowerAlphabet = alphabet.ToLower();
        var table = new string[lowerAlphabet.Length];
        for (int i = 0; i < lowerAlphabet.Length; i++)
        {
            table[i] = lowerAlphabet.Substring(i) + lowerAlphabet.Substring(0, i);
        }
        return table;
    }
}
