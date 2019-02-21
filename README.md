# GoogleTranslateApi
Api for Google Translate

## Usage:

``` C#
// Constructor Parameters
// Translate(SourceLanguage, TargetLanguage);
Translate translate = new Translate(Translate.Language.Portuguese, Translate.Language.English);

// This Function Will return the Translated Text
string TranslatedText = translate.Text();
```
