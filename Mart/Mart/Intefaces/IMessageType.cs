using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mart.Intefaces
{
    interface IMessageType
    {
        void MessageSuccess(string des, string title);
        void MessageError(string des, string title);
        void MessageWarning(string des, string title);
        DialogResult MessageVerify(string des, string title);
    }
}
