using System.Drawing;
using System.Windows.Forms;

namespace Ex01.ApplicationUI
{
    internal class YellowFormDesigner : FormDesigner
    {
        public override void design()
        {
            defualt();
            foreach (Control control in Form.Controls)
            {
                paintControl(control, Color.Yellow);
            }
        }
    }
}