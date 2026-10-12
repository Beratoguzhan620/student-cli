using System.IO.Pipes;

namespace TP230OgrenciYonetimUygulamasi
{
    internal class Program
    {
        static Ogrenci Ogr = new Ogrenci();
        static List<Ogrenci> Ogrenciler = new List<Ogrenci>();

        static void Main(string[] args)
        {
            Uygulama();
        }
        static void Uygulama()
        {
            SahteVeri();
            Menu();
            while (true)
            {
                string giris = SecimAl();
                switch (giris)
                {
                    case "1":
                    case "E":
                        OgrenciEkle();
                        break;
                    case "2":
                    case "L":
                        OgrenciListele();
                        break;
                    case "3":
                    case "S":
                        OgrenciSil();
                        break;
                    case "4":
                    case "X":
                        Environment.Exit(0);
                        break;
                }
            }
        }
        static void Menu()
        {
            Console.WriteLine("Öğrenci Yönetim Uygulaması");
            Console.WriteLine("1 - Öğrenci Ekle(E)       ");
            Console.WriteLine("2 - Öğrenci Listele(L)    ");
            Console.WriteLine("3 - Öğrenci Sil(S)        ");
            Console.WriteLine("4 - Çıkış(X)              ");
            Console.WriteLine();
        }
        static void OgrenciEkle()
        {
            string ad, soyad;
            Console.WriteLine("1 - Öğrenci Ekle----------");
            Console.WriteLine($"{Ogrenciler.Count + 1}. Öğrencinin");
           
            while (true)
            {
                Console.Write("No :");
                int no = Convert.ToInt32(Console.ReadLine());
                bool VarMi = false;
                foreach (var Ogr in Ogrenciler)
                {
                    if (Ogr.No == no)
                    {
                        VarMi = true;
                        break;
                    }
                }
                if (VarMi == true)
                {
                    Console.WriteLine("Bu numara zaten kayıtlı! Lütfen başka bir numara girin.");
                }
                else
                {
                    Ogr.No = no;
                    break;
                }
            }

            Console.Write("Adı : ");
            ad = Console.ReadLine();
            Ogr.Ad = ad.Substring(0, 1).ToUpper() + ad.Substring(1).ToLower();
            Console.Write("Soyadı : ");
            soyad = Console.ReadLine();
            Ogr.Soyad = soyad.Substring(0, 1).ToUpper() + soyad.Substring(1).ToLower();
            Console.Write("Şubesi : ");
            Ogr.Sube = Console.ReadLine().ToUpper();

            Console.WriteLine("Öğrenciyi kaydetmek istediğinize emin misiniz? (E/H)");

            string giris = Console.ReadLine().ToUpper();
            switch (giris)
            {
                case "E":
                    Ogrenciler.Add(new Ogrenci {No=Ogr.No,Ad=Ogr.Ad,Soyad=Ogr.Soyad,Sube=Ogr.Sube });
                    Console.WriteLine("Ogrenci eklendi.");
                    break;
                case "H":
                    Console.WriteLine("Ogrenci eklenmedi");
                    break;
                default:
                    Console.WriteLine("Hatali bir giris yaptiniz tekrar deneyiniz: ");
                    break;
            }

        }
        static void OgrenciListele()
        {
            Console.WriteLine("2- Öğrenci Listele-----------");
            if (Ogrenciler.Count == 0)
            {
                Console.WriteLine("Listelenecek Öğrenci Yok.");
            }
            else
            {
                Console.WriteLine("Sıra No".PadRight(9) + "Şube".PadRight(7) + "No".PadRight(7) + "Ad".PadRight(7) + "Soyad".PadRight(7));
                Console.WriteLine("----------------------------------");
                Console.WriteLine();
                int sayac = 0;
                foreach (Ogrenci item in Ogrenciler)
                {
                    sayac += 1;
                    Console.WriteLine($"{sayac.ToString().PadLeft(5).PadRight(9)}{item.Sube.PadLeft(2).PadRight(7)}{item.No.ToString().PadRight(7)}{item.Ad.PadRight(7)}{item.Soyad.PadRight(7)} ");

                }
                Console.WriteLine();
            }



        }
        static void OgrenciSil()
        {


            if (Ogrenciler.Count == 0)
            {
                Console.WriteLine("Listede Silinecek Öğrenci Yok.");
                return;
            }
            else
            {
                Console.WriteLine("3- Öğrenci Sil ----------");
                Console.WriteLine("Silmek istediğiniz öğrencinin");
                Console.Write("No: ");
                int no = Convert.ToInt32(Console.ReadLine());
                if (no!=Ogr.No)
                {
                    Console.WriteLine("Öğrenci Numarası Bulunamadı.");
                    
                }
            
                

                    Ogrenci a = null;

                    foreach (Ogrenci item in Ogrenciler)
                    {
                        if (no == item.No)
                        {
                            a = item;
                        }
                    }
                    if (a != null)
                    {
                        Console.WriteLine("Ad: " + a.Ad);
                        Console.WriteLine("Soyad: " + a.Soyad);
                        Console.WriteLine("Sube : " + a.Sube);

                        Console.WriteLine("Öğrenciyi silmek istediğinize emin misiniz? (E/H)");

                        string giris = Console.ReadLine().ToUpper();
                        switch (giris)
                        {
                            case "E":
                                Ogrenciler.Remove(a);
                                Console.WriteLine("Ogrenci silindi.");
                                break;
                            case "H":
                                Console.WriteLine("Ogrenci silinmedi");
                                break;
                            default:
                                Console.WriteLine("Hatali bir giris yaptiniz tekrar deneyiniz: ");
                                break;
                        }
                    }
                



            }

        }
        static string SecimAl()
        {
            string karakterler = "1234ELSX";
            int sayac = 0;
            while (true)
            {
                sayac++;
                Console.Write("Seçiminiz :");
                string input = Console.ReadLine().ToUpper();
                int index = karakterler.IndexOf(input);
                if (index >= 0)
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Hatalı giriş yaptınız. Lütfen tekrar deneyiniz.");
                    if (sayac == 10)
                    {
                        Console.WriteLine("Üzgünüm çok fazla sayıda hatalı deneme yaptınız. Program kapatılıyor.");
                        Environment.Exit(0);
                    }
                }
            }
        }
        static void SahteVeri()
        {
            Ogrenci o1 = new Ogrenci();

            o1.Ad = "Serkan";
            o1.Soyad = "Kilickap";
            o1.Sube = "A";
            o1.No = 3;
            Ogrenciler.Add(o1);

            Ogrenci o2 = new Ogrenci();

            o2.Ad = "Mahmut";
            o2.Soyad = "Aydin";
            o2.Sube = "B";
            o2.No = 13;
            Ogrenciler.Add(o2);

            Ogrenci o3 = new Ogrenci();

            o3.Ad = "Uzeyir";
            o3.Soyad = "Yaman";
            o3.Sube = "c";
            o3.No = 42;
            Ogrenciler.Add(o3);
        }
    }
}
