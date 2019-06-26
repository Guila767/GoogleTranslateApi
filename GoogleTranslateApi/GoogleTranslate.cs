using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.ComponentModel;
using System.IO;

namespace GoogleTranslateApi
{
    public class Translate
    {
        private const string Url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=";
        private string Request { get; set; } = String.Empty;

        private struct Block
        {
            public object[] Data { get; }
            public int Elements { get => Data.Length; }
            public int Blocks
            {
                get
                {
                    int c = 0;
                    foreach (object _data in Data)
                        if (_data is Block)
                            c++;
                    return c;
                }
            }
            public Block this[int x]
            {
                get
                {
                    if (x > Blocks)
                        throw new IndexOutOfRangeException("Index out of range");
                    List<Block> _blocks = new List<Block>();
                    foreach(object _data in Data)
                    {
                        if (_data is Block)
                        {
                            Block block = _data as Block? ?? throw new Exception("Cannot Parse the block at the given index");
                            _blocks.Add(block);
                        }
                    }
                    return _blocks[x];
                }
            }


            public Block(string data)
            {
                if (!IsValidData(data))
                    throw new ArgumentException("Invalid data string", "data");
                //int elements = data.Count(c => c == '[') + data.Count(c => c == ',')*2;
                //Data = new object[elements];
                List<object> ldata = new List<object>() { null };                
                Queue<char> vs = new Queue<char>(data.ToCharArray());

                //int datai = 0;
                while (vs.Count > 0)
                {
                    char @char = vs.Dequeue();
                    switch (@char)
                    {
                        case '[':
                            string nblock = new string(vs.ToArray());
                            int end = 0;
                            for (int n = nblock.Count(f => f == '[') + 1; n != 0; end++)
                                if (nblock[end] == ']')
                                    n--;
                            vs = new Queue<char>(nblock.Substring(end));
                            //ldata[datai] = new Block(nblock.Substring(0, nblock.LastIndexOf(']')));
                            ldata[ldata.Count - 1] = new Block(nblock.Substring(0, end - 1));
                            break;
                        case ',':
                            //datai++
                            ldata.Add(null);
                            continue;
                        case ']':
                            Data = ldata.ToArray();
                            return;
                        case '"':
                            do
                            {
                                @char = vs.Peek();
                                switch(@char)
                                {
                                    case '\\':
                                        @char = vs.Dequeue();
                                        break;
                                    case '"':
                                        continue;
                                    default:
                                        @char = vs.Dequeue();
                                        break;
                                }
                                //Data[datai] = String.Concat(Data[datai], @char);
                                ldata[ldata.Count - 1] = String.Concat(ldata[ldata.Count - 1], @char == '\\' ? vs.Dequeue() : @char);
                            }
                            while (vs.Peek() != '"');
                            vs.Dequeue();
                            break;
                        default:
                            //Data[datai] = String.Concat(Data[datai], @char);
                            ldata[ldata.Count - 1] = String.Concat(ldata[ldata.Count - 1], @char);
                            break;
                    }

                }

                Data = ldata.ToArray();
            }
         
            private static bool IsValidData(string data)
            {
                if (data == String.Empty)
                    return false;
                if ((data.Count(c => c == '[') + data.Count(c => c == ']')) % 2 != 0)
                    return false;
                if (data.Count(c => c == '"') % 2 != 0)
                    return false;
                return true;
            }
        }

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
            text = Download(text);
            Block Datablock = (new Block(text))[0][0][0];
            //string Source = Datablock.Data[1] == null ? string.Empty : Datablock.Data[1].ToString();
            string Dest = Datablock.Data[0] == null ? string.Empty : Datablock.Data[0].ToString();
            return Dest;
        }
    }
}
