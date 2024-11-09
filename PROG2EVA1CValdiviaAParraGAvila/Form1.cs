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

        //Arreglo con las palabras a buscar
        private string[] palabras = { "PROGRAMA", "CODIGO", "HERENCIA", "VARIABLE", "BUCLE", "ARREGLO", "CLASE", "METODO" };


        //Lista con los caracteres que se seleccionaran con el mouse
        private List<char> letrasSeleccionadas = new List<char>();

        //String en el que se ira concatenando la letra seleccionada por el usuario
        //para luego ver si coincide con alguna palabra del arreglo
        private string busqueda = "";

        //Lista en la que se van a ir agregando las palabras encontradas (Especial para el metodo para que no se repita la seleccion)
        private List<string> palabrasTachadas = new List<string>();


        //String en el que se ira concatenando las palabras ya encontradas
        private string palabrasYaEncontradas = "";
        private int contadorDePalabrasEncontradas = 0;

        //Variable Bool para indicar que ya se hizo click una sola vez en el label
        private bool clickHecho = false;


        public Form1()
        {
            InitializeComponent();
        }


        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            LimpiezaMatrizEntera();
            Limpieza();
            LimpiezaPalabrasEncontradas();
            LimpiezaClicks();


            //Paso 1, colocar las palabras del Arreglo
            colocarPalabrasDeLaLista();

            //Paso 2, colocar letras aleatorias en espacios vacios de la matriz
            //Aqui de igual forma se pasara por cada celda por lo que aprovechamos de agregarle
            //un capturador de click en el label para poder seleccionar las letras
            rellenarConLetrasAleatorias();


        }






        //PASO 1

        private void colocarPalabrasDeLaLista()
        {

            //Por cada palabra en la lista de palabras iremos validando si se puede
            //colocar vertical o horizontalmente
            foreach (string palabra in palabras)
            {
                bool colocada = false;
                int cantidadDeIntentos = 0;



                //Mientras no este la palabra colocada y no se agoten los intentos
                //Se intentara poner la palabra en la matriz
                while (!colocada && cantidadDeIntentos < 100)
                {

                    //Especificamos una fila y columna aleatoria donde poner la palabra
                    //al igual que una orientacion random para la palabra
                    int fila = random.Next(tbLayout.RowCount);
                    int columna = random.Next(tbLayout.ColumnCount);
                    int direccion = random.Next(2); // 0: horizontal, 1: vertical


                    //Ocupamos el metodo que nos permite ver si la palabra cabe horizontal o
                    //Verticalmente, si es asi, agregamos la palabra a los labels del tableLayoutPanel
                    if (PuedeColocarPalabra(palabra, fila, columna, direccion))
                    {

                        tbLayout.SuspendLayout();

                        recorrerMatriz(palabra, fila, columna, direccion);

                        colocada = true;
                    }
                    cantidadDeIntentos++;


                }



            }
        }


        //Esta funcion va asignando cada letra individual siguiendo celda por celda en la fila o columna
        private void recorrerMatriz(string palabra, int fila, int columna, int direccion)
        {
            //For para extraer cada caracter de la palabra e ir moviendose a la siguiente celda para asignarlo al label
            for (int i = 0; i < palabra.Length; i++)
            {
                Control control = tbLayout.GetControlFromPosition(columna, fila);

                if (control is Label label)
                {
                    //Primero la vaciamos para asegurarnos  NO BORRAR!
                    label.Text = "";


                    //asignamos la letra de la palabra al Label
                    label.Text = palabra[i].ToString();


                    //nos movemos a la siguiente celda en la dirección seleccionada
                    if (direccion == 0) // Horizontal
                    {
                        columna++; //aumentamos la columna
                    }
                    else if (direccion == 1) // Vertical
                    {
                        fila++; //aumentamos la fila
                    }


                }
            }
        }


        //Metodo para calcular si la palabra cabe horizontal o verticalmente
        private bool PuedeColocarPalabra(string palabra, int fila, int columna, int direccion)
        {
            int longitud = palabra.Length;

            if (direccion == 0) //Horizontal
            {
                //Verificamos que la longitud de la palabra junto al numero de todas las columnas quepa dentro del
                //Tablelayout
                if (columna + longitud > tbLayout.ColumnCount)
                {
                    return false;
                }

                //Con este for encontramos los labels dentro de la posicion de la columna y fila con el fin de verificar
                //que se encuentren vacios (PARA QUE LAS PALABRAS NO SE SOLAPEN UNAS CON OTRAS!)
                for (int col = columna; col < columna + longitud; col++)
                {
                    Control control = tbLayout.GetControlFromPosition(col, fila);
                    if (control is Label label && label.Text != "   ")
                    {
                        return false; //Devolvemos Falso si ya hay algun caracter
                    }
                }
                return true; //Si no cayo en ninguna validacion devolvemos verdadero
            }
            else if (direccion == 1) //Vertical
            {
                //Verificamos que la palabra quepa en el tablero
                if (fila + longitud > tbLayout.RowCount)
                {
                    return false;
                }

                //Con este for encontramos los labels dentro de la posicion de la columna y fila con el fin de verificar
                //que se encuentren vacios (PARA QUE LAS PALABRAS NO SE SOLAPEN UNAS CON OTRAS!)
                for (int f = fila; f < fila + longitud; f++)
                {
                    Control control = tbLayout.GetControlFromPosition(columna, f);
                    if (control is Label label && label.Text != "   ")
                    {
                        return false;  //Devolvemos Falso si ya hay algun caracter
                    }
                }
                return true; //Si no cayo en ninguna validacion devolvemos verdadero
            }

            return false; //Falso por si no se pudo poner la palabra


        }



        //PASO 2


        private void rellenarConLetrasAleatorias()
        {


            tbLayout.SuspendLayout();

            for (int columna = 0; columna < tbLayout.ColumnCount; columna++)
            {
                for (int fila = 0; fila < tbLayout.RowCount; fila++)
                {
                    //Buscamos los labels en cada posicion del tableLayoutPanel
                    Control control = tbLayout.GetControlFromPosition(columna, fila);
                    if (control is Label label)
                    {

                        if (label.Text == "   ")
                        {
                            //Agregamos la letra aleatoria en la posicion del momento
                            label.Text = obtenerLetra().ToString();

                        }


                        //Agregamos un capturador de click para el label vacio o no vacio
                        //El cual nos permitira seleccionar las letras
                        //y guardarlas para compararlas con las palabras de la lista
                        label.Click += label1_Click;



                    }
                }

            }
        }

        private char obtenerLetra()
        {

            //desde la posicion base de 'A' -> 65 en ASCII 
            //hasta la posicion 'Z' -> 90 (91 para incluir la 'Z')
            return (char)random.Next(65, 91);
        }





        //Metodos para seleccionar las letras
        private void label1_Click(object sender, EventArgs e)
        {

            //Capturamos el click del label que previamente asignamos al llenar la matriz
            //De igual forma verificamos si viene del mouse (con el fin de identificar si es click izquierdo o derecho)
            if (sender is Label label && e is MouseEventArgs mouseEvent)
            {


                //Si se presiono click izquierdo haremos el procedimiento de buscar la palabra
                if (mouseEvent.Button == MouseButtons.Left)
                {

                    Console.WriteLine("Label: " + label.Text);


                    //Cambiamos el color posterior del label al hacerle click
                    label.BackColor = Color.DarkSeaGreen;

                    //Con este metodo iremos concatenando la letra seleccionada a un string
                    palabraEstaEnArreglo(label.Text);

                }

                //Si se presiono click derecho deseleccionamos el texto
                if(mouseEvent.Button == MouseButtons.Right)
                {

                    Limpieza();

                }

            }
        }

        //Metodo que va concatenando las letras que se van seleccionando con el mouse en un string
        //y a su vez vamos verificando si ese string que se va armando a medida que el usuario va seleccionando letras
        //coincide con alguno en el arreglo de palabras
        private void palabraEstaEnArreglo(string labelLetra)
        {

            //Concatenamos la letra seleccionada con el mouse en el string
            busqueda += labelLetra;

            Console.WriteLine("Concatenacion? " + busqueda);


            //Recorremos el arreglo de palabras
            for (int i = 0; i < palabras.Length; i++)
            {

                string palabraDelArreglo = palabras[i];


                //Si las letras que se concatenaron en el string para formar una palabra, coincide con alguna palabra del arreglo,
                //Y si de igual forma la palabra no se encuentra ya en una lista que va almacenando las palabras encontradas
                //entonces mostramos la palabra encontrada en un label al usuario
                if (busqueda == palabraDelArreglo && !palabraEsRepetida(palabraDelArreglo))
                {

                    //Agregamos la palabra encontrada en una lista (Esto con el fin de que no se pueda volver a seleccionar)
                    palabrasTachadas.Add(palabraDelArreglo);


                    //Concatenamos la palabra encontrada en el string destinado a aquello 
                    palabrasYaEncontradas = palabrasYaEncontradas + "- " + palabras[i].ToString() + "\n";

                    //Aumentamos el contador de palabras ya encontradas
                    contadorDePalabrasEncontradas++;

                    //Mostramos en un label la lista de palabras encontradas al usuario
                    palabrasEncontradas.Text = palabrasYaEncontradas;

                    //Deseleccionamos lo previamente seleccionado una vez encontrada la palabra
                    LimpiezaColorSeleccionadoLabel();

                    //Aprovechamos de verificar si ya se encontraron todas las palabras
                    JuegoCompletado();

                    //Limpiamos busqueda para poder encontrar una nueva palabra
                    busqueda = "";




                }

            }
        }


        //Metodo para verificar si la palabra que se encontro al concatenar las letras no esta ya en la lista de palabras encontradas
        private bool palabraEsRepetida(string palabra)
        {
            //Recorremos la lista para verificar si la palabra tachada esta repetida
            for(int i = 0;i<palabrasTachadas.Count; i++)
            {

                //Si la palabra de la lista es igual a la palabra que se paso de parametro devolvemos que
                //Esta repetida
                if (palabrasTachadas[i] == palabra)
                {
                    //Mostramos una alerta y tambien limpiamos la seleccion de las letras
                    MessageBox.Show("La palabra ya fue encontrada!");
                    Limpieza();

                    return true;
                }

            }

            return false;


        }



        //Metodo para verificar si ya se encontraron las palabras
        private void JuegoCompletado()
        {

            Console.WriteLine("Contador : " + contadorDePalabrasEncontradas);
            Console.WriteLine("Largo : " + palabras.Length);


            //Si el largo del arreglo coincide con el contador que aumentamos cada que encontramos una palabra
            //mandamos un mensaje
            if (contadorDePalabrasEncontradas == palabras.Length)
            {
                MessageBox.Show("Has encontrado todas las palabras!");
            }


        }


        //METODOS DE LIMPIEZA

        //Metodo para limpiar el string de busqueda y reestablecer el color de los labels
        private void Limpieza()
        {

            //Limpiamos la busqueda
            busqueda = "";


            //Deseleccionamos todos los labels (cambiamos su color rojo)
            tbLayout.SuspendLayout();

            for (int columna = 0; columna < tbLayout.ColumnCount; columna++)
            {
                for (int fila = 0; fila < tbLayout.RowCount; fila++)
                {
                    Control control = tbLayout.GetControlFromPosition(columna, fila);
                    if (control is Label label)
                    {
                        label.BackColor = Color.Transparent;

                    }
                }

            }





        }


        //Metodo para limpiar el color que indica que la letra esta seleccionado
        private void LimpiezaColorSeleccionadoLabel()
        {
            for (int columna = 0; columna < tbLayout.ColumnCount; columna++)
            {
                for (int fila = 0; fila < tbLayout.RowCount; fila++)
                {
                    Control control = tbLayout.GetControlFromPosition(columna, fila);
                    if (control is Label label)
                    {
                        label.BackColor = Color.Transparent;

                    }
                }

            }
        }

        //Metodo para limpiar los capturadores de click en los labels (Al apretar randomizar sopa se van aumentando y la concatenacion no funciona)
        private void LimpiezaClicks()
        {
            for (int columna = 0; columna < tbLayout.ColumnCount; columna++)
            {
                for (int fila = 0; fila < tbLayout.RowCount; fila++)
                {
                    Control control = tbLayout.GetControlFromPosition(columna, fila);
                    if (control is Label label)
                    {
                        label.Click -= label1_Click;

                    }
                }

            }

        }

        //Metodo para limpiar los strings que almacenan las palabras encontradas
        private void LimpiezaPalabrasEncontradas()
        {
            palabrasTachadas.Clear();
            palabrasYaEncontradas = "";
            palabrasEncontradas.Text = "- ";
        }

        //Metodo que limpia cada label y le asigna TRES ESPACIOS (Si no son tres espacios no funciona!)
        private void LimpiezaMatrizEntera()
        {

            for (int columna = 0; columna < tbLayout.ColumnCount; columna++)
            {
                for (int fila = 0; fila < tbLayout.RowCount; fila++)
                {
                    Control control = tbLayout.GetControlFromPosition(columna, fila);
                    if (control is Label label)
                    {
                        label.Text = "   ";

                    }
                }

            }


        }





        //Boton en el Form para deseleccionar
        private void button2_Click(object sender, EventArgs e)
        {
            Limpieza();

        }
    }
}
