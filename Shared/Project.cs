namespace ProjectBank.Shared;

    public class Project
    {
    public int id { get; set; }
    public string name { get; set; }
    public string desc { get; set; }
    public int authorid { get; set; }
    //public ICollection<int> applications { get; set; }
    public int nrOfViews { get; set; }
    public float ratio { get; set; }

    public Project() {}

    public Project(int Id, string name, string desc, int authorid, int nrOfViews, float ratio)
    {
        this.id = Id;
        this.name = name;
        this.desc = desc;
        this.authorid = authorid;
        this.nrOfViews = nrOfViews;
        this.ratio = ratio;

    }
}
