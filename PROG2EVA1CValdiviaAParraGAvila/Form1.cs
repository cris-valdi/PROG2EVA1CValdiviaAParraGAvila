using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1CValdiviaAParraGAvila
{
    public partial class Form1 : Form

        
    {

        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbLayout.SuspendLayout();

            for (int columna = 0; columna < tbLayout.ColumnCount; columna++)
            {
                for (int fila = 0; fila < tbLayout.RowCount; fila++)
                {
                    Control control = tbLayout.GetControlFromPosition(columna, fila);
                    if (control is Label label)
                    {
                        label.Text = obtenerLetra().ToString(); 
                    }
                }
                    
            }
        }

        private char obtenerLetra()
        {

            return (char)('A' + random.Next(0, 26));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
