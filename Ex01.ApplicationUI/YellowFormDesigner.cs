using System.Drawing;
using System.Windows.Forms;

namespace Ex01.ApplicationUI
{
    internal class YellowFormDesigner : FormDesigner
    {
        //public FormMain FormMain { get; set; }

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