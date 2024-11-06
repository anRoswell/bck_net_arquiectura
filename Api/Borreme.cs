namespace Api
{
    public class Borreme
    {

    }


    public class Rootobject
    {
        public int estado { get; set; }
        public string url { get; set; }
        public string url_origen { get; set; }
        public string mensaje { get; set; }
        public Datos datos { get; set; }
    }

    public class Datos
    {
        public int id_usuario { get; set; }
        public int id_persona { get; set; }
        public string nombres_apellidos { get; set; }
        public string identificacion { get; set; }
        public string tipo_identificacion { get; set; }
        public Zona[] zonas { get; set; }
        public Perfile[] perfiles { get; set; }
        public Menu[] menus { get; set; }
    }

    public class Zona
    {
        public int id_zona { get; set; }
        public string nombre_zona { get; set; }
    }

    public class Perfile
    {
        public int id_perfil { get; set; }
        public string nombre_perfil { get; set; }
    }

    public class Menu
    {
        public int id { get; set; }
        public string label { get; set; }
        public string routerLink { get; set; }
        public string icon { get; set; }
        public int order { get; set; }
        public bool disabled { get; set; }
        public bool clicked { get; set; }
        public bool expanded { get; set; }
        public Item[] items { get; set; }
        public string[] permisos { get; set; }
        public int[] perfiles { get; set; }
    }

    public class Item
    {
        public int id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string routerLink { get; set; }
        public bool disabled { get; set; }
        public bool clicked { get; set; }
        public object[] permisos { get; set; }
        public int[] perfiles { get; set; }
        public bool expanded { get; set; }
        public Item1[] items { get; set; }
    }

    public class Item1
    {
        public int id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string routerLink { get; set; }
        public bool disabled { get; set; }
        public bool clicked { get; set; }
        public object[] permisos { get; set; }
        public int[] perfiles { get; set; }
    }

}
