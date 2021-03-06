﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;//se agrego la referencia de system.oracle.database
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Text.RegularExpressions;

namespace LoginUsuario
{
    //realizar cambios de colores en la entrega final

    public partial class Login : MaterialForm
    {
        string usuario;
        PanelPrincipal inicio;
        OracleConnection ora= new OracleConnection("DATA SOURCE=aura.cckolbo3d8nz.us-east-1.rds.amazonaws.com:1521/Auradb;USER ID=ADMAURA;PASSWORD= admin123");
       // OracleConnectionStringBuilder ora = new OracleConnectionStringBuilder("DATA SOURCE=aura.cckolbo3d8nz.us-east-1.rds.amazonaws.com:1521/Auradb;USER ID=ADMAURA PASSWORD= admin123;");
       //DATA SOURCE=aura.cckolbo3d8nz.us-east-1.rds.amazonaws.com:1521/Auradb;PERSIST SECURITY INFO=True;USER ID=ADMAURA
       //string de conexion con la base de datos /lo ponemos aqui para que sea usado de manera globl en el formulario

        //OracleConnection ora = new OracleConnection("DATA SOURCE=aura.cckolbo3d8nz.us-east-1.rds.amazonaws.com:1521/Auradb;USER ID=ADMAURA PASSWORD= admin123;");
        /**
        Data Sourse = es la dirección donde se encuentra nuestra base de datos con el / SID
            USER ID= el Usuario de Nuestra base de datos
            PASSWORD= es la contraseña de nuestra USUARIO en la BD
        **/
        public Login()
        {
            InitializeComponent();
            //Cambiar los valores de colores del material skin
            MaterialSkinManager m = MaterialSkinManager.Instance;
            m.AddFormToManage(this);
            m.Theme = MaterialSkinManager.Themes.LIGHT;
            m.ColorScheme = new ColorScheme(Primary.Blue600, Primary.Blue500, Primary.Blue400  , Accent.Green100, TextShade.WHITE);

        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //Lo metemos en un try catch para tomar las excepciones y no se caiga el programa
            try
            {
              
                if (txt_Usuario.Text.Equals(""))//si esta vacío el txt usuario 
                {

                    MessageBox.Show("Debe completar el campo usuario", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_Usuario.SelectAll();
                    txt_Usuario.Focus();

                }
                 if (txt_contrasena.Text.Equals(""))//si el txtcontrasena esta vacío
                {
                    MessageBox.Show("Debe completar el campo contraseña", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_contrasena.SelectAll();
                    txt_contrasena.Focus();
                }

                   if (txt_contrasena.Text.Equals("") && txt_Usuario.Text.Equals(""))//si ambos estan vacíos
                {
                    MessageBox.Show(" Debes completar todos los campos","Validación",MessageBoxButtons.OK,MessageBoxIcon.Exclamation );
                }
                else {
                    ora.Open();
                    //abrimos la conexión con la base de datos

                    //Realizamos una consulta con la base de datos
                    OracleCommand comando = new OracleCommand("select * from TRABAJADOR where (Correo='" + txt_Usuario.Text + "' and CONTRASENA='" + txt_contrasena.Text + "') and (ID_PUESTO = 2)", ora);
                    //realizamos un adaptador para verificar
                    OracleDataAdapter adap = new OracleDataAdapter(comando);
                    DataTable tabla = new DataTable();
                    //el adaptador lo llenamos con el valor rescatado en la consulta de la bd
                    adap.Fill(tabla);
                    //si encuentra el valor o es mayor a cero
                    if (tabla.Rows.Count > 0)
                    {
                        
                        //creara un nuevo panel principal
                        PanelPrincipal inicio = new PanelPrincipal();
                        inicio.Show();//mostrara el Panel
                        this.Hide();//ocultara el panel de Login
                        MessageBox.Show("Bienvenido al Sistema AURA ","Bienvenido",MessageBoxButtons.OK,MessageBoxIcon.Information);//Mostrara un mensaje de Bienvenida
                        ora.Close();//cerrara la conexion con la BD
                 
                    }
                    else
                    {
                        //modificar mensaje de error
                        MessageBox.Show("Contraseña invalida", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Question);

                        ora.Close();//cerrara la conexion con la BD 
                        //borrara los campos
                        txt_Usuario.Clear();
                        txt_contrasena.Clear();
                    }

                }
                                           
            }
            catch (Exception)
            {
                //si existe otro error no contemplado mostrara el mensaje de error y no se caera 
                MessageBox.Show("El sistema no puede conectarse con el servidor", "Conexión Down", MessageBoxButtons.OK, MessageBoxIcon.Question);
                txt_Usuario.Clear();
                txt_contrasena.Clear();
            }
            ora.Close();

        }

        private void txt_Usuario_Leave(object sender, EventArgs e)
        {
            //captura del dato
            if (validarCorreo(txt_Usuario.Text))
            {

            }
            else
            {
                MessageBox.Show("el correo debe tener el formato ejemplo@gmail.cl, por favor, seleccione un correo válido", "Dirección de correo electrónico no válida",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                txt_Usuario.SelectAll();
                txt_Usuario.Focus();
            }
        }

        private Boolean validarCorreo(string email)//validar formato de correo
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";//formato de correo
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
