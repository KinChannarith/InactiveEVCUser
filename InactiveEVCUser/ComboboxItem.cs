using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InactiveEVCUser
{
    public class ComboboxItem
    {
        public String Text { get; set; }
        public String Value { get; set; }

        public string SelectedText
        {
            get
            {
                return this.Text;
            }
        }
        public string SelectedValue
        {
            get
            {
                return this.Value.ToString();
            }
        }
        public override string ToString()
        {
            return Text;
        }
    }
}
