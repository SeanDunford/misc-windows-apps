using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EggScramble
{
    public class FormattedValue
    {
        string format;
        int val;
        public string Text;

        public int Value
        {
            get
            {
                return val;
            }
            set
            {
                val = value;
                Text = string.Format(format, val);
            }
        }

        public FormattedValue(string format)
        {
            this.format = format;
            Value = 0;
        }

    }
}
