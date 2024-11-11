using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROG2EVA1CValdiviaAParraGAvila
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string rut = textBox1.Text; //Guardamos el Rut ingresado por el usuario en un string

            rut = agregaCeros(rut);  //Llamamos a un metodo para agregar ceros al Rut en caso de que no tenga un largo de diez

            if (calculoRut(rut)) //Si el metodo para validar el rut nos entrega que el rut es correcto, abrimos el siguiente formulario
            {
                //Creamos un objeto de la clase en la que se encuentra el juego
                Form1 sopaDeLetras = new Form1();

                //Mostramos el proximo formulario
                sopaDeLetras.Show();

                //Cerramos el Formulario Actual
                this.Hide();

            }

        }

        private string agregaCeros(string rut)
        {
            while (rut.Length < 10)
            {

                rut = "0" + rut;

            }
            return rut;

        }

        private bool calculoRut(string rut)
        {
            double numeroRut = 0;
            double modulo = 2;
            double suma2 = 0;
            int digAntesDelGuion = rut.Length - 3;
            if (rut[8] == '-' && rut.Length < 11)
            {
                for (int i = digAntesDelGuion; i >= 0; i--)
                {
                    if (modulo == 8) { modulo = 2; }
                    numeroRut = double.Parse(rut[i].ToString());
                    suma2 = suma2 + numeroRut * modulo;
                    modulo++;
                }
                double division = suma2 / 11;
                int entero = (int)division;
                double resta = division - entero;
                double digito = (11 - (11 * (resta)));
                digito = Math.Round(digito);


                if (digitoVerificadorFix(digito.ToString()) == digitoVerificadorFix(rut[9].ToString()))
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Error en Digito Verificador, Intentar con " + digitoVerificadorFix(digito.ToString()));
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        private string digitoVerificadorFix(string rut)
        {
            if (rut == "11")
            {
                return "0";
            }
            else if (rut == "10")
            {
                return "K";
            }
            else if (rut == "k") //Por si el usuario ingresa una 'k' minuscula, validamos como si fuera mayuscula
            {
                return "K";
            }
            else
            {
                return rut;
            }
        }
    }
}
