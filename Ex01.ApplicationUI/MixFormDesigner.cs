using System.Drawing;
using System.Windows.Forms;

namespace Ex01.ApplicationUI
{
    internal class MixFormDesigner : FormDesigner
    {
        //public Form FormMain { get; set; }

        public override void design()
        {
            defualt();
            foreach (Control control in Form.Controls)
            {
                paintControl(control, Color.Yellow);
                heightControl(control, 40);
                
            }
        }
    }
}