namespace AppCreditoff.Model
{
    public class Model_Creditoff
    {
        public int id { get; set; }
        public string logo { get; set; }
        public string name { get; set; }
        public int suma { get; set; }
        public string time_credit { get; set; }
        public string time_check { get; set; }
        public string pereplata { get; set; }
        public string information { get; set; }
        public int logo_count_star { get; set; }
        public string url { get; set; }

        public Model_Creditoff(int Id, string Logo, string Name, int Suma, string Time_credit, string Time_check, string Pereplata, string Information, int Logo_count_star, string Url)
        {
            id = Id;
            logo = Logo;
            name = Name;
            suma = Suma;
            time_credit = Time_credit;
            time_check = Time_check;
            pereplata = Pereplata;
            information = Information;
            logo_count_star = Logo_count_star;
            url = Url;
        }
    }

    public class ModelTime
    {
        public int id { get; set; }
        public double time { get; set; }
        public string name_time { get; set; }

        public ModelTime(int Id, double Time, string Name_time)
        {
            id = Id;
            time = Time;
            name_time = Name_time;
        }
    }
}
