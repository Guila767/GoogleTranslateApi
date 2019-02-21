using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.ComponentModel;

namespace GoogleTranslateApi
{
    public class Translate
    {
        private const string Url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=";
        private string Request;

        public class Language
        {
            private Language(string x) { Value = x; }

            public string Value { get; set; }
            public static Language Portuguese { get { return new Language("pt"); } }
            public static Language English { get { return new Language("en"); } }
            public static Language Spanish { get { return new Language("es"); } }
            public static Language Russian { get { return new Language("ru"); } }
            public static Language French { get { return new Language("fr"); } }
            public static Language German { get { return new Language("de"); } }
            public static Language Swedish { get { return new Language("sv"); } }
            public static Language Italian { get { return new Language("it"); } }
            public static Language Japanese { get { return new Language("ja"); } }
            public static Language Chinese { get { return new Language("zh"); } }
            //public static Language Auto { get { return new Language("auto"); } }
        }


        public Translate(Language source, Language target)
        {
            //if (target.Value == Language.Auto.Value)
            //    throw new Exception("The target language can't be Language.Auto");
            this.Request = Url + $"{source.Value}&tl={target.Value}&dt=t&q=";
        }

        private string Download(string text)
        {
            WebClient web = new WebClient();
            web.Encoding = Encoding.UTF8;
            //web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            //web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
            Uri uri = new Uri(this.Request + Uri.EscapeUriString(text));
            //web.DownloadFile(uri, "Tranlate.txt");
            return web.DownloadString(uri);
        }

        public string Text(string text)
        {
            text = Download(text.Replace('\x22', '\x27'));
            // Variables
            StringBuilder @string = new StringBuilder();
            int StartTxt, EndTxt;
            int BlocksStart, BlocksEnd;
            int BlocksNumbers = 0;
            // Set Variables
            StartTxt = text.IndexOf('\x5B'); EndTxt = text.LastIndexOf('\x5D');
            BlocksStart = text.IndexOf('\x5B', StartTxt + 1); BlocksEnd = text.LastIndexOf('\x5D', EndTxt - 1);
            for (int x = 0; x < text.Substring(BlocksStart, BlocksEnd - BlocksStart).Length; x++)
                if (text.Substring(BlocksStart + 1, BlocksEnd - BlocksStart)[x] == '\x5B')
                    BlocksNumbers++;
            //
            text = text.Substring(BlocksStart, BlocksEnd - BlocksStart);
            for (int x = 0; x < BlocksNumbers; x++)
            {
                int StartBlock = text.IndexOf('\x5B');
                int EndBlock = text.IndexOf('\x5D', StartBlock + 1);
                string sub = text.Substring(StartBlock, EndBlock - StartBlock);
                int Subindex = sub.IndexOf('\x22');
                sub = sub.Substring(Subindex + 1, sub.IndexOf('\x22', Subindex + 1) - (Subindex + 1));
                @string.Append(sub.Replace('\x27', '\x22'));
                text = text.Substring(EndBlock + 1);
            }
            //int index = text.IndexOf('\x22');
            //text = text.Substring(index+1, text.IndexOf('\x22', index+1) - index );
            return @string.ToString();
        }
    }
}
