using SQLite4Unity3d;
public class Collected_Objects
{

    //the classes in SQLITE ADDING AND RECORDING are in the same name and type as in the sqlite db tables  

    [PrimaryKey,AutoIncrement]
    public int collected_objects_id { get; set; }
    public int gameplay_id { get; set; }
    public int obj_number { get; set; }
    public string obj_position { get; set; }
    public string obj_field { get; set; }
    public bool obj_recorded_on_pad { get; set; }
    public bool obj_collected { get; set; }
    public string obj_collected_by_hand { get; set; }
    public int attempts {get;set;}

    public void SetValues( int gameplay_id, int obj_number, string obj_position, string obj_field, bool obj_recorded_on_pad, bool obj_collected, string obj_collected_by_hand, int attempts)
    {
        //this.collected_objects_id = collected_objects_id;
        this.gameplay_id = gameplay_id;
        this.obj_number = obj_number;
        this.obj_position = obj_position[0].ToString().ToUpper();
        this.obj_field = obj_field;
        this.obj_recorded_on_pad = obj_recorded_on_pad;
        this.obj_collected = obj_collected;
        this.obj_collected_by_hand = obj_collected_by_hand;
        this.attempts = attempts;
    }






}
