using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace System
{
    public static class ObjectExtensions
    {
       

        public static T To<T>(this object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static int ToInt(this object obje)
        {
            int result;
            try
            {
                result = Convert.ToInt32(obje);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static List<T> To<T>(this List<object> value)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < value.Count; i++)
            {
                list.Add(value[i].To<T>());
            }
            return list;
        }

      

        public static  Image ResizeImage(this Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public static Image ByteToImage(this Byte[] array)
        {
            MemoryStream ms = new MemoryStream(array);
            return Image.FromStream(ms);
        }

        public static Byte [] ImgToByte(this Image img)
        {
            byte[] arr;
            ImageConverter converter = new ImageConverter();
            arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
            return arr;
        }

        /// <summary>
        /// Secilen resmin yolunu alır
        /// </summary>
        public static string GetPicPath(this string path)
        {
            try
            {

                OpenFileDialog openDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"

                };
                DialogResult result = openDialog.ShowDialog();
                if (result == DialogResult.OK && openDialog.FileName != "")
                {
                    path = openDialog.FileName;
                    return path;
                   
                }
                return "";
            }
            catch (Exception ex)
            {

                throw new Exception("GetPicPath\n" + ex.Message);
                
            }
        }
        /// <summary>
        /// Parametre olarak gelen yoldaki resmi Image olarak geriye döndürür.
        /// </summary>
        /// <param name="path"></param>
        public static Image SetPicEdit(this string path)
        {

            try
            {

                Image img = Image.FromFile(path);
                return img;
            }
            catch (Exception ex)
            {

               throw new Exception("SetPicEdit\n" + ex.Message);
            }

        }
    }
}
