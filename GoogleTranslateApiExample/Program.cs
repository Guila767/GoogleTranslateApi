using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleTranslateApiExample
{
    class Program
    {
        static void Main(string[] args) => new Example(args).StartAsync().GetAwaiter().GetResult();
    }

    class Example
    {
        private readonly GoogleTranslateApi.Language source = GoogleTranslateApi.Language.English;
        private readonly GoogleTranslateApi.Language target = GoogleTranslateApi.Language.Portuguese;
        private readonly GoogleTranslateApi.GoogleTranslator translate;

        public Example() : this(Array.Empty<string>()) { }
        
        public Example(string[] args) 
        {
            if (args.Length == 2)
            {
                source = GoogleTranslateApi.Language.Parse(args[0]);
                target = GoogleTranslateApi.Language.Parse(args[1]);
            }

            this.translate = new GoogleTranslateApi.GoogleTranslator(
                source,
                target
            );
        }

        public async Task StartAsync()
        {
            string text;
            Console.WriteLine("Type '!exit' to close the test sample...\n");
            while((text = Console.ReadLine()) != "!exit")
            {
                Console.WriteLine($"> Translation from {source.GetLanguageFullName()} to {target.GetLanguageFullName()}:");
                Console.Out.WriteLine(await this.translate.GetTextAsync(text));
            }
        }
    }
    //static class TextWriterEx
    //{
    //    public static async Task OnTextReadAsync(this TextReader textReader, Action<string> action, CancellationToken cancellationToken)
    //    {
    //        while (!cancellationToken.IsCancellationRequested)
    //        {
    //            var r = await textReader.ReadLineAsync();
    //            action.Invoke(r);
    //        }
    //    }

    //    public static async Task OnTextReadAsync(this TextReader textReader, Action<string> action) => await OnTextReadAsync(textReader, action, CancellationToken.None);
    //}
}
