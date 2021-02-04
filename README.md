# GoogleTranslateApi
Free Api for Google Translate

## Usage:

``` C#
// Namespace: GoogleTranslateApi
// Constructor Parameters: GoogleTranslator(SourceLanguage, TargetLanguage);
GoogleTranslator translate = new GoogleTranslator(Language.Portuguese, Language.English);

// This Function Will return the Translated Text
string TranslatedText = translate.Text("Olá Mundo");

// In a assynchronous context
string TranslatedText = await translate.GetTextAsync("Olá Mundo");
```
