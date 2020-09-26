using System.Drawing;
using System.Windows.Forms;

namespace Ex01.ApplicationUI
{
    public abstract class FormDesigner
    {
        public Form Form { get; set; }

        public abstract void design();

        protected void defualt()
        {
            foreach (Control control in Form.Controls)
            {
                paintControl(control, Color.SteelBlue);
                heightControl(control, 20);
            }
        }

        protected void paintControl(Control i_Control, Color i_Color)
        {
            bool notButton = !(i_Control is Button);

            if (notButton)
            {
                i_Control.BackColor = i_Color;
            }

            foreach (Control control in i_Control.Controls)
            {
                paintControl(control, i_Color);
            }
        }

        protected void heightControl(Control i_Control, int i_height)
        {
            if (i_Control is Button)
            {
                i_Control.Height = i_height;
            }

            foreach (Control control in i_Control.Controls)
            {
                heightControl(control, i_height);
            }
        }
    }
}