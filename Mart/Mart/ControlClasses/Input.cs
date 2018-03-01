using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mart
{
    public class Input
    {
        public static void InputString(TextBox Textbox, KeyPressEventArgs e)
        {
            if (Textbox.Text == "" && e.KeyChar == '.') e.Handled = true;
            if (Textbox.Text == "0" && e.KeyChar != '.') Textbox.Text = "";
            if (Textbox.Text.IndexOf('.') > 0 && e.KeyChar == '.') e.Handled = true;
            if (char.IsDigit(e.KeyChar) == true && e.KeyChar != '.' && e.KeyChar != '\b') e.Handled = true;
        }

        public static void InputNmber(TextBox Textbox, KeyPressEventArgs e)
        {
            if (Textbox.Text == "" && e.KeyChar == '.') e.Handled = true;
            if (Textbox.Text == "0" && e.KeyChar != '.') Textbox.Text = "";
            if (Textbox.Text.IndexOf('.') > 0 && e.KeyChar == '.') e.Handled = true;
            if (char.IsDigit(e.KeyChar) == false && e.KeyChar != '.' && e.KeyChar != '\b') e.Handled = true;
        }

    }
}
