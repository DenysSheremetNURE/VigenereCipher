using VigenereCipher;

Console.OutputEncoding = System.Text.Encoding.UTF8;

string alphabet = "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя";
//string alphabetEnglish = "abcdefghijklmnopqrstuvwxyz";

string initialText = "Бмлдяц бьй южьаебчез жсвт ивсцд птлдцшїиз Лле Дтцць 1984 лшга. Нсї\r\nщзрбамяяї, іш чфур ф'зьрґигз ішьххїеюіє нрзшзргяйїїїляд зг їцлзьяяху зхрґ\r\nцц'ч иуш бшуяєїд иаьшюд Иисир, ґш см жшфішїяха і мфґирчо щзшжмааза\r\nлїнмеяхбефтгцт щачє-кфййє юсчма. Чшнйду миг самс Длцеьт плхддтхлюч\r\nхядс флиирншц фзрбяййзиестчєж ошишфшьшіфєж, лхб ї 2000 зшжс,\r\nґифлкфд шччцу ьсчшюсж їзифхдїєюґс ь мґсбядґесш флржяайлийсч, самр\r\nїриишиз няехяях ї гріяк.";

string plainTextFragment = "Першим про спрощення";

//string decryptedText = VigenereCipher.VigenereCipher.Decrypt(initialText, "ключ", alphabet);

string key = VigenereCipher.VigenereCipher.FindKeyWithKnownPlaintext(initialText, plainTextFragment, alphabet);
string decryptedText = VigenereCipher.VigenereCipher.Decrypt(initialText, key, alphabet);

Console.WriteLine(key);
Console.WriteLine(decryptedText);