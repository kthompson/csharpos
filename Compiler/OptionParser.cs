using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Compiler
{
    public abstract class OptionParserException : Exception
    {
        public Option Option { get; private set; }

        protected OptionParserException(Option option)
            : this(option, string.Empty)
        {
        }

        protected OptionParserException(Option option, string message)
            : base(message)
        {
            this.Option = option;
        }
    }

    public class MissingOptionException : OptionParserException
    {
        public MissingOptionException(Option option)
            : base(option, string.Empty)
        {

        }

        public MissingOptionException(Option option, string message)
            : base(option, message)
        {

        }
    }

    public class MissingOptionParameterException : OptionParserException
    {
        public MissingOptionParameterException(Option option)
            : base(option, string.Empty)
        {
        }

        public MissingOptionParameterException(Option option, string message)
            : base(option, message)
        {
            
        }
    }

    public class Option
    {
        public string ShortForm { get; set; }
        public string LongForm { get; set; }
        public bool Required { get; set; }
        public bool Found { get; set; }
        public string Description { get; set; }

        private Action _action;
        public Action Action
        {
            get { return _action; }
            set
            {
                if (_actionWithParam != null)
                    throw new InvalidOperationException("You cannot set Action and ActionWithParam. Use only one.");
                _action = value;
            }
        }

        private Action<string> _actionWithParam;
        public Action<string> ActionWithParam {
            get { return _actionWithParam; }
            set
            {
                if (_action != null)
                    throw new InvalidOperationException("You cannot set Action and ActionWithParam. Use only one.");
                _actionWithParam = value;
            }
        }

        public bool RequiresParam
        {
            get
            {
                return this.ActionWithParam != null;
            }
        }
    }

    

    public class OptionParser : IEnumerable<Option>
    {
        private readonly List<Option> _list = new List<Option>();

        public string GetUsage()
        {
            using(var sw = new StringWriter())
            {
                foreach (var option in this)
                {
                    var sbRow = new StringBuilder();
                    sbRow.Append('\t');
                    var hasShort = !string.IsNullOrEmpty(option.ShortForm);
                    var hasLong = !string.IsNullOrEmpty(option.LongForm);
                    if (hasShort)
                        sbRow.AppendFormat("-{0}", option.ShortForm);

                    if (hasLong && hasShort)
                        sbRow.Append(", ");

                    if (hasLong)
                    {
                        if (option.RequiresParam)
                            sbRow.AppendFormat("--{0} <{1}>", option.LongForm, option.LongForm.ToUpper());
                        else
                            sbRow.AppendFormat("--{0}", option.LongForm);
                    }

                    if(sbRow.Length >= 26)
                    {
                        //overflowed to the next line
                        sw.WriteLine(sbRow.ToString());
                        sw.Write("".PadRight(26));
                    }
                    else
                    {
                        sw.Write(sbRow.ToString().PadRight(26));
                    }
                    sw.WriteLine(option.Description);
                }
                return sw.ToString();
            }
        }

        public string[] Parse(string[] args)
        {
            var stack = new Stack<string>(args.Reverse());
            while (stack.Count > 0)
            {
                var option = stack.Pop();
            
                //the rest should be passed back.
                if (option == "--")
                    break;

                //not an option so push it back onto the stack and give up
                if ((option[0] != '/') && (option[0] != '-'))
                {
                    stack.Push(option);
                    break;
                }

                //remove extra dash for long form
                option = option.Substring(1);
                if (option[0] == '-')
                    option = option.Substring(1);

                //if we have an equals sign, break it up
                if (option.Contains('='))
                {
                    var values = option.Split(new[] {'='}, 2);
                    option = values[0];
                    stack.Push(values[1]);
                }

                var match = this.Where(o => o.LongForm == option || o.ShortForm == option).FirstOrDefault();
                if (match == null)
                {
                    //cant execute the option so put it back.
                    stack.Push(option);
                    break;
                }

                //we are going to execute it
                if (match.RequiresParam)
                {
                    //cant execute the option because we dont have a param supplied
                    if (stack.Count == 0)
                        throw new MissingOptionParameterException(match);

                    match.ActionWithParam(stack.Pop());
                }
                else
                {
                    match.Action();
                }

                match.Found = true;
            }

            var notFound = this.Where(o => o.Required && !o.Found).FirstOrDefault();
            if(notFound != null)
                throw new MissingOptionException(notFound);

            return stack.Reverse().ToArray();
        }

        public void Add(Option option)
        {
            this._list.Add(option);
        }


        #region IEnumerable<Option> Members

        public IEnumerator<Option> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
